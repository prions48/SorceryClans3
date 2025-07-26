using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
namespace SorceryClans3.Data.Models
{
	public class ResearchProject
	{
		[Key] public Guid ID { get; set; }
		public Guid FacilityID { get; set; }
		private int PowerPoints { get; set; }
		private int ColorPoints { get; set; }
		private int LastDiscoveryPts { get; set; }
		private int LastDiscoveryClr { get; set; }
		public int CurrentThreshold { get; set; }
		private int PointsToDiscover { get; set; }
		public int PowerThreshold { get { return 1000000 * CurrentThreshold; } }
		public int ColorThreshold { get { if (Color == MagicColor.None) return 0; return 20 * CurrentThreshold * CurrentThreshold; } }
		public List<ResearchMission> Missions { get; set; } = new List<ResearchMission>();
		public List<Guid> TeamIDs { get { return GetTeams.Select(e => e.ID).ToList(); } }
		public List<Guid> TeamResetIDs { get; set; } = new List<Guid>();
		public List<Team> GetTeams { get { return Missions.Where(e => e.Team != null).Select(e => e.Team!).ToList(); } }
		private MagicColor Color { get; set; }
		public MagicColor GetColor { get { return Color; } }
		public ResearchProject(Guid facilityid, MagicColor color)
		{
			ID = Guid.NewGuid();
			FacilityID = facilityid;
			LastDiscoveryPts = 0;
			LastDiscoveryClr = 0;
			PowerPoints = 0;
			Color = color;
			SetThreshold(1);
		}
		public void SetThreshold(int threshold)
		{
			Random r = new();
			CurrentThreshold = threshold;
			PointsToDiscover = PowerThreshold + r.Next(1000000);
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
					team.MissionID = FacilityID;
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
			int addpts = team.ResearchPower(Color);
			PowerPoints += addpts;
			IList<MagicColor> tcolors = team.GetColors;
			ColorPoints += tcolors.Count(e => e == this.Color);
			Random r = new Random();
			if (PowerPoints - LastDiscoveryPts >= PointsToDiscover && ColorPoints - LastDiscoveryClr >= ColorThreshold + r.Next(ColorThreshold))
			{
				//discover!
				//reset stats
				int discppts = PowerPoints - LastDiscoveryPts;
				int disccpts = ColorPoints - LastDiscoveryClr;
				LastDiscoveryPts = PowerPoints;
				LastDiscoveryClr = ColorPoints;
				SetThreshold(CurrentThreshold);
				return new SpellDiscovery()
				{
					Color = this.Color,
					PowerPoints = discppts,
					ColorPoints = disccpts
				};
			}
			else
			{
				//this bit is because if color is lagging, points get inflated
				if (PowerPoints - LastDiscoveryPts > PointsToDiscover)
					PowerPoints = LastDiscoveryPts + PointsToDiscover + 1;
				if (ColorPoints - LastDiscoveryClr > ColorThreshold * 2)
					ColorPoints = LastDiscoveryClr + ColorThreshold * 2;
			}
			return null;
		}
		public string Progress
		{
			get
			{
				double pctprogress = (PowerPoints - LastDiscoveryPts) * 1.0 / PowerThreshold;
				double clrprogress = 1;
				if (ColorThreshold > 0)
					clrprogress = (ColorPoints - LastDiscoveryClr) * 1.0 / ColorThreshold;
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
				IList<int> thresholds = new List<int>() { 1 };
				if (PowerPoints > 5000000 && (ColorPoints > 100 || Color == MagicColor.None))
					thresholds.Add(2);
				if (PowerPoints > 10000000 && (ColorPoints > 300 || Color == MagicColor.None))
					thresholds.Add(3);
				if (PowerPoints > 15000000 && (ColorPoints > 800 || Color == MagicColor.None))
					thresholds.Add(4);
				if (PowerPoints > 35000000 && (ColorPoints > 1500 || Color == MagicColor.None))
					thresholds.Add(5);
				return thresholds;
			}
		}
		public void ResetResets()
		{
			TeamResetIDs = new List<Guid>();
		}
		public string ThresholdDisplay
		{
			get
			{
				switch (CurrentThreshold)
				{
					case 1: return "Humble";
					case 2: return "Small";
					case 3: return "Medium";
					case 4: return "Large";
					case 5: return "Extravagant";
					default: return "Unknown";
				}
			}
		}
		public bool WarnBeforeChange { get { return (PowerPoints - LastDiscoveryPts) * 1.0 / PowerThreshold >= 0.05; } }
		public static List<(string, int)> Thresholds
		{
			get
			{
				return new List<(string, int)>() { ("Humble", 1), ("Small", 2), ("Medium", 3), ("Large", 4), ("Extravagant", 5) };
			}
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

