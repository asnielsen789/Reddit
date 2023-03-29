using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reddit_miniProjekt.Shared.Models
{
	public class Comment
	{
		public long CommentId { get; set; }
        public string Content { get; set; }

        [JsonIgnore]
        public string createdAt { get; set; }

        public User User { get; set; }
        public List<Vote> Votes { get; set; } = new List<Vote>();


        public Comment()
		{
		}
		
		public Comment(string content, DateTime createAt, User user)
		{
			this.Content = content;
			this.CreatedAt = createAt;
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

        public int calculateVotes()
        {
            int total = 0;
            foreach (Vote vote in Votes)
            {
                if (vote.Evaluation == true)
                {
                    total++;
                }
                else
                {
                    total--;
                }
            }

            return total;

        }
    }
}

