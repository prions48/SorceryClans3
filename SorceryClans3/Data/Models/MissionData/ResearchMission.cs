using System;
using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Models
{
	public class ResearchMission
	{
		[Key] public Guid ID { get; set; }
		public int NumDays { get; set; }
		public Team? Team { get; set; }
		public bool Cycle { get; set; } = true;
		public bool ShowTeam { get; set; } = false;
		public ResearchMission(Team team)
		{
			ID = Guid.NewGuid();
			Team = team;
			Random r = new Random();
			//NumDays = 25 + r.Next(12);//real
			NumDays = 5 + r.Next(5);//for testing
		}
		public Team? AdvanceDay()
		{
			if (--NumDays <= 0)
				return Team;
			return null;
		}
	}
}

