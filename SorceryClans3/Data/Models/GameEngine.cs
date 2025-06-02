namespace SorceryClans3.Data.Models
{
    public class GameEngine
    {
        public GameSettings Settings { get; set; } = new();
        public List<GameEvent> Events { get; set; } = [];
        public List<Mission> Missions { get; set; } = [];
        public List<Clan> Clans { get; set; } = [];
        public List<Soldier> Soldiers { get; set; } = [];
        public List<Team> Teams { get; set; } = [];

        public void StartMission(Mission mission, Team team)
        {
            mission.AttemptingTeam = team;
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
            List<GameEvent> events = [];
            int i = 0;
            while (i < Events.Count)
            {
                if (Events[i].EventCompleted < Settings.CurrentTime)
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
                        displays.Add(ev.ResolveMercenary());
                        break;
                    default: displays.Add(new("Undefined game event"));
                        break;
                }
            }
            return displays;
        }
        
    }
}