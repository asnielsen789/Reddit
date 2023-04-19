using System;
using System.ComponentModel.DataAnnotations;

namespace Reddit.Shared.Models
{
	public class User
	{
		public long UserId { get; set; }
		[Required]
        public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		public User()
		{
		}

		public User(string name, string email)
		{
			this.Name = name;
			this.Email = email;
		}
	}
}

