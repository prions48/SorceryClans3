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
        public List<Soldier> Soldiers { get; set; } = [];
        public List<Team> Teams { get; set; } = [];
        public Academy Academy { get; set; } = new();
        public List<ClientCity> Clients { get; set; } = [];
        public Resources Resources { get; set; } = new(); //home base resources
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
        public void TeamCityTravel(ClientCity city, Team team, bool liaison, bool returning = false)
        {
            if (liaison)
            {
                if (returning)
                    city.Liaisons.Remove(team);
                else
                    city.Liaisons.Add(team);
            }
            else
            {
                if (returning)
                    city.Teams.Remove(team);
                else
                    city.Teams.Add(team);
            }
            team.MissionID = returning ? Guid.Empty : city.ID;
            Events.Add(new(team, Settings.TravelCompletion(MapLocation.HomeBase, /*this needs cleanup later*/ city.Location, team.DScore), liaison && !returning ? MissionType.LiaisonAtLocation : MissionType.TravelToLocation, returning ? MapLocation.HomeBase : city.Location));
        }
        public List<GameEvent> IncrementTime() //called internally
        {
            if (!Settings.RealTime)
                Settings.GameTime = Settings.GameTime.AddHours(6);//4x/day for now

            //add random things
            List<GameEvent> randoms = GenerateRandomEvents();
            Events.AddRange(randoms);

            //process
            List<GameEvent> events = [];
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
                switch (ev.Type)
                {
                    case MissionType.TravelToLocation:
                        displays.Add(ev.ResolveReturn());
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
                            Events.Add(new(contract, Settings.PayDay(contract),true));
                        break;
                    case MissionType.Mercenary:
                        var merc = ev.ResolveMercenary();
                        //remove? or only if !failed?... for now only 1 try
                        //could allow retries on certain missions, do that logic here
                        merc.DisplayMission!.Client.FinishMission(merc);
                        merc.DisplayTeam!.MissionID = Guid.Empty;
                        displays.Add(merc);
                        Events.Add(new GameEvent(merc.DisplayTeam, Settings.MissionTravelTime(merc.DisplayMission!), MissionType.TravelToLocation, merc.DisplayTeam!.Location ?? MapLocation.HomeBase));
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
                    default:
                        displays.Add(new("Undefined game event", Settings.CurrentTime));
                        break;
                }
            }
            return displays;
        }
        private List<GameEvent> GenerateRandomEvents()
        {
            Random r = new Random();
            List<GameEvent> randoms = [];
            if (Settings.BanditAttackOdds())//1 in 20, way too high for real
            {
                randoms.Add(new GameEvent(MissionType.BanditAttack, Settings.BanditTime(), true));
            }
            return randoms;
        }
    }
}