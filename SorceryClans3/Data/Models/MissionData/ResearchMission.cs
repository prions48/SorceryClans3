using System;
using System.ComponentModel.DataAnnotations;
using SorceryClans3.Data.Abstractions;

namespace SorceryClans3.Data.Models
{
	public class ResearchMission : IMission
	{
		[Key] public Guid ID { get; set; } = Guid.NewGuid();
		public ResearchProject Project { get; set; }
		public Guid MissionID => ID;
		public string MissionName => $"Research {(Project.GetColor == MagicColor.None ? "" : Project.GetColor.ToString() + " ")}Magic";
		public int NumDays { get; set; }
		public Team? Team { get; set; }
		public bool Cycle { get; set; } = true;
		public bool ShowTeam { get; set; } = false;
		public ResearchMission(ResearchProject project, Team team)
		{
			Project = project;
			Team = team;
			team.AssignMission(this);
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

