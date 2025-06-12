using System.ComponentModel.DataAnnotations;
using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class ClientCity
    {
        [Key] public Guid ID { get; set; } = Guid.NewGuid();
        public int Distance { get; set; }
        public string CityName { get; set; }
        public int CityLevel { get; set; }
        public ClientCity(int lvl)
        {
            Random r = new();
            Distance = 5 + r.Next(20);
            CityLevel = lvl;
            CityName = Names.CityName(lvl);
        }
        public List<Team> Liaisons { get; set; } = [];
        public int TotalCharisma { get { return Liaisons.SelectMany(e => e.Leaders).Sum(e => (int)(e.Charisma * e.LeadershipXP)); } }
        public int TotalLogistics { get { return Liaisons.SelectMany(e => e.Leaders).Sum(e => (int)(e.Logistics * e.LeadershipXP)); } }
        public int TotalTactics { get { return Liaisons.SelectMany(e => e.Leaders).Sum(e => (int)(e.Tactics * e.LeadershipXP)); } }
        public List<Mission> Missions { get; set; } = [];
        public void NewMissionCycle()
        {
            //to do: probability gives # of new missions
            Random r = new();
            int nmissions = r.Next(5);
            for (int i = 0; i < nmissions; i++)
            {
                Mission newmission = new(1000 * CityLevel + r.Next(500000 * CityLevel), false, true);
                newmission.MoneyReward = (int)((newmission.MoneyReward ?? 0) * (25.0 + TotalCharisma / 25.0));
                newmission.MissionDays = (int)Math.Ceiling(newmission.MissionDays * (25.0 / (25.0 + TotalLogistics)));
                newmission.SetDisp(TotalTactics);
                Missions.Add(newmission);
            }
        }
    }
}