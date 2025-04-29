using System;
namespace SorceryClans3.Data.Models
{
	public class Research
	{
		public IList<ResearchFacility> Facilities { get; set; } = new List<ResearchFacility>();
		IDictionary<MagicColor, BodyOfKnowledge> Body { get; set; } = new Dictionary<MagicColor,BodyOfKnowledge>();
		Random r = new Random();
		public IList<Spell> Spells { get; set; } = new List<Spell>();
		public void AddFacility(int n = 1)
		{
			Facilities.Add(new ResearchFacility { NumTeams = n });
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
			ResearchProject project = new ResearchProject(Color)
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
		public IList<MagicColor> AvailableColors
		{
			get
			{
				IList<MagicColor> Colors = new List<MagicColor>();
				foreach (KeyValuePair<MagicColor,BodyOfKnowledge> color in Body)
				{
					//if (color.Value.PowerPoints > 1000)
					Colors.Add(color.Key);
				}
				if (!Colors.Contains(MagicColor.None))
					Colors.Add(MagicColor.None);
				return Colors;
			}
		}
		public IList<string> IncrementDay()
		{
			IList<string> msgs = new List<string>();
			IList<ProjectResult> results = new List<ProjectResult>();
			foreach (ResearchFacility facility in Facilities)
			{
				if (facility.Project != null)
				{
					results.Add(facility.Project.IncrementDay());
				}
			}
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
						case MagicColor.None: NewSpell = new Spell(spellColor,ResearchDiscovery.Power,powerpts); break; 
						case MagicColor.Black:
							if (powerpts.PointsToScore(ResearchDiscovery.LesserUndead)* r.NextDouble() < 3.0)
							{
								NewSpell = new Spell(spellColor,ResearchDiscovery.LesserUndead,powerpts);
							}
							else
							{
								NewSpell = new Spell(spellColor,ResearchDiscovery.GreaterUndead,powerpts);
							}
							break; 
						case MagicColor.Red:
							if (r.Next(powerpts) < 2000000)
							{
								NewSpell = new Spell(spellColor,ResearchDiscovery.LesserDemon,powerpts);
							}
							else
							{
								NewSpell = new Spell(spellColor, ResearchDiscovery.GreaterDemon, powerpts);
							}
							break; 
						case MagicColor.Blue:
							if (r.Next(powerpts) < 2000000)
							{
								NewSpell = new Spell(spellColor,ResearchDiscovery.SpiritSoldier,powerpts);
							}
							else
							{
								NewSpell = new Spell(spellColor,ResearchDiscovery.SpiritArtifact,powerpts);
							}
							break; 
						case MagicColor.Green:
							IList<Spell> powerspells = currentSpells.Where(e => e.Power != null).ToList();
							IList<Spell> harvestspells = currentSpells.Where(e => e.Beast != null && e.Beast.AvailableForHarvest).ToList();
							if (powerspells.Count == 0 || powerspells.Count < r.NextDouble()*3 || r.NextDouble() < .03)
							{
								NewSpell = new Spell(MagicColor.Green, ResearchDiscovery.Power, powerpts);
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
										NewSpell = new Spell(MagicColor.Green, ResearchDiscovery.Power, powerpts);
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
							else
							{
								NewSpell = new Spell(MagicColor.Green, ResearchDiscovery.BeastTame,powerpts);
							}
							break; 
						case MagicColor.White: NewSpell = new Spell(spellColor,ResearchDiscovery.Angel,powerpts); break; 
						case MagicColor.Purple: NewSpell = new Spell(spellColor,ResearchDiscovery.SummonFaerie,powerpts); break; 
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
						msgs.Add("New discovery: " + NewSpell.SpellName);
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
									msgs.Add(team.TeamName + " cycled to new mission");
									found = true;
									break;
								}
							}
						}
						if (found)
							break;
					}
					if (!found)
						msgs.Add(team.TeamName + " released from research duty.");
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

