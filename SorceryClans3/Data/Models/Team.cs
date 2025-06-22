using System.ComponentModel.DataAnnotations.Schema;

namespace SorceryClans3.Data.Models
{
    public class Team
    {
        public Guid ID { get; set; }
        public string TeamName { get; set; }
        public Guid? MissionID { get; set; }
        public MapLocation? Location { get; set; } = null;
        public Resources Resources { get; set; } = new();
        public bool IsAtHome { get { return Location == null || (Location.X == 0 && Location.Y == 0); } }
        public List<Soldier> Soldiers { get; set; }
        public List<Soldier> Leaders { get; set; }
        public int SoldierCount { get { return Soldiers.Count + Leaders.Count; } }
        [NotMapped] public bool View { get; set; }
        public void AddSoldier(Soldier s)
        {
            foreach (Soldier sold in Soldiers)
            {
                if (!s.Teamwork.ContainsKey(sold.ID))
                    s.Teamwork.Add(sold.ID,0);
                if (!sold.Teamwork.ContainsKey(s.ID))
                    sold.Teamwork.Add(s.ID,0);
            }
            Soldiers.Add(s);
        }
        public void RemoveSoldier(Soldier s)
        {
            Soldier? sold = Leaders.FirstOrDefault(e => e.ID == s.ID);
            if (sold != null)
            {
                Leaders.Remove(sold);
                foreach (Soldier s2 in sold.SubSoldiers)
                {
                    RemoveSoldier(s2);
                }
                return;
            }
            sold = Soldiers.FirstOrDefault(e => e.ID == s.ID);
            if (sold != null)
            {
                Soldiers.Remove(sold);
                foreach (Soldier s2 in sold.SubSoldiers)
                {
                    RemoveSoldier(s2);
                }
            }
        }
        public List<Soldier> GetAllSoldiers
        {
            get
            {
                return [.. Leaders, .. Soldiers];
            }
        }
        public List<MagicColor> GetColors
        {
            get
            {
                List<MagicColor> colors = new List<MagicColor>();
                foreach (Soldier s in Soldiers)
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
                IList<Soldier> solds = Soldiers.OrderBy(e => e.Combat).ToList();
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
                IList<Soldier> solds = Soldiers.OrderBy(e => e.Combat).ToList();
                for (int i = 0; i < solds.Count; i++) //fancy for diminished returns to add
                {
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
                IList<Soldier> solds = Soldiers.OrderBy(e => e.Combat).ToList();
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
                IList<Soldier> solds = Soldiers.OrderBy(e => e.Combat).ToList();
                for (int i = 0; i < solds.Count; i++) //fancy for diminished returns to add
                {
                    score += solds[i].HealScore;
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
                int min = 999;
                int boost = 0;
                foreach (Soldier sold in GetAllSoldiers)
                {
                    if (sold.Travel != null && sold.Travel.Value < min)
                        min = sold.Travel.Value;
                    boost += sold.TravelGroupBoost;
                }
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
                foreach (Soldier sold in Soldiers)
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
                foreach (Soldier sold in Soldiers)
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
                foreach (Soldier sold in Soldiers)
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
                foreach (Soldier s in Soldiers)
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
                foreach (Soldier s in Soldiers)
                {
                    foreach (Soldier s2 in Soldiers)
                    {
                        if (s.ID != s2.ID)
                        {
                            double val = 0;
                            s.Teamwork.TryGetValue(s2.ID, out val);
                            ret += 1 + val;
                        }
                    }
                }
                return ret/(Soldiers.Count* (Soldiers.Count-1));
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
            foreach (Soldier s in Soldiers)
            {
                foreach (Soldier s2 in Soldiers)
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
			return (int)(MScore * total / Leaders.Count);
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
    }
}