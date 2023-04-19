using System;
using Reddit.Shared.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;

namespace Reddit.Client.Services
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

        public async Task<string> CreateUser(User user)
        {
            string url = $"{baseAPI}/api/user";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PostAsJsonAsync(url, user);

            // Get the JSON string from the response
            string response = msg.Content.ReadAsStringAsync().Result;

            // Return the new comment 
            return response;
        }

        public async Task<User> GetUser(string email)
        {
            string url = $"{baseAPI}/api/user/{email}";
            return (await http.GetFromJsonAsync<User>(url))!;
        }

        public async Task<string> voteThread(int threadId, Vote vote)
        {
            string url = $"{baseAPI}/api/votethread/{threadId}";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PostAsJsonAsync(url, vote);

            // Get the JSON string from the response
            string response = msg.Content.ReadAsStringAsync().Result;

            // Return the updated post (vote increased)
            return response;
        }
        public async Task<string> voteComment(int commentId, Vote vote)
        {
            string url = $"{baseAPI}/api/votecomment/{commentId}";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PostAsJsonAsync(url, vote);

            // Get the JSON string from the response
            string response = msg.Content.ReadAsStringAsync().Result;

            // Return the updated post (vote increased)
            return response;
        }
    }
}

