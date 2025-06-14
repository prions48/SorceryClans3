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
        public ClientCity(int lvl)
        {
            Random r = new();
            Location = new(20, 5); //do cooler things with this later
            CityLevel = lvl;
            CityName = Names.CityName(lvl);
        }
        public List<Team> Liaisons { get; set; } = [];
        public List<Team> Teams { get; set; } = [];
        public int TotalCharisma  { get { return Liaisons.Where(e => e.MissionID == this.ID).SelectMany(e => e.Leaders).Sum(e => (int)(e.Charisma * e.LeadershipXP)); } }
        public int TotalLogistics { get { return Liaisons.Where(e => e.MissionID == this.ID).SelectMany(e => e.Leaders).Sum(e => (int)(e.Logistics * e.LeadershipXP)); } }
        public int TotalTactics   { get { return Liaisons.Where(e => e.MissionID == this.ID).SelectMany(e => e.Leaders).Sum(e => (int)(e.Tactics * e.LeadershipXP)); } }
        public List<Mission> Missions { get; set; } = [];
        public void NewMissionCycle()
        {
            //to do: probability gives # of new missions
            Random r = new();
            int nmissions = (int)(r.NextDouble() * 5 * (25.0 + TotalLogistics) / 25 );
            for (int i = 0; i < nmissions; i++)
            {
                Mission newmission = new(1000 * (CityLevel + 1) + r.Next(50000 * CityLevel), this, false, true);
                newmission.MoneyReward = (int)((newmission.MoneyReward ?? 0) * (25.0 + TotalCharisma / 25.0));
                newmission.SetDisp(TotalTactics);
                Missions.Add(newmission);
            }
        }
    }
}