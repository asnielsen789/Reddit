using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Reddit.Server.Services;
using Reddit.Server.Context;
using Reddit.Shared.Models;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

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
});

app.MapGet("/api/thread/{id}", (DataService service, int id) =>
{
    return service.GetRedditThread(id);
});

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
});

app.MapPost("/api/user", (DataService service, User user) =>
{
    try
    {
        return service.CreateUser(user);
        //return service.GetRedditThreads().FirstOrDefault(redditThread);
    }
    catch (Exception e)
    {
        return e.ToString();
    }
});

app.MapGet("/api/user/{email}", (DataService service, string email) =>
{
    return service.GetUser(email);
});

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
});

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
});

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
});

app.Run();

