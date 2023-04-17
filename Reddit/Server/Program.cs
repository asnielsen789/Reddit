using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Reddit.Server.Services;
using Reddit.Server.Context;
using Reddit.Shared.Models;
using System.Threading;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
    {
        c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
        };
    });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("reddit:read-write", p => p.
        RequireAuthenticatedUser().
        RequireClaim("scope", "reddit:read-write"));
});

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Sætter CORS så API'en kan bruges fra andre domæner
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


// Tilføj DbContext factory som service.
builder.Services.AddDbContext<RedditContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));

// Tilføj DataService så den kan bruges i endpoints
builder.Services.AddScoped<DataService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(AllowSomeStuff);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Seed data hvis nødvendigt.
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    dataService.SeedData(); // Fylder data på, hvis databasen er tom. Ellers ikke.
}
// Middlware der kører før hver request. Sætter ContentType for alle responses til "JSON".
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

///
/// Minimal API
/// 
/*
app.MapGet("/", (DataService service) =>
{
    return new { message = "Hello World!" };
});
*/

app.MapGet("/api/threads", (DataService service) =>
{
    return service.GetRedditThreads();
}).RequireAuthorization("reddit:read-write");

app.MapGet("/api/thread/{id}", (DataService service, int id) =>
{
    return service.GetRedditThread(id);
}).RequireAuthorization("reddit:read-write");

app.MapPost("/api/thread", (DataService service, RedditThread redditThread) =>
{
    try
    {
        return service.CreateRedditThread(redditThread);
        //return service.GetRedditThreads().FirstOrDefault(redditThread);
    }
    catch (Exception e)
    {
        return e.ToString();
    }
}).RequireAuthorization("reddit:read-write");

app.MapPost("/api/comment/{threadId}", (DataService service, Comment comment, int threadId) =>
{
    try
    {
        return service.CreateComment(threadId, comment);
    }
    catch (Exception e)
    {
        return e.ToString();
    }
}).RequireAuthorization("reddit:read-write");

app.MapPost("/api/votethread/{threadId}", (DataService service, Vote vote, int threadId) =>
{
    try
    {
        var thread = service.GetRedditThread(threadId)!;
        return service.CreateVote(thread, vote);
    }
    catch (Exception e)
    {
        return e.ToString();
    }
}).RequireAuthorization("reddit:read-write");

app.MapPost("/api/votecomment/{commentId}", (DataService service, Vote vote, int commentId) =>{
    try
    {
        var comment = service.GetComment(commentId)!;
        return service.CreateVote(comment, vote);
    }
    catch (Exception e)
    {
        return e.ToString();
    }
}).RequireAuthorization("reddit:read-write");

app.Run();

