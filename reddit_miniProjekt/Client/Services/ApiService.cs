using System;
using reddit_miniProjekt.Shared.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;

namespace reddit_miniProjekt.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient http;
        private readonly IConfiguration configuration;
        private readonly string baseAPI = "";

        public ApiService(HttpClient http, IConfiguration configuration)
        {
            this.http = http;
            this.configuration = configuration;
            this.baseAPI = configuration["base_api"]!;
        }

        public async Task<RedditThread[]> GetThreads()
        {
            string url = $"{baseAPI}/api/threads";
            return (await http.GetFromJsonAsync<RedditThread[]>(url))!;
        }

        public async Task<RedditThread> GetThread(int id)
        {
            string url = $"{baseAPI}/api/thread/{id}";
            return (await http.GetFromJsonAsync<RedditThread>(url))!;
        }

        public async Task<string> CreateThread(RedditThread thread)
        {
            string url = $"{baseAPI}/api/thread";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PostAsJsonAsync(url, thread);

            // Get the JSON string from the response
            string response = msg.Content.ReadAsStringAsync().Result;

            // Return the new comment 
            return response;
        }

        public async Task<string> CreateComment(Comment comment, int threadId)
        {
            string url = $"{baseAPI}/api/comment/{threadId}";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PostAsJsonAsync(url, comment);

            // Get the JSON string from the response
            string response = msg.Content.ReadAsStringAsync().Result;

            

            // Return the new comment 
            return response;
        }

        public async Task<string> voteThread(int threadId, Vote vote)
        {
            string url = $"{baseAPI}/api/votethread/{threadId}";

            Console.WriteLine("I'm here - voteThread/apiservice");

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PostAsJsonAsync(url, vote);

            // Get the JSON string from the response
            string response = msg.Content.ReadAsStringAsync().Result;
            /*
            // Deserialize the JSON string to a Post object
            string? response = JsonSerializer.Deserialize<string>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
            });
            */

            // Return the updated post (vote increased)
            return response;
        }
    }
}

