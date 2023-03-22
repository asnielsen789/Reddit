using System;
namespace reddit_miniProjekt.Shared.Models
{
	public class User
	{
		public long UserId { get; set; }
        public string Name { get; set; }

		public User()
		{
		}

		public User(string name)
		{
			this.Name = name;
		}
	}
}

