using System.ComponentModel.DataAnnotations;
using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class ClientCity
    {
        [Key] public Guid ID { get; set; } = Guid.NewGuid();
        public MapLocation Location { get; set; }
        public int Distance { get { return (int)Math.Ceiling(Location.GetDistance()); } }
        public string CityName { get; set; }
        public int CityLevel { get; set; }
        public int RepScore { get; set; } = 0;
        public int UnseenMissions { get; set; } = 0;
        private DateTime NextRefresh { get; set; } = DateTime.MinValue;
        public ClientReputation Reputation
        {
            get
            {
                if (RepScore < -1000) return ClientReputation.Hostile;
                if (RepScore < 0) return ClientReputation.Hostile;
                if (RepScore < 1000) return ClientReputation.Hostile;
                if (RepScore < 10000) return ClientReputation.Hostile;
                return ClientReputation.Trust;
            }
        }
        public ClientCity(int lvl)
        {
            CityName = Names.CityName(lvl);
            Location = new(20, 5) { LocationName = CityName }; //do cooler things with this later
            CityLevel = lvl;
        }
        public List<Team> Liaisons { get; set; } = [];
        public List<Team> Teams { get; set; } = [];
        public List<Team> AllTeams {get { return Liaisons.Concat(Teams).ToList(); } }
        public int TotalCharisma { get { return Liaisons.Where(e => e.MissionID == this.ID).SelectMany(e => e.Leaders).Sum(e => (int)(e.Charisma * e.LeadershipXP)); } }
        public int TotalLogistics { get { return Liaisons.Where(e => e.MissionID == this.ID).SelectMany(e => e.Leaders).Sum(e => (int)(e.Logistics * e.LeadershipXP)); } }
        public int TotalTactics { get { return Liaisons.Where(e => e.MissionID == this.ID).SelectMany(e => e.Leaders).Sum(e => (int)(e.Tactics * e.LeadershipXP)); } }
        public List<Mission> Missions { get; set; } = [];
        public List<GameEventDisplay> NewMissionCycle(GameSettings settings)
        {
            List<GameEventDisplay> ret = [];
            int x = 0;
            while (x < Missions.Count)
            {
                if (Missions[x].AttemptingTeam != null || settings.CurrentTime < Missions[x].ExpirationDate)
                    x++;
                else
                {
                    if (Missions[x].Importance != ClientImportance.Normal)
                    {
                        RepScore -= Missions[x].ReputationBoost();//more things here maybe?
                        ret.Add(new($"An important mission for {CityName} has been neglected!", settings.CurrentTime));
                    }
                    Missions.RemoveAt(x);
                }
            }
            if (settings.CurrentTime < NextRefresh || Liaisons.Count == 0)
            {
                if (UnseenMissions > 0 && Liaisons.Any(e => e.MissionID == null))//liaison comes active and refresh hasn't hit yet but hit while liaison team was out
                {
                    ret.Add(new($"New mission{(UnseenMissions == 1 ? "" : "s")} ready for {CityName}!", settings.CurrentTime));
                    UnseenMissions = 0;
                }
                return ret;
            }
            //to do: probability gives # of new missions
            Random r = new();
            int nmissions = 1 + (int)(r.NextDouble() * 5 * (15.0 + TotalLogistics) / 15);
            for (int i = 0; i < nmissions && Missions.Count <= settings.MaxMissions(CityLevel); i++)
            {
                Mission newmission = new(settings, 1000 * (CityLevel + 1) + r.Next(50000 * CityLevel), this, false, true, Liaisons.Count > 0 && Reputation >= ClientReputation.Neutral);
                newmission.MoneyReward = (int)((newmission.MoneyReward ?? 0) * (25.0 + TotalCharisma / 25.0));
                newmission.SetDisp(TotalTactics);
                Missions.Add(newmission);
            }
            //now that it's triggered, set the next one
            if (settings.RealTime)
                NextRefresh = settings.CurrentTime.AddMinutes(30 + r.Next(10));
            else
                NextRefresh = settings.CurrentTime.AddDays(10 + r.Next(5));
            if (nmissions > 0 || UnseenMissions > 0)//now this only triggers if a liaison comes active the same day as a refresh
            {
                if (Liaisons.Any(e => e.MissionID == null))
                {
                    ret.Add(new($"New mission{(nmissions == 1 ? "" : "s")} ready for {CityName}!", settings.CurrentTime));
                    UnseenMissions = 0;
                }
                else
                    UnseenMissions += nmissions;
            }
            return ret;
        }
    }
    public enum ClientReputation
    {
        Hostile = 0,
        Poor = 1,
        Neutral = 2,
        Warm = 3,
        Friendly = 4,
        Trust = 5
    }
    public enum ClientImportance
    {
        Normal,
        Important,
        Critical
    }
}