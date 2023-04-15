using System;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Reddit.Shared.Models;
using Reddit.Server.Context;
using System.Threading;

namespace Reddit.Server.Services
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
                db.Threads.Add(new RedditThread("EAAA","Det er fedt at gå i skole her", DateTime.Now, user1));
                db.Threads.Add(new RedditThread("EAAA 3d", "Det er fedt at 3d printe", DateTime.Now, user2));
                db.Threads.Add(new RedditThread("EAAA food", "Maden skal være billigere", DateTime.Now, user1));
                db.Threads.Add(new RedditThread("EAAA indoor setting", "Bedre stole i klasserne", DateTime.Now, user1));
                db.SaveChanges();
                var thread = db.Threads.FirstOrDefault()!;
                thread.Comments.Add(new Comment("helt enig!", DateTime.Now, user2));
                thread.Comments.Add(new Comment("Du har helt ret", DateTime.Now, user3));
                db.SaveChanges();
                thread.Votes.Add(new Vote(true, user2));
                thread.Votes.Add(new Vote(false, user1));
                var comment = thread.Comments.FirstOrDefault()!;
                comment.Votes.Add(new Vote(true, user3));
                comment.Votes.Add(new Vote(false, user1));
                db.SaveChanges();
            }
        }

        public List<RedditThread> GetRedditThreads()
        {
            return db.Threads
                    .Include(t => t.User)
                    .Include(t => t.Votes)
                    .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                    .Include(t => t.Comments)
                    .ThenInclude(c => c.Votes)
                    .ToList();
        }

        public RedditThread? GetRedditThread(int id)
        {
            try
            {
                return db.Threads
                    .Include(t => t.User)
                    .Include(t => t.Votes)
                    .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                    .Include(t => t.Comments)
                    .ThenInclude(c => c.Votes)
                    .FirstOrDefault(t => t.RedditThreadId == id);
            }catch
            {
                return null;
            }
        }

        public User? GetUser(int id)
        {
            try
            {
                return db.Users
                    .FirstOrDefault(c => c.UserId == id);
            }
            catch
            {
                return null;
            }
        }

        public Comment? GetComment(int id)
        {
            try
            {
                return db.Comments
                    .Include(c => c.User)
                    .Include(c => c.Votes)
                    .FirstOrDefault(c => c.CommentId == id);
            }
            catch
            {
                return null;
            }
        }

        public string CreateRedditThread(RedditThread redditThread)
        {
            var thread = new RedditThread(redditThread.Title, redditThread.Content, DateTime.Now, GetUser((int)redditThread.User.UserId)!);
            db.Threads.Add(thread);
            db.SaveChanges();
            return "redditThread created";
        }

        public string CreateComment(int threadId, Comment comment)
        {
            var thread = db.Threads.FirstOrDefault(t => t.RedditThreadId == threadId); ;
            if (thread != null)
            {
                var c = new Comment(comment.Content, DateTime.Now, GetUser((int)comment.User.UserId)!);
                thread.Comments.Add(c);
                db.SaveChanges();
                return "comment created";
            }
            else
            {
                return "no thread found";
            }
        }

        public string CreateVote(RedditThread redditThread, Vote vote)
        {
            var thread = db.Threads.FirstOrDefault(t => t.RedditThreadId == redditThread.RedditThreadId);
            if (thread != null)
            {
                var v = new Vote(vote.Evaluation, GetUser((int)vote.User.UserId)!);
                thread.Votes.Add(v);
                db.SaveChanges();
                return "vote created";
            }
            else
            {
                return "no thread found";
            }
        }
        
        public string CreateVote(Comment comment, Vote vote)
        {
            var c = db.Comments.FirstOrDefault(c => c.CommentId == comment.CommentId);
            if (c != null)
            {
                var v = new Vote(vote.Evaluation, GetUser((int)vote.User.UserId)!);
                c.Votes.Add(v);
                db.SaveChanges();
                return "vote created";
            }
            else
            {
                return "no comment found";
            }
        }
        
    }
}