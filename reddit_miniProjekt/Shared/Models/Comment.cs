using System;
namespace reddit_miniProjekt.Shared.Models
{
	public class Comment
	{
		public long CommentId { get; set; }
        public string Content { get; set; }

        public User User { get; set; }
        public List<Vote> Votes { get; set; } = new List<Vote>();


        public Comment()
		{
		}
		
		public Comment(string content, User user)
		{
			this.Content = content;
			this.User = user;
		}
		

	}
}

