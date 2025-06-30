using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Models
{
    public class TeamResult
    {
        [Key] public Guid ID { get; set; }
        public bool Success { get; set; }

        public Dictionary<Soldier, int> HPDamage { get; set; } = [];
        public List<(Guid, int, bool)> Gains { get; set; } = [];
        public TeamResult(Team team, List<(Guid, int, bool)> gains, bool success, int diff)
        {
            Success = success;
            Gains = gains;
            Random r = new();
            //do math
            List<Soldier> solds = team.GetAllSoldiers.ToList();
            HPDamage = solds.ToDictionary(e => e, e => 0);
            int totdmg = team.PScore - diff;
            if (totdmg < 0)
            {
                for (int i = 0; i < r.Next(5); i++)
                    HPDamage[solds[r.Next(solds.Count)]]++;
            }
            else
            {
                //this is V1 of this algorithm, it probably sucks but we can't have a dime holding up a dollar
                while (totdmg > 0)
                {
                    Soldier s = solds[r.Next(solds.Count)];
                    HPDamage[s]++;
                    totdmg -= s.PowerLevel;
                }
            }
            foreach (KeyValuePair<Soldier, int> dmg in HPDamage)
            {
                dmg.Key.Hurt(dmg.Value);
            }
            team.Cleanup();
        }
    }
}