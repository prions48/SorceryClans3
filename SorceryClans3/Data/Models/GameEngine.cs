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

        public void StartMission(Mission mission, Team team)
        {
            mission.AttemptingTeam = team;
            team.MissionID = mission.ID;
            Events.Add(new(mission, Settings.MissionEndTime(mission)));
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
                    case MissionType.Mercenary:
                        var merc = ev.ResolveMercenary();
                        Missions.Remove(merc.DisplayMission!);
                        merc.DisplayTeam!.MissionID = Guid.Empty;
                        displays.Add(merc);
                        Events.Add(new GameEvent(merc.DisplayTeam, Settings.MissionTravelTime(merc.DisplayMission!), MissionType.TravelToLocation, merc.DisplayTeam!.Location ?? MapLocation.HomeBase));
                        break;
                    case MissionType.BanditAttack:
                        displays.Add(new("BANDITS HAVE ATTACKED!", Settings.CurrentTime));
                        break;
                    default: displays.Add(new("Undefined game event", Settings.CurrentTime));
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
                randoms.Add(new GameEvent(MissionType.BanditAttack, Settings.BanditTime()));
            }
            return randoms;
        }
    }
}