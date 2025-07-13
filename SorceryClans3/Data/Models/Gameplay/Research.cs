using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace SorceryClans3.Data.Models
{
	public class Research
	{
		public IList<ResearchFacility> Facilities { get; set; } = new List<ResearchFacility>();
		public List<Team> GetAllTeams { get { return Facilities.SelectMany(e => e.Teams).ToList(); } }
		public List<Team> GetAvailableTeams { get { return Facilities.SelectMany(e => e.Teams.Where(f => f.MissionID == e.ID)).ToList(); } }
		IDictionary<MagicColor, BodyOfKnowledge> Body { get; set; } = new Dictionary<MagicColor,BodyOfKnowledge>();
		public Action<List<Soldier>> CreateSoldiers { get; set; }
		public Action<Artifact> AddArtifact { get; set; }
		Random r = new Random();
		public IList<Spell> Spells { get; set; } = new List<Spell>();
		public Research(Action<List<Soldier>> createSoldiers, Action<Artifact> addArtifact)
		{
			CreateSoldiers = createSoldiers;
			AddArtifact = addArtifact;
		}
		public void AddFacility(int n = 1)
		{
			Facilities.Add(new ResearchFacility(AddKnowledge) { NumTeams = n });
		}
		public void AddKnowledge()
		{
			foreach (ResearchFacility facility in Facilities)
			{
				foreach (Team team in facility.Teams)
				{
					AddColorFromTeam(team);
				}
			}
		}
		public void UpgradeFacility(Guid facilityid, int n)
		{
			ResearchFacility? fac = Facilities.FirstOrDefault(e => e.ID == facilityid);
			if (fac != null)
				fac.NumTeams = n;
		}
		public void StartProject(ResearchFacility facility, MagicColor Color = MagicColor.None)
		{
			if (!Body.ContainsKey(Color))
			{
				Body.Add(Color, new BodyOfKnowledge());
			}
			ResearchProject project = new ResearchProject(facility.ID, Color)
			{
				CurrentThreshold = 1
			};
			facility.Project = project;
		}
		public void AddTeamToProject(ResearchProject project, Team team)
		{
			project.StartMission(team);
		}
		public void AddColorFromTeam(Team team)
		{
			foreach (Soldier soldier in team.GetAllSoldiers)
			{
				IList<MagicColor> colors = soldier.GetColors;
				foreach (MagicColor color in colors)
					if (!Body.ContainsKey(color))
						Body.Add(color, new BodyOfKnowledge());
			}
		}
		public HashSet<MagicColor> AvailableColors
		{
			get
			{
				HashSet<MagicColor> colors = Body.Keys.ToHashSet();
				colors.Add(MagicColor.None);
				return colors;
			}
		}
		public List<string> IncrementDay()
		{
			List<string> msgs = [];
			List<ProjectResult> results = [];
			List<SpellCastMission> results2 = [];
			foreach (ResearchFacility facility in Facilities)
			{
				results2.AddRange(facility.IncrementDay());
				CreateSoldiers.Invoke(results2.SelectMany(e => e.AddedSoldiers).ToList());
				foreach (SpellCastMission mission in results2)
				{
					if (mission.CastingSpell.GeneratesArtifact)
					{
						AddArtifact.Invoke(mission.CastingSpell.GenerateArtifact()!);
					}
				}
				if (facility.Project != null)
				{
					results.Add(facility.Project.IncrementDay());
				}
			}
			msgs.AddRange(results2.Select(e => e.CreateMessage()));
			foreach (ProjectResult result in results)
			{
				Body[result.Color].PowerPoints += result.PowerPoints;
				Body[result.Color].ColorPoints += result.ColorPoints;
				foreach (SpellDiscovery disco in result.Discoveries)
				{
					MagicColor spellColor = disco.Color;
					IList<Spell> currentSpells = Spells.Where(e => e.Color == spellColor).ToList();
					Spell? NewSpell = null;
					int powerpts = disco.PowerPoints;
					switch (spellColor)
					{
						case MagicColor.None: NewSpell = new Spell(spellColor, ResearchDiscovery.Power, powerpts); break;
						case MagicColor.Black:
							if (powerpts.PointsToScore(ResearchDiscovery.LesserUndead) * r.NextDouble() < 3.0)
							{
								NewSpell = new Spell(spellColor, ResearchDiscovery.LesserUndead, powerpts);
							}
							//to add: necromancy artifact with trapped ghost
							else
							{
								NewSpell = new Spell(spellColor, ResearchDiscovery.GreaterUndead, powerpts);
							}
							break;
						case MagicColor.Red:
							if (r.Next(powerpts) < 2000000)
							{
								NewSpell = new Spell(spellColor, ResearchDiscovery.LesserDemon, powerpts);
							}
							//to add: curse as separate discovery and put in artifact
							else
							{
								NewSpell = new Spell(spellColor, ResearchDiscovery.GreaterDemon, powerpts);
							}
							break;
						case MagicColor.Blue:
							if (r.Next(powerpts) < 2000000)
							{
								NewSpell = new Spell(spellColor, ResearchDiscovery.SpiritSoldier, powerpts);
							}
							//to add: weather control spell to boost mercenary mission or cover retreat of hurt team
							else
							{
								NewSpell = new Spell(spellColor, ResearchDiscovery.SpiritArtifact, powerpts);
							}
							break;
						case MagicColor.Green:
							IList<Spell> powerspells = currentSpells.Where(e => e.Power != null).ToList();
							IList<Spell> harvestspells = currentSpells.Where(e => e.Beast != null && e.Beast.AvailableForHarvest).ToList();
							if (powerspells.Count == 0 || powerspells.Count < r.NextDouble() * 3 || r.NextDouble() < .03)
							{
								NewSpell = new Spell(MagicColor.Green, ResearchDiscovery.Power, powerpts);
							}
							else if (!currentSpells.Any(e => e.BeastPet != null)) //tmp
							{
								NewSpell = new Spell(MagicColor.Green, ResearchDiscovery.BeastPet, powerpts);
							}
							else if (harvestspells.Count > 0 && r.NextDouble() < .3)
							{
								IList<Guid> weightedaverage = new List<Guid>();
								foreach (Spell harvest in harvestspells)
								{
									for (int i = harvest.Beast!.NumTamed; i > 0; i--)
										weightedaverage.Add(harvest.Beast.ID);
								}
								Beast? beast = harvestspells.Select(e => e.Beast).FirstOrDefault(e => e != null && e.ID == weightedaverage[r.Next(weightedaverage.Count)]);
								if (beast != null)
								{
									BeastHarvest? harvest = beast.CreateHarvest(disco.PowerPoints.PointsToScore(ResearchDiscovery.BeastHarvest));
									if (harvest == null)
									{
										NewSpell = new Spell(MagicColor.Green, ResearchDiscovery.Power, powerpts); //fallback
									}
									else
									{
										NewSpell = new Spell(MagicColor.Green, ResearchDiscovery.BeastHarvest, powerpts)
										{
											Harvest = harvest
										};
									}
								}
							}
							//to add: magic domestication
							else
							{
								NewSpell = new Spell(MagicColor.Green, ResearchDiscovery.BeastTame, powerpts);
							}
							break;
						case MagicColor.White: NewSpell = new Spell(spellColor, ResearchDiscovery.Angel, powerpts);
							//to add: nephilim
							break;
						case MagicColor.Purple: NewSpell = new Spell(spellColor, ResearchDiscovery.SummonFaerie, powerpts);
							//to add: all the things
							break;
						default: break;
					}
					if (NewSpell != null)
					{
						Spells.Add(NewSpell);
						if (NewSpell.Beast?.TamePower != null)
						{
							//NewSpell.Power = NewSpell.Beast.TamePower;
							Spell spell2 = new Spell(MagicColor.Green, ResearchDiscovery.Power, powerpts)
							{
								Power = NewSpell.Beast.TamePower
							};
							Spells.Add(spell2);
						}
						msgs.Add("New discovery: " + NewSpell.SpellName + " resulted from " + powerpts + " pts and " + disco.ColorPoints + " color.");
					}
				}
				foreach (Team team in result.Completed)
				{
					bool found = false;
					foreach (ResearchFacility facility in Facilities)
					{
						if (facility.Project != null)
						{
							foreach (Guid teamid in facility.Project.TeamResetIDs)
							{
								if (teamid == team.ID)
								{
									facility.Project.StartMission(team);
									//msgs.Add(team.TeamName + " cycled to new mission");
									found = true;
									break;
								}
							}
						}
						if (found)
							break;
					}
					//if (!found)
						//msgs.Add(team.TeamName + " released from research duty.");
				}
			}
			return msgs;
		}
	}
	public class BodyOfKnowledge
	{
		public int PowerPoints { get; set; } = 0;
		public int ColorPoints { get; set; } = 0;
	}
}

