using System;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using reddit_miniProjekt.Shared.Models;
using reddit_miniProjekt.Server.Context;

namespace reddit_miniProjekt.Server.Services
{
    public class DataService
    {
        private RedditContext db { get; }

        public DataService(RedditContext db)
        {
            this.db = db;
        }
        /// <summary>
        /// Seeder noget nyt data i databasen hvis det er nødvendigt.
        /// </summary>
        public void SeedData()
        {
            RedditThread redditThread = db.Threads.FirstOrDefault()!;
            if (redditThread == null)
            {
                User user1 = new User("Bob morgan");
                User user2 = new User("Leasly Piers");
                User user3 = new User("Peter Hansen");
                db.Threads.Add(new RedditThread("EAAA","Det er fedt at gå i skole her", user1));
                db.Threads.Add(new RedditThread("EAAA 3d", "Det er fedt at 3d printe", user2));
                db.Threads.Add(new RedditThread("EAAA food", "Maden skal være billigere", user1));
                db.Threads.Add(new RedditThread("EAAA indoor setting", "Bedre stole i klasserne", user1));
                db.SaveChanges();
                var thread = db.Threads.FirstOrDefault()!;
                thread.Comments.Add(new Comment("helt enig!", user2));
                thread.Comments.Add(new Comment("Du har helt ret", user3));

                db.SaveChanges();
            }
        }

        public List<RedditThread> GetRedditThreads()
        {
            return db.Threads
                    .Include(t => t.Comments)
                    .Include(t => t.User)
                    .Include(t => t.Votes).ToList();
        }

        public RedditThread? GetRedditThread(int id)
        {
            try
            {
                return db.Threads
                    .Include(t => t.Comments)
                    .Include(t => t.User)
                    .Include(t => t.Votes)
                    .FirstOrDefault(t => t.RedditThreadId == id);
            }catch
            {
                return null;
            }
            
        }

        public string CreateRedditThread(RedditThread redditThread)
        {
            db.Threads.Add(redditThread);
            db.SaveChanges();
            return "redditThread created";
        }

    }
}