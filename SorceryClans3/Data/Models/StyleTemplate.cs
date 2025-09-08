using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class StyleTemplate
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid? ClanID { get; set; }
        public string StyleName { get; set; }
        public StatBlock MinReqs { get; set; }
        public StatBlock MaxReqs { get; set; }
        public IList<StyleRank> Ranks { get; set; }
        private StyleReqPower ReqPower { get; set; }
        private StyleGivePower GivePower { get; set; }
        public PowerTemplate? Power { get; set; }
        public bool SoldierEligible(Soldier s)
        {
            if (s.Power != null && ReqPower == StyleReqPower.NoAcceptsPower)
                return false;
            if (MinReqs.IsBelow(s))
                return false;
            if (MaxReqs.IsAbove(s))
                return false;
            return true;
        }
        private static Random r = new Random();
        ///<summary>
        ///lvl best behaved 1-3 but can run to like 10
        ///</summary>
        public StyleTemplate(int lvl)
        {
            //half ass it a little bit for now
            //if (r.NextDouble()*lvl < 1)
                ReqPower = StyleReqPower.AcceptsPower;
            //else
                ReqPower = StyleReqPower.NoAcceptsPower;
            if (r.NextDouble()*lvl > 1.5 && ReqPower == StyleReqPower.NoAcceptsPower)
                GivePower = StyleGivePower.GivePower;
            else
                GivePower = StyleGivePower.NoGivePower;

            int nranks = lvl/2 + 3 + r.Next(lvl+1);
            if (nranks < 4)
                nranks = 4;
            if (nranks > 10)
                nranks = 10;
            Ranks = new List<StyleRank>();
            for (int i = 0; i < nranks; i++)
            {
                Ranks.Add(new StyleRank(ID));
            }
            List<int> rankids = [r.Next(0,4), r.Next(10,14)];
            while (rankids.Count < nranks)
            {
                int rank = r.Next(14);
                while (rankids.Contains(rank))
                    rank = r.Next(14);
                rankids.Add(rank);
            }
            rankids = rankids.Order().ToList();
            for (int i = 0; i < Ranks.Count; i++)
            {
                Ranks[i].Name = Names.RankName(rankids[i]);
                Ranks[i].StyleXP =  i == 0 ? 0 : ((i-1) * 90 / (Ranks.Count-2)) + 10 + (i == Ranks.Count - 1 ? 0 : 3 - r.Next(7));
            }
            SkillStat prim = (SkillStat)(r.NextDouble()*3.3);
            SkillStat sec = (SkillStat)(r.NextDouble()*3.3);
            SkillStat tert = (SkillStat)r.Next(3);
            StyleName = Names.StyleName(prim,sec,lvl, null);
            int? cmin = null, mmin = null, smin = null, kmin = null, cmax = null, mmax = null, smax = null, kmax = null;
            switch (prim)
            {
                case SkillStat.Combat: cmin = 2 + r.Next(lvl+3); break;
                case SkillStat.Magic: mmin = 2 + r.Next(lvl+3); break;
                case SkillStat.Subtlety: smin = 2 + r.Next(lvl+3); break;
                case SkillStat.Heal: kmin = 2 + r.Next(lvl+3); break;
            }
            switch (sec)
            {
                case SkillStat.Combat: cmin = cmin == null ? r.Next(lvl) + 2 : cmin + r.Next(lvl); break;
                case SkillStat.Magic: mmin = mmin == null ? r.Next(lvl) + 2 : mmin + r.Next(lvl); break;
                case SkillStat.Subtlety: smin = smin == null ? r.Next(lvl) + 2 : smin + r.Next(lvl); break;
                case SkillStat.Heal: kmin = cmin == null ? r.Next(lvl) + 2 : kmin + r.Next(lvl); break;
            }
            switch (tert)
            {
                case SkillStat.Combat: cmin = cmin == null ? r.Next(lvl) + 2 : cmin + r.Next(lvl); break;
                case SkillStat.Magic: mmin = mmin == null ? r.Next(lvl) + 2 : mmin + r.Next(lvl); break;
                case SkillStat.Subtlety: smin = smin == null ? r.Next(lvl) + 2 : smin + r.Next(lvl); break;
                case SkillStat.Heal: kmin = kmin == null ? r.Next(lvl) + 2 : kmin + r.Next(lvl); break;
            }
            if (cmin != null) cmax = cmin + lvl + 2 + r.Next(lvl+2);
            if (mmin != null) mmax = mmin + lvl + 2 + r.Next(lvl+2);
            if (smin != null) smax = smin + lvl + 2 + r.Next(lvl+2);
            if (kmin != null) kmax = kmin + lvl + 2 + r.Next(lvl+2);
            int pmin = r.Next(100*lvl)+500;
            MinReqs = new StatBlock(cmin,mmin,smin,null,kmin,pmin,null,null,null,null);
            MaxReqs = new StatBlock(cmax,mmax,smax,null,kmax,pmin+r.Next(100*lvl)+500,null,null,null,null);
            int numpts = lvl + 1 + r.Next(lvl/2) + (GivePower == StyleGivePower.NoGivePower ? lvl/2+3 : 0);
            if (numpts < Ranks.Count)
                numpts = Ranks.Count;
            for (int i = 1; i <= numpts; i++)
            {
                int ctr = i;
                if (ctr >= Ranks.Count)
                    ctr = r.Next(1, Ranks.Count);
                switch (SelectStat(prim,sec,tert))
                {
                    case SkillStat.Combat: Ranks[ctr].CBonus++; break;
                    case SkillStat.Magic: Ranks[ctr].MBonus++; break;
                    case SkillStat.Subtlety: Ranks[ctr].SBonus++; break;
                    case SkillStat.Heal: Ranks[ctr].KBonus++; break;
                }
            }
            RankTeach tlevel = RankTeach.Teach;
            for (int i = Ranks.Count - 1; i >= Ranks.Count / 3; i--)
            {
                Ranks[i].Teach = tlevel;
                if (i <= Ranks.Count * 4 / 5 && r.NextDouble() < .5)
                    tlevel = RankTeach.AssistTeach;
                else if (i <= Ranks.Count / 2 && r.NextDouble() < .3)
                    tlevel = RankTeach.NoTeach;
            }
            if (GivePower == StyleGivePower.GivePower)
            {
                BoostStat bsprim = (BoostStat)prim;
                if (bsprim == BoostStat.HP)
                    bsprim = BoostStat.Heal;
                Power = new PowerTemplate(ID, lvl <= 3 ? lvl : 3, false, bsprim);
                StyleName = Names.StyleName(prim,sec,lvl, Power.Color);
                Power.Heritability = null;
                string[] words = StyleName.Split(" ");
                Power.PowerName = words[words.Length-2] + " " + words[words.Length-1] + " " + Names.StylePower();
                for (int i = Ranks.Count/2; i < Ranks.Count; i++)
                {
                    if (i == 0)
                        continue;
                    Ranks[i].GivePower = true;
                }
            }
        }
        public Style CreateStyle(Soldier sold)
        {
            return new Style(this, sold);
        }
        private static SkillStat SelectStat(SkillStat stat1, SkillStat stat2, SkillStat stat3)
        {
            if (r.NextDouble() < .6)
                return stat1;
            if (r.NextDouble() < .7)
                return stat2;
            return stat3;
        }
        public int CTotal { get { return Ranks.Select(e => e.CBonus).Sum(); } }
        public int MTotal { get { return Ranks.Select(e => e.MBonus).Sum(); } }
        public int STotal { get { return Ranks.Select(e => e.SBonus).Sum(); } }
        public int KTotal { get { return Ranks.Select(e => e.KBonus).Sum(); } }
    }
    
}