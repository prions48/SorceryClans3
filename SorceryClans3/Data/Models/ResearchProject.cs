﻿using System;
using System.ComponentModel.DataAnnotations;
namespace SorceryClans3.Data.Models
{
	public class ResearchProject
	{
		[Key] public Guid ID { get; set; }
		private int PowerPoints { get; set; }
		private int ColorPoints { get; set; }
		private int LastDiscoveryPts { get; set; }
		private int LastDiscoveryClr { get; set; }
		public int CurrentThreshold { get; set; }
		public int PowerThreshold { get { return 1000000 * CurrentThreshold; } }//low for testing
		public int ColorThreshold { get { if (Color == MagicColor.None) return 0; return 25 * CurrentThreshold; } }
		public IList<ResearchMission> Missions { get; set; } = new List<ResearchMission>();
		public IList<Guid> TeamIDs { get { return GetTeams.Select(e => e.ID).ToList(); } }
		public IList<Guid> TeamResetIDs { get; set; } = new List<Guid>();
		public IList<Team> GetTeams { get { return Missions.Where(e => e.Team != null).Select(e => e.Team!).ToList(); } }
		private MagicColor Color { get; set; }
		public MagicColor GetColor { get { return Color; } }
		public ResearchProject(MagicColor color)
		{
			ID = Guid.NewGuid();
			LastDiscoveryPts = 0;
            LastDiscoveryClr = 0;
			PowerPoints = 0;
			Color = color;
		}
		public void StartMission(Team team)
		{
			Random r = new Random();
            foreach (Soldier sold in team.Leaders)
            {
				if (!sold.ResearchSkill.ContainsKey(Color))
					sold.ResearchSkill.Add(Color, 0.2 + r.NextDouble() * 0.2); //(r.NextDouble() * .5) - .75);
            }
			ResearchMission mission = new ResearchMission(team);
            Missions.Add(mission);
			team.MissionID = mission.ID;
		}
		public ProjectResult IncrementDay()
		{
			ResetResets();
			ProjectResult Result = new ProjectResult() { Color = this.Color };
			int i = 0;
			while (i < Missions.Count)
			{
				Team? team = Missions[i].AdvanceDay();
				if (team != null)
				{
					SpellDiscovery? spell = AddProgress(team);
					if (spell != null)
						Result.Discoveries.Add(spell);
					team.MissionID = null;
					if (Missions[i].Cycle)
					{
						TeamResetIDs.Add(team.ID);
					}
					Missions.RemoveAt(i);
					Result.Completed.Add(team);
				}
				else
					i++;
			}
			return Result;
		}
		public SpellDiscovery? AddProgress(Team team)
		{
			if (PowerThreshold == 0)
				return null;
			PowerPoints += team.ResearchPowerIncrement(Color);
			IList<MagicColor> tcolors = team.GetColors;
			ColorPoints += tcolors.Count(e => e == this.Color);
			Random r = new Random();
			if (PowerPoints - LastDiscoveryPts >= PowerThreshold + r.Next(PowerThreshold) && ColorPoints - LastDiscoveryClr >= ColorThreshold + r.Next(ColorThreshold))
			{
				//discover!
				//reset stats
				int discppts = PowerPoints - LastDiscoveryPts;
				int disccpts = ColorPoints - LastDiscoveryClr;
                LastDiscoveryPts = PowerPoints;
                LastDiscoveryClr = ColorPoints;
				return new SpellDiscovery()
				{
					Color = this.Color,
					PowerPoints = discppts,
					ColorPoints = disccpts
				};
			}
			return null;
		}
		public string Progress
		{
			get
			{
				double pctprogress = (PowerPoints - LastDiscoveryPts) * 1.0 / PowerThreshold;
				double clrprogress = (ColorPoints - LastDiscoveryClr) * 1.0 / ColorThreshold;
				double progress = pctprogress;
				if (clrprogress < pctprogress)
					progress = clrprogress;
				if (progress < 0.1)
					return "Initial work";
				if (progress < 0.25)
					return "Hypothesis extension";
                if (progress < 0.5)
                    return "Exploratory phase";
                if (progress < 0.8)
                    return "Development phase";
                if (progress < 0.95)
                    return "Critical revisions";
				return "Finalization phase";
            }
		}
		public IList<int> AvailableThresholds
		{
			get
			{
				IList<int> thresholds = new List<int>(){ 1 };
				if (PowerPoints > 1000000 && (ColorPoints > 100 || Color == MagicColor.None))
					thresholds.Add(2);
				if (PowerPoints > 2000000 && (ColorPoints > 200 || Color == MagicColor.None))
					thresholds.Add(3);
				if (PowerPoints > 3000000 && (ColorPoints > 300 || Color == MagicColor.None))
					thresholds.Add(4);
				if (PowerPoints > 5000000 && (ColorPoints > 500 || Color == MagicColor.None))
					thresholds.Add(5);
				return thresholds;
			}
		}
		public void ResetResets()
		{
			TeamResetIDs = new List<Guid>();
		}
	}
	public class ProjectResult
	{
		public MagicColor Color { get; set; }
		public IList<SpellDiscovery> Discoveries { get; set; } = new List<SpellDiscovery>();
		public IList<Team> Completed { get; set; } = new List<Team>();
		public int PowerPoints { get; set; }
		public int ColorPoints { get; set; }
    }
	public class SpellDiscovery
	{
		public MagicColor Color { get; set; }
		public int PowerPoints { get; set; }
		public int ColorPoints { get; set; }
	}
}

