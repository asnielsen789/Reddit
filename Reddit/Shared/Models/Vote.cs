using System;
namespace Reddit.Shared.Models
{
	public class Vote
	{

		public long VoteId { get; set; }
		public bool Evaluation { get; set; }
		public User User { get; set; }

		public Vote()
		{
		}
		
		public Vote(bool eval, User user)
		{
			Evaluation = eval;
			User = user;
		}
	}
}

