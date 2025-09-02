using Microsoft.Identity.Client;

namespace SorceryClans3.Data.Models
{
    public class GameEngine
    {
        public GameSettings Settings { get; set; } = new();
        public List<GameEvent> Events { get; set; } = [];
        public List<GameEvent> VisibleEvents { get { return Events.Where(e => e.Visible).ToList(); } }
        public List<Mission> Missions { get; set; } = [];
        public List<Clan> Clans { get; set; } = [];
        public List<StyleTemplate> ExtraStyles { get; set; } = [];
        public List<StyleTemplate> AllStyles
        {
            get
            {
                List<StyleTemplate> ret = ExtraStyles.ToList();
                foreach (Clan clan in Clans)
                    if (clan.Style != null)
                        ret.Add(clan.Style);
                return ret;
            }
        }
        public List<Soldier> AllSoldiers { get; set; } = [];
        public List<Soldier> Soldiers { get { return AllSoldiers.Where(e => e.IsActive).ToList(); } }
        public List<GameEvent> InProgressMissions
        {
            get
            {
                return Events.Where(e => e.MissionToComplete != null).ToList();
            }
        }
        public List<Team> Teams { get; set; } = [];
        public Academy Academy { get; set; } = new();
        public List<ClientCity> Clients { get; set; } = [];
        public Resources Resources { get; set; } = new(); //home base resources
        public Defenses Defenses { get; set; } = new();
        public Research Research { get; set; }
        private Random r = new();
        public GameEngine()
        {
            Research = new(CreateSoldiers, AddArtifact);
        }
        private void CreateSoldiers(List<Soldier> soldiers)
        {
            AllSoldiers.AddRange(soldiers);
        }
        private void AddArtifact(Artifact artifact)
        {
            Resources.Artifacts.Add(artifact);
        }
        public void StartMission(Mission mission, Team team)
        {
            mission.AttemptingTeam = team;
            team.MissionID = mission.ID;
            Events.Add(new(mission, Settings.MissionEndTime(mission)));
        }
        public void StartContractMission(MissionContract mission, Team team, bool first = false)
        {
            mission.AttemptingTeam = team;
            team.MissionID = mission.ID;
            Events.Add(new(mission, Settings.MissionEndTime(mission), false));
            if (first)
                Events.Add(new(mission, Settings.PayDay(mission),true));
        }
        public void AssignDefense(Team team, DefenseStructure structure)
        {
            structure.Team = team;
            team.MissionID = structure.ID;
            Events.Add(new(team, MissionType.Defending, Settings.DefenseDate(structure.Type), structure));
        }
        public void StartHunt(Team team, Spell spell, Soldier? target = null)
        {
            team.MissionID = spell.ID;
            Events.Add(new(team, spell, target, Settings.HuntDate(spell)));
        }
        public void UnassignDefense(DefenseStructure structure)
        {
            if (structure.Team != null)
            {
                List<GameEvent> events = Events.Where(e => e.TeamInTransit?.ID == structure.Team.ID && e.Type == MissionType.Defending).ToList();
                structure.Team.MissionID = null;
                structure.Team = null;
                int ctr = 0;
                while (ctr < Events.Count) //remove cycling missions
                {
                    if (events.Contains(Events[ctr]))
                    {
                        Events.RemoveAt(ctr);
                    }
                    else
                        ctr++;
                }
            }
        }
        public void StartBuilding(DefenseType type)
        {
            Events.Add(new(type, Settings.BuildTime(type)));
            Resources.LoseMoney(type.Cost());
        }
        public void StartResourceRun(ClientCity city, Team team, bool roundtrip)
        {
            team.Resources.TransferResources(city.Resources);
            Events.Add(new(team, Settings.TravelCompletion(city, team), city, roundtrip));
                team.MissionID = Guid.Empty;
            if (!roundtrip)
            {
                team.MissionID = Guid.Empty;
                team.Location = null;
                city.RemoveTeam(team);
            }
        }
        public void StartRescueMission(Team rescuer, Team rescuee, MapLocation location)
        {
            rescuer.MissionID = rescuee.ID;
            Events.Add(new(rescuer, rescuee, location, Settings.TravelCompletion(rescuer.Location ?? MapLocation.HomeBase, location, rescuer.DScore)));
        }
        public void StartTrainingMission(Team team, Soldier soldier)
        {
            team.MissionID = soldier.ID;
            Events.Add(new(team, MissionType.LeadershipTraining, Settings.TrainingDate(), soldier));
        }
        public void StartMedTrainMission(Team team, Soldier medic)
        {
            team.MissionID = medic.ID;
            Events.Add(new(team, MissionType.MedicalTraining, Settings.TrainingDate(), medic));
        }
        public void StartStyleTrainMission(Team team, Soldier teacher, StyleTemplate template)
        {
            team.MissionID = teacher.ID;
            Events.Add(new(team, MissionType.StyleTraining, Settings.TrainingDate(), teacher, template));
        }
        public List<DefenseType> InProgressBuildings(DefenseType? type = null)
        {
            return Events.Where(e => e.DefenseType != null && (type == null || e.DefenseType == type)).Select(e => e.DefenseType!.Value).ToList();
        }
        public void TeamCityTravel(ClientCity city, Team team, bool liaison, bool returning = false)
        {
            if (liaison)
            {
                if (returning)
                    city.Liaisons.Remove(team);
                else
                {
                    if (!city.Liaisons.Contains(team))
                        city.Liaisons.Add(team);
                }
            }
            else
            {
                if (returning)
                    city.Teams.Remove(team);
                else
                {
                    if (!city.Teams.Contains(team))
                        city.Teams.Add(team);
                }
            }
            team.MissionID = returning ? Guid.Empty : city.ID;
            Events.Add(new(team, Settings.TravelCompletion(MapLocation.HomeBase, /*this needs cleanup later*/ city.Location, team.DScore), liaison && !returning ? MissionType.LiaisonAtLocation : MissionType.TravelToLocation, returning ? MapLocation.HomeBase : city.Location));
        }
        public List<GameEvent> IncrementTime() //called internally
        {
            if (!Settings.RealTime)
            {
                Settings.GameTime = Settings.GameTime.AddHours(6);//4x/day for now
            }

            //add random things
            List<GameEvent> randoms = GenerateRandomEvents();
            Events.AddRange(randoms);
            List<GameEvent> events = [];

            if (Settings.HealTime)
            {
                List<Soldier> deads = [];
                foreach (Soldier soldier in Soldiers)
                {
                    soldier.Medical?.Rest(Settings);
                    if (soldier.RemainingActive.HasValue)
                    {
                        soldier.RemainingActive--;
                        if (soldier.RemainingActive <= 0)
                        {
                            soldier.Retired = true;
                            if (soldier.SubTo != null)
                            {
                                if (soldier.SubTo.Artifact is NecroArtifact necro)
                                {
                                    necro.TargetID = null;
                                    necro.UndeadID = null;
                                }
                                soldier.SubTo.SubSoldiers.Remove(soldier);
                                soldier.SubTo = null;
                            }
                            if (soldier.Team != null)
                            { 
                                deads.AddRange(soldier.Team.RemoveSoldier(soldier));
                            }
                        }
                    }
                    if (!soldier.Type.Bleeds())
                    {
                        if (soldier.HPCurrent < soldier.HPMax)
                            soldier.HPCurrent++;
                        continue;
                    }
                    if (soldier.HPCurrent < soldier.HPMax)
                        {
                            //soldier.MedicalHeal(5);//experimental failure?
                            if (soldier.Health <= HealthLevel.Hurt)
                            {
                                if (r.Next(3) == 0)
                                {
                                    soldier.HPCurrent++;
                                }
                            }
                            else
                            {
                                if (r.Next(3) == 0)
                                {
                                    soldier.Hurt(1);
                                    if (!soldier.IsAlive)
                                    {
                                        deads.Add(soldier);
                                    }
                                }
                            }
                        }
                }
                if (deads.Count > 0)
                    events.Add(new(deads, Settings.CurrentTime));
            }
            //process
            int i = 0;
            while (i < Events.Count)
            {
                if (Events[i].EventCompleted <= Settings.CurrentTime)
                {
                    events.Add(Events[i]);
                    Events.RemoveAt(i);
                }
                else
                    i++;
            }
            return events;
        }
        public List<GameEventDisplay> ResolveEvents()//called externally
        {
            List<GameEvent> events = IncrementTime();
            List<GameEventDisplay> displays = [];
            foreach (ClientCity city in Clients)
            {
                displays.AddRange(city.NewMissionCycle(Settings));
            }
            foreach (GameEvent ev in events)
            {
                GameEventDisplay? disp = null;
                GameEventDisplay? merc = null;
                switch (ev.Type)
                {
                    case MissionType.Announcement:
                        displays.Add(ev.Announcement());
                        break;
                    case MissionType.TravelToLocation:
                        disp = ev.ResolveReturn();
                        displays.Add(disp);
                        break;
                    case MissionType.LiaisonAtLocation:
                        displays.Add(ev.ResolveReturn(true));
                        break;
                    case MissionType.PayDay:
                        //to implement here
                        var payday = ev.ResolvePayday();
                        if (payday != null)
                            displays.Add(payday);
                        MissionContract? contract = ev.MissionToComplete as MissionContract;
                        if (contract != null)
                            Events.Add(new(contract, Settings.PayDay(contract), true));
                        break;
                    case MissionType.Mercenary:
                        merc = ev.ResolveMercenary();
                        //remove? or only if !failed?... for now only 1 try
                        //could allow retries on certain missions, do that logic here
                        merc.DisplayMission!.Client.FinishMission(merc);
                        merc.DisplayTeam!.MissionID = Guid.Empty;
                        displays.Add(merc);
                        if (merc.DisplayTeam.GetAllSoldiers.Any(e => e.IsAlive))
                        {
                            Events.Add(new GameEvent(merc.DisplayTeam, Settings.MissionTravelTime(merc.DisplayMission!), MissionType.TravelToLocation, merc.DisplayTeam!.Location ?? MapLocation.HomeBase));
                        }
                        else
                        {
                            Teams.Remove(merc.DisplayTeam);
                            merc.DisplayMission.Client.RemoveTeam(merc.DisplayTeam);
                            displays.Add(new("Team " + merc.DisplayTeam.TeamName + " has been wiped out!", Settings.CurrentTime));
                        }
                        break;
                    case MissionType.ContractMisson:
                        var merc2 = ev.ResolveMercenary();
                        displays.Add(merc2);
                        var toll = merc2.DisplayMission!.Client.TollContract(merc2);
                        if (toll != null)
                        {
                            merc2.DisplayTeam!.MissionID = Guid.Empty;
                            Events.Add(new GameEvent(merc2.DisplayTeam, Settings.MissionTravelTime(merc2.DisplayMission!), MissionType.TravelToLocation, merc2.DisplayTeam!.Location ?? MapLocation.HomeBase));
                            displays.Add(toll);
                        }
                        else
                        {
                            MissionContract? contract2 = merc2.DisplayMission as MissionContract;
                            if (contract2 != null && merc2.DisplayTeam != null)
                                StartContractMission(contract2, merc2.DisplayTeam);
                        }
                        //do other things later?
                        break;
                    case MissionType.BanditAttack:
                        displays.Add(new("BANDITS HAVE ATTACKED!", Settings.CurrentTime));
                        break;
                    case MissionType.Building:
                        Defenses.BuildStructure(ev.DefenseType!.Value);
                        displays.Add(ev.ResolveBuilding());
                        break;
                    case MissionType.ResourceTransit:
                        if (ev.TeamInTransit != null)
                        {
                            Resources.TransferResources(ev.TeamInTransit.Resources);
                            displays.Add(ev.ResolveResourceTransit());
                            if (ev.RoundTrip == true)
                            {
                                TeamCityTravel(ev.City!, ev.TeamInTransit, false, false);
                            }
                        }
                        break;
                    case MissionType.Defending: //defense assignment cycling
                        if (ev.TeamInTransit != null && ev.DefenseStructure != null)
                        {
                            ev.ResolveDefensePatrol();
                            //displays.Add(ev.ResolveDefensePatrol());//for testing
                            Events.Add(new(ev.TeamInTransit, MissionType.Defending, Settings.DefenseDate(ev.DefenseStructure.Type), ev.DefenseStructure));
                        }
                        break;
                    case MissionType.LeadershipTraining:
                        disp = ev.ResolveLeadershipTraining();
                        displays.Add(disp);
                        break;
                    case MissionType.MedicalTraining:
                        displays.Add(ev.ResolveMedicalTraining());
                        break;
                    case MissionType.StyleTraining:
                        displays.Add(ev.ResolveStyleTraining());
                        break;
                    case MissionType.MedicalRescue:
                        displays.Add(ev.ResolveRescue());
                        Events.Add(new GameEvent(ev.TeamInTransit!, Settings.TravelCompletion(ev.Destination!, ev.TeamInTransit!.Location ?? MapLocation.HomeBase, ev.TeamInTransit.DScore), MissionType.TravelToLocation, ev.TeamInTransit!.Location ?? MapLocation.HomeBase));
                        break;
                    case MissionType.TameBeast:
                        disp = ev.ResolveHunt();
                        if (disp.NewSoldier != null)
                            AllSoldiers.Add(disp.NewSoldier);
                        displays.Add(disp);
                        break;
                    default:
                        displays.Add(new("Undefined game event", Settings.CurrentTime));
                        break;
                }
                if (disp?.DisplayTeam != null && disp.DisplayTeam.IsAtHome && disp.DisplayTeam.GetAllSoldiers.Any(e => e.IsInjured) && Defenses.Structures.Any(e => e.Type == DefenseType.Hospital && e.Team != null))
                {
                    disp.OpenHealDialog = true;
                }
                if (merc?.DisplayTeam != null && merc.DisplayTeam.GetAllSoldiers.Any(e => e.IsInjured))
                {
                    merc.OpenRescueDialog = true;
                }
            }
            //research missions are separate for now
            if (Settings.CurrentTime.Hour == 0)
            {
                List<string> msgs = Research.IncrementDay();
                displays.AddRange(msgs.Select(e => new GameEventDisplay(e, Settings.CurrentTime)));
            }
            //healing soldiers who are on their own team
            if (Settings.HealTime)
            {
                foreach (Team team in Teams)
                {
                    if (!displays.Any(e => e.DisplayTeam?.ID == team.ID) && team.GetAllSoldiers.Any(e => e.IsInjured) && team.GetAllSoldiers.Any(e => e.HealScore > 0))
                    {
                        displays.Add(new($"Team {team.TeamName} must render medical assistance to itself.", Settings.CurrentTime) { OpenHealDialog = true, DisplayTeam = team, DisplayTeam2 = team });
                    }
                }
                Settings.SetNextHeal();
            }
            //resolve lost/consumed artifacts
            int a = 0;
            while (a < Resources.Artifacts.Count)
            {
                Artifact art = Resources.Artifacts[a];
                if (art.Lost)
                {
                    if (art is SpiritWeather weather || art.IconID != null)
                    {
                        Resources.Artifacts.RemoveAt(a);
                        continue;
                    }
                    else
                    {
                        //generate lost artifact mission here right?
                        //displays.Add(new lost artifact mission)
                        a++;
                    }
                }
                else
                    a++;
            }
            return displays;
        }
        private List<GameEvent> GenerateRandomEvents()
        {
            List<GameEvent> randoms = [];
            if (Settings.BanditAttackOdds())//1 in 20, way too high for real
            {
                randoms.Add(new GameEvent(MissionType.BanditAttack, Settings.BanditTime(), true));
            }
            return randoms;
        }
    }
}