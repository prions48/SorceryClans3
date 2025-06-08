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

        public void StartMission(Mission mission, Team team)
        {
            mission.AttemptingTeam = team;
            team.MissionID = mission.ID;
            if (Settings.RealTime)
            {
                Events.Add(new(mission, Settings.CurrentTime.AddMinutes((mission.TravelDistance / team.DScore) + mission.MissionDays)));
            }
            else
            {
                Events.Add(new(mission, Settings.GameTime.AddDays((mission.TravelDistance / team.DScore) + mission.MissionDays)));
            }
        }
        public List<GameEvent> IncrementTime() //called internally
        {
            if (!Settings.RealTime)
                Settings.GameTime = Settings.GameTime.AddDays(1);

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
                    case MissionType.TravelHome:
                        displays.Add(ev.ResolveReturn());
                        break;
                    case MissionType.Mercenary:
                        var merc = ev.ResolveMercenary();
                        Missions.Remove(merc.DisplayMission!);
                        merc.DisplayTeam!.MissionID = Guid.Empty;
                        displays.Add(merc);
                        if (Settings.RealTime)
                        {
                            Events.Add(new GameEvent(merc.DisplayTeam, Settings.CurrentTime.AddMinutes(merc.DisplayMission!.TravelDistance / merc.DisplayTeam.DScore)));
                        }
                        else
                        {
                            Events.Add(new GameEvent(merc.DisplayTeam, Settings.CurrentTime.AddDays(merc.DisplayMission!.TravelDistance / merc.DisplayTeam.DScore)));
                        }
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
            if ((Settings.RealTime && r.NextDouble() < 0.05) || (!Settings.RealTime && r.NextDouble() < .05))//1 in 20, way too high for real
            {
                if (Settings.RealTime)
                {
                    randoms.Add(new GameEvent(MissionType.BanditAttack, Settings.CurrentTime.AddMinutes(r.Next(2)+1)));
                }
                else
                {
                    randoms.Add(new GameEvent(MissionType.BanditAttack, Settings.CurrentTime.AddDays(r.Next(5) + 10)));
                }
            }
            return randoms;
        }
    }
}