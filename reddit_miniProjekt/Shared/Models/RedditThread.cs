using System;
namespace reddit_miniProjekt.Shared.Models
{
	public class RedditThread
	{
        public long RedditThreadId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
		public User User { get; }

		public List<Vote> Votes { get; set; } = new List<Vote>();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public RedditThread()
		{
		}

		public RedditThread(string title, string content, User user)
		{
			this.Title = title;
			this.Content = content;
			this.User = user;
		}
	}
}

