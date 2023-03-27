using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reddit_miniProjekt.Shared.Models
{
	public class RedditThread
	{
        public long RedditThreadId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

		[JsonIgnore]
		public string createdAt { get; set; }
        public User User { get; set; }

		public List<Vote> Votes { get; set; } = new List<Vote>();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public RedditThread()
		{
		}

		public RedditThread(string title, string content, DateTime createdAt, User user)
		{
			this.Title = title;
			this.Content = content;
			this.CreatedAt = createdAt;
			this.User = user;
		}

		[NotMapped]
        public DateTime CreatedAt
		{
			get 
			{ 
				return DateTime.Parse(this.createdAt);
			}
			set 
			{ 
				this.createdAt = value.ToString(); 
			}
		}
	}
}

