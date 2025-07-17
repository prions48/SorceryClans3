using System.ComponentModel.DataAnnotations.Schema;
using MudBlazor.Extensions;

namespace SorceryClans3.Data.Models
{
    public class Team
    {
        public Guid ID { get; set; }
        public string TeamName { get; set; }
        public Guid? MissionID { get; set; }
        public MapLocation? Location { get; set; } = null;
        public MapLocation BaseLocation { get { return Location ?? MapLocation.HomeBase; } }
        public Resources Resources { get; set; } = new();
        public bool IsAtHome { get { return Location == null || (Location.X == 0 && Location.Y == 0); } }
        public List<Soldier> Soldiers { get; set; }
        public List<Soldier> Leaders { get; set; }
        public int SoldierCount { get { return GetAllSoldiers.Count; } }
        [NotMapped] public bool View { get; set; }
        public void AddSoldier(Soldier s)
        {
            foreach (Soldier sold in GetAllSoldiers)
            {
                if (!s.Teamwork.ContainsKey(sold.ID))
                    s.Teamwork.Add(sold.ID, 0);
                if (!sold.Teamwork.ContainsKey(s.ID))
                    sold.Teamwork.Add(s.ID, 0);
            }
            Soldiers.Add(s);
            s.Team = this;
        }
        public void RemoveSoldier(Soldier s)
        {
            if (Leaders.Contains(s))
            {
                Leaders.Remove(s);
                s.Team = null;
                return;
            }
            if (Soldiers.Contains(s))
            {
                Soldiers.Remove(s);
                s.Team = null;
            }
        }
        public bool EligibleToHunt(Beast? beast = null)
        {
            if (beast == null)
                return GetAllSoldiers.Any(e => e.GetColors.Contains(MagicColor.Green));
            return GetAllSoldiers.Any(e => beast.IsEligible(e));
        }
        public List<Soldier> GetSoldiers
        {
            get
            {
                List<Soldier> allsoldiers = [];
                foreach (Soldier s in Leaders)
                    allsoldiers.AddRange(s.SubSoldiers);
                foreach (Soldier sold in Soldiers)
                {
                    allsoldiers.Add(sold);
                    allsoldiers.AddRange(sold.SubSoldiers);
                }
                return allsoldiers;
            }
        }
        public List<Soldier> GetAllSoldiers
        {
            get
            {
                return [..Leaders,..GetSoldiers];
            }
        }
        public List<MagicColor> GetColors
        {
            get
            {
                List<MagicColor> colors = new List<MagicColor>();
                foreach (Soldier s in GetAllSoldiers)
                {
                    colors = colors.Concat(s.GetColors).ToList();
                }
                return colors;
            }
        }
        public int PScore //for dmg purposes
        {
            get
            {
                return GetAllSoldiers.Sum(e => e.PowerLevel);
            }
        }
        public int CScore
        {
            get
            {
                int score = 0;
                IList<Soldier> solds = GetSoldiers.OrderBy(e => e.Combat).ToList();
                for (int i = 0; i < solds.Count; i++) //fancy for diminished returns to add
                {
                    score += solds[i].Combat * solds[i].PowerLevel;
                }
                return (int)(score * Teamwork * ComTeamSizeFactor);
            }
        }
        public int MScore
        {
            get
            {
                int score = 0;
                IList<Soldier> solds = GetSoldiers.OrderBy(e => e.Magic).ToList();
                for (int i = 0; i < solds.Count; i++) //fancy for diminished returns to add
                {
                    score += solds[i].Magic * solds[i].PowerLevel;
                }
                return (int)(score * Teamwork * MagTeamSizeFactor);
            }
        }
        public int MSpellScore
        {
            get
            {
                int score = 0;
                IList<Soldier> solds = GetSoldiers.OrderBy(e => e.Magic).ToList();
                for (int i = 0; i < solds.Count; i++) //fancy for diminished returns to add
                {
                    if (solds[i].IsCaster)
                        score += solds[i].Magic * solds[i].PowerLevel;
                }
                return (int)(score * Teamwork * MagTeamSizeFactor);
            }
        }
        public int SScore
        {
            get
            {
                int score = 0;
                IList<Soldier> solds = GetSoldiers.OrderBy(e => e.Subtlety).ToList();
                for (int i = 0; i < solds.Count; i++) //fancy for diminished returns to add
                {
                    score += solds[i].Subtlety * solds[i].PowerLevel;
                }
                if (solds.Count > 0) //average
                    score /= solds.Count;
                double tw = Teamwork;
                return (int)(score * tw * tw * SubTeamSizeFactor); //new addition: teamwork boosts sub more!
            }
        }
        public int KScore
        {
            get
            {
                int score = 0;
                IList<Soldier> solds = GetSoldiers.OrderBy(e => e.HealScore).ToList();
                for (int i = 0; i < solds.Count; i++) //fancy for diminished returns to add
                {
                    score += solds[i].HealScore ?? 0;
                }
                solds = Leaders.OrderBy(e => e.HealScore).ToList();
                for (int i = 0; i < solds.Count; i++)
                {
                    score += (solds[i].HealScore ?? 0) / 2;
                }
                return score; //should teamwork or leadership be involved at all???
            }
        }
        public int DScore
        {
            get
            {
                if (GetAllSoldiers.Count == 0)
                    return 0;
                List<MountCalc> solds = MountCalc.GenerateMountCalcs(GetAllSoldiers);
                int min = solds.Select(e => e.TravelScore).Min();
                int boost = solds.Sum(e => e.TravelBoost);
                if (min < 1)
                    min = 1;
                return min + boost;
            }
        }
        private double ComTeamSizeFactor
        {
            get
            {
                int weight = 0, leadership = 0;
                foreach (Soldier s in Leaders)
                    leadership += s.ComLeadership;
                foreach (Soldier sold in GetSoldiers)
                {
                    weight += sold.TeamWeight;
                    leadership += sold.ComLeadership / 2;
                }
                int factor = weight - leadership - 10;
                //return 8 * Math.Sqrt(count * 0.25) / ((count+2)*(count+2));
                if (factor <= 1)
                {
                    if (leadership <= 0)
                        factor = 2;
                    else
                        return 1;
                }
                return 1.0 / Math.Sqrt(factor);//incorporate leadership here?
            }
        }
        private double MagTeamSizeFactor
        {
            get
            {
                int weight = 0, leadership = 0;
                foreach (Soldier s in Leaders)
                    leadership += s.MagLeadership;
                foreach (Soldier sold in GetSoldiers)
                {
                    weight += sold.TeamWeight;
                    leadership += sold.MagLeadership / 2;
                }
                int factor = weight - leadership - 10;
                if (factor <= 1)
                {
                    if (leadership <= 0)
                        factor = 2;
                    else
                        return 1;
                }
                return 1.0 / Math.Sqrt(factor);
            }
        }
        private double SubTeamSizeFactor
        {
            get
            {
                int weight = 0, leadership = 0;
                foreach (Soldier s in Leaders)
                    leadership += s.SubLeadership;
                foreach (Soldier sold in GetSoldiers)
                {
                    weight += sold.TeamWeight;
                    leadership += sold.SubLeadership / 2;
                }
                int factor = weight - leadership - 10;
                if (factor <= 1)
                {
                    if (leadership <= 0)
                        factor = 2;
                    else
                        return 1;
                }
                return 1.0 / Math.Sqrt(factor);
            }
        }
        public string LeadText
        {
            get
            {
                if (Soldiers.Count == 0)
                    return "";
                int weight = 0, leadership = 0;
                foreach (Soldier s in Leaders)
                {
                    leadership += (s.ComLeadership + s.MagLeadership + s.SubLeadership) / 3;
                }
                foreach (Soldier s in GetSoldiers)
                {
                    weight += s.TeamWeight;
                    leadership += (s.ComLeadership + s.MagLeadership + s.SubLeadership) / 6;
                }
                int factor = weight - leadership - 10;
                if (factor <= 1)
                {
                    if (leadership <= 0)
                        return "Free-Flowing";
                    return "Fully Operational";
                }
                if (factor <= 3)
                    return "Overloaded";
                if (factor <= 7)
                    return "Discombobulated";
                if (factor <= 12)
                    return "Chaotic";
                return "Dysfunctional";
            }
        }
        public double Teamwork
        {
            get
            {
                double ret = 0;
                if (Soldiers.Count <= 1)
                    return 1;
                foreach (Soldier s in GetAllSoldiers)
                {
                    foreach (Soldier s2 in GetAllSoldiers)
                    {
                        if (s.ID != s2.ID)
                        {
                            double val = 0;
                            s.Teamwork.TryGetValue(s2.ID, out val);
                            ret += 1 + val;
                        }
                    }
                }
                return ret / (SoldierCount * (SoldierCount - 1));
            }
        }
        public string TeamworkText
        {
            get
            {
                double tw = Teamwork;
                if (tw <= 0.99)
                    return "Damaged";
                if (tw <= 1.05)
                    return "None";
                if (tw <= 1.3)
                    return "Developing";
                if (tw <= 1.55)
                    return "Good";
                if (tw <= 1.8)
                    return "Excellent";
                if (tw <= 2.05)
                    return "Remarkable";
                if (tw <= 2.3)
                    return "Outstanding";
                return "Superlative";
            }
        }
        public void BoostTeamwork(double twadd)
        {
            foreach (Soldier s in GetAllSoldiers)
            {
                foreach (Soldier s2 in GetAllSoldiers)
                {
                    if (s.ID != s2.ID)
                    {
                        bool twexist = s.Teamwork.TryGetValue(s2.ID, out double tw);
                        if (twexist)
                        {
                            double divfactor = 1.0;
                            if (tw > 0.5)
                                divfactor = 1.5;
                            if (tw > 0.9)
                                divfactor = 3.0;
                            if (tw > 1.2)
                                divfactor = 5.0;
                            s.Teamwork[s2.ID] = tw + (twadd / divfactor);
                            if (s.Teamwork[s2.ID] > 1.5)
                                s.Teamwork[s2.ID] = 1.5;
                        }
                        else
                        {
                            s.Teamwork.Add(s2.ID, twadd);
                            if (s.Teamwork[s2.ID] > 1.5)
                                s.Teamwork[s2.ID] = 1.5;
                        }
                    }
                }
            }
        }
        public Team()
        {
            ID = Guid.NewGuid();
            Soldiers = new List<Soldier>();
            Leaders = new List<Soldier>();
            Random r = new Random();
            if (r.NextSingle() < .2)
                TeamName = "Tiger Squad";
            else if (r.NextSingle() < .25)
                TeamName = "Dragons";
            else if (r.NextSingle() < .333)
                TeamName = "Warriors";
            else if (r.NextSingle() < .5)
                TeamName = "Delta Bravo";
            else
                TeamName = "Brute Squad";
        }
        public void MakeLeader(Guid soldid)
        {
            foreach (Soldier sold in Soldiers)
            {
                if (sold.ID == soldid)
                {
                    sold.IsLeading = true; //you gotta do it
                    Leaders.Add(sold);
                    Soldiers.Remove(sold);
                    break;
                }
            }
        }
        public List<(Guid, int, bool)> BoostSoldiers(int factor)
        {
            Random r = new();
            BoostTeamwork(0.05);
            return GetAllSoldiers.Select(e => e.GainPower(r.Next(factor, factor * 2))).ToList();
        }
        public int ResearchPower(MagicColor color)
        {
            if (Leaders.Count == 0)
                return 0;
            double total = 0;
            foreach (Soldier sold in Leaders)
            {
                if (sold.ResearchSkill.ContainsKey(color))
                {
                    total += sold.ResearchSkill[color];
                }
            }
            if (total < 0)
                return 0;
            return (int)(MSpellScore * total / Leaders.Count);
        }
        public int ResearchPowerIncrement(MagicColor color)
        {
            if (Leaders.Count == 0)
                return 0;
            double total = 0;
            foreach (Soldier sold in Leaders)
            {
                if (sold.ResearchSkill.ContainsKey(color))
                {
                    total += sold.IncrementSkill(color);
                }
            }
            if (total < 0)
                return 0;
            return (int)(MScore * total / Leaders.Count);
        }
        public string ResearchDisplay
        {
            get
            {
                if (Leaders.Count == 0)
                    return "None";
                double total = 0;
                foreach (Soldier sold in Leaders)
                {
                    total += sold.ResearchAffinity;
                }
                total = total / Leaders.Count;
                if (total < .5)
                    return "Terrible";
                if (total < .8)
                    return "Poor";
                if (total < 1.1)
                    return "Good";
                if (total < 1.4)
                    return "Excellent";
                return "Superior";
            }
        }
        public bool LeadershipTraining(Soldier soldier)
        {
            if (!Leaders.Contains(soldier))
                return false;
            soldier.LevelLead();
            int rawlead = soldier.Charisma + soldier.Logistics + soldier.Tactics;
            double totallead = soldier.LeadershipXP * rawlead;
            Random r = new();
            bool fail = rawlead < r.Next(16) + 5 || r.NextDouble() * totallead < 3;
            //success
            foreach (Soldier sold in GetAllSoldiers)
            {
                if (fail)
                {
                    sold.LevelLead(10, 0.02);
                    if (r.Next(3) == 0)
                        sold.Hurt(r.Next(4) + 1);
                }
                else
                {
                    sold.LevelLead(4);
                    sold.TrainLead();
                }
            }
            Cleanup();
            return !fail;
        }
        public bool MedicalTraining(Soldier medic)
        {
            if (!GetAllSoldiers.Contains(medic) || medic.HealScore == null)
                return false;
            medic.Medical!.GainMedicPower();
            Random r = new();
            if (r.Next((int)(medic.HealScore * medic.TeachSkill)) < 200)
                return false;
            foreach (Soldier soldier in GetAllSoldiers)
            {
                if (soldier.Medical == null)
                    soldier.Medical = new();
                if (!soldier.Medical.Assessed)
                {
                    soldier.Medical.Assessed = true;
                }
                else
                {
                    soldier.Medical.GainMedicPower(5);
                }
            }
            return true;
        }
        public void PromoteToLeader(Soldier soldier)
        {
            if (Soldiers.Contains(soldier))
            {
                Soldiers.Remove(soldier);
                Leaders.Add(soldier);
            }
        }
        public void DemoteFromLeader(Soldier soldier)
        {
            if (Leaders.Contains(soldier))
            {
                Leaders.Remove(soldier);
                Soldiers.Add(soldier);
            }
        }
        public void Cleanup()
        {
            int i = 0;
            while (i < Leaders.Count)
            {
                if (Leaders[i].IsAlive)
                {
                    int j = 0;
                    while (j < Leaders[i].SubSoldiers.Count)
                    {
                        if (Leaders[i].SubSoldiers[j].IsAlive)
                            j++;
                        else
                            Leaders[i].SubSoldiers.RemoveAt(j);
                    }
                    i++;
                }
                else
                    Leaders.RemoveAt(i);
            }
            i = 0;
            while (i < Soldiers.Count)
            {
                if (Soldiers[i].IsAlive)
                {
                    int j = 0;
                    while (j < Soldiers[i].SubSoldiers.Count)
                    {
                        if (Soldiers[i].SubSoldiers[j].IsAlive)
                            j++;
                        else
                            Soldiers[i].SubSoldiers.RemoveAt(j);
                    }
                    i++;
                }
                else
                    Soldiers.RemoveAt(i);
            }
        }
        public string TeachDisplay
        {
            get
            {
                if (GetAllSoldiers.Count == 0)
                    return "";
                double avg = GetAllSoldiers.Where(e => e.LeadAssessed).Sum(e => e.TeachSkill) / GetAllSoldiers.Count;
                if (avg < 0.3)
                    return "Terrible";
                if (avg < 0.6)
                    return "Poor";
                if (avg < 1.0)
                    return "Mediocre";
                if (avg < 1.5)
                    return "Excellent";
                return "Legendary";
            }
        }
    }
}