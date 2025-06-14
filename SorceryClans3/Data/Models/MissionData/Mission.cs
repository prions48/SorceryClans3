namespace SorceryClans3.Data.Models
{
    public class Mission
    {
        public Guid ID { get; set; }
        private int Seed { get; set; }
        public Team? AttemptingTeam { get; set; }
        public MissionType Type { get; set; } = MissionType.Mercenary;
        public ClientCity Client { get; set; }
        public MapLocation Location { get; set; }
        public IList<string> Rewards
        {
            get
            {
                IList<string> ret = new List<string>();
                if (MoneyReward != null)
                    ret.Add(MoneyReward.Value.ToString("C0"));
                if (ArtifactReward != null)
                    ret.Add(ArtifactReward.ArtifactName);
                return ret;
            }
        }
        public int? MoneyReward { get; set; }
        public Artifact? ArtifactReward { get; set; }
        public int MissionDays { get; set; }
        public int TravelDistance
        {
            get
            {
                return (int)Math.Ceiling(Location.GetDistance(Client.Location));
            }
        }
        public int? CScore { get; set; }
        public int? MScore { get; set; }
        public int? SScore { get; set; }
        public int? KScore { get; set; }
        public int? CDisp { get; set; } = null;
        public int? MDisp { get; set; } = null;
        public int? SDisp { get; set; } = null;
        public int? KDisp { get; set; } = null;
        public IList<MagicColor> ColorReqs = new List<MagicColor>();
        public int CPenalty { get; set; }
        public int MPenalty { get; set; }
        public int SPenalty { get; set; }
        public int KPenalty { get; set; }
        Random r = new Random();
        public Mission()
        {
            ID = Guid.NewGuid();
            Seed = 100;
            CScore = 100;
            MScore = 100;
            SScore = 100;
            KScore = null;
            SetColor(false);
            SetDisp();
            SetTime();
        }
        public Mission(int seed, ClientCity client, bool forceall = false, bool nocolor = false)
        {
            Client = client;
            Location = new(client);
            ID = Guid.NewGuid();
            Seed = seed;
            SetScore(forceall);
            SetColor(nocolor);
            SetDisp();
            SetTime();
        }

        private void SetScore(bool forceall)
        {
            while (CScore == null && MScore == null && SScore == null && KScore == null)
            {
                CScore = r.NextDouble() < .3 && !forceall ? null : r.Next(Seed, 2 * Seed);
                MScore = r.NextDouble() < .4 && !forceall ? null : r.Next(Seed, (int)(1.5 * Seed));
                SScore = r.NextDouble() < .5 && !forceall ? null : r.Next(Seed/10, Seed/2);
                int heal = (int)(Math.Log10(Seed+1) * Math.Log10(Seed+1) * 25) + r.Next(100);
                KScore = r.NextDouble() < .95 || forceall ? null : r.Next(heal, 2 * heal);
            }
            MoneyReward = (CScore == null ? 0 : r.Next(CScore.Value/10, CScore.Value/5)) +
                     (MScore == null ? 0 : r.Next(MScore.Value/10, MScore.Value/5)) +
                     (SScore == null ? 0 : r.Next(SScore.Value/4, SScore.Value/2)) +
                     (KScore == null ? 0 : r.Next(KScore.Value*100, KScore.Value*200));
        }
        private void SetColor(bool nocolor)
        {
            //start with just one for now
            if (Seed < 100000 || r.NextDouble() > .2 || nocolor)
                return;
            int numcolors = r.Next(2)+1;
            for (int i = 0; i < numcolors; i++)
            {
                MagicColor color = (MagicColor)(r.Next(6)+1);
                int tally = r.Next(1,5);
                for (int j = 0; j < tally; j++)
                    ColorReqs.Add(color);
            }
            CPenalty = CScore == null ? 0 : (int)(CScore * (r.NextDouble() * .1 + .1)); 
            MPenalty = MScore == null ? 0 : (int)(MScore * (r.NextDouble() * .3 + .3));
            SPenalty = SScore == null ? 0 : (int)(SScore * (r.NextDouble() * .2 + .1));
            KPenalty = KScore == null ? 0 : (int)(KScore * (r.NextDouble() * .2 + .2));
        }
        public void SetDisp(int accfactor = 0)
        {
            double pfac = 1.0 + (5.0 / (25.0 + accfactor));
            double nfac = 1.0 - (5.0 / (25.0 + accfactor));
            if (CScore != null)
            {
                CDisp = r.Next((int)(CScore * nfac), (int)(CScore * pfac));
                CDisp = (CDisp / 10) * 10;
            }
            if (MScore != null)
            {
                MDisp = r.Next((int)(MScore * nfac),(int)(MScore * pfac));
                MDisp = (MDisp / 10) * 10;
            }
            if (SScore != null)
            {
                SDisp = r.Next((int)(SScore * nfac),(int)(SScore * pfac));
                SDisp = (SDisp / 10) * 10;
            }
            if (KScore != null)
            {
                KDisp = r.Next((int)(KScore * nfac),(int)(KScore * pfac));
                KDisp = (KDisp / 10) * 10;
            }
        }
        private void SetTime()
        {
            MissionDays = r.Next(1,6) + (int)(Seed <= 1 ? 0 : Math.Log(Seed));
        }
        public (bool,int) CompleteMission()
        {
            if (AttemptingTeam == null)
                throw new Exception("No completing team identified for mission");
            Team team = AttemptingTeam;
            bool passcolor = true;
            int failby = 0;
            double failbypct = 0;

            if (ColorReqs.Count > 0)
            {
                IList<MagicColor> teamcolors = team.GetColors;
                foreach (MagicColor color in ColorReqs.Distinct())
                {
                    IList<MagicColor> reqs = ColorReqs.Where(e => e == color).ToList();
                    IList<MagicColor> tcolors = teamcolors.Where(e => e == color).ToList();
                    passcolor = passcolor && tcolors.Count >= reqs.Count;     
                    if (tcolors.Count < reqs.Count)
                    {
                        failby = reqs.Count - tcolors.Count;
                        failbypct += failby * 1.0 / reqs.Count;
                    }
                }
            }
            bool success = PassStat(CScore, team.CScore, passcolor, CPenalty) &&
                   PassStat(MScore, team.MScore, passcolor, MPenalty) &&
                   PassStat(SScore, team.SScore, passcolor, SPenalty) &&
                   PassStat(KScore, team.KScore, passcolor, KPenalty) && (passcolor || PctSuccessOnColorFail(failby, failbypct));//if color inadequate, tops out at 50% pass rate
            int diff = 0;
            int cdiff = DiffSuccess(CScore,team.CScore,passcolor,CPenalty);
            int mdiff = DiffSuccess(MScore,team.MScore,passcolor,MPenalty);
            int sdiff = DiffSuccess(SScore,team.SScore,passcolor,SPenalty);
            int kdiff = DiffSuccess(KScore,team.KScore,passcolor,KPenalty);
            if (success)
                diff = cdiff + mdiff + sdiff + kdiff;
            else
            diff = (cdiff > 0 ? 0 : cdiff) + (mdiff > 0 ? 0 : mdiff) + 
                   (sdiff > 0 ? 0 : sdiff) + (kdiff > 0 ? 0 : kdiff);
            return (success, diff);
        }
        private bool PctSuccessOnColorFail(int failby, double failbypct)
        {
            if (r.NextDouble() > failbypct * 1.5) //autofail if deficient by 67% (needed 3, had 1)
                return false; //75% chance of failure if needed 2 had 1
            //also ensures autofailure if missing any colors completely (adds 100%)
            return failby < r.Next(10);
            //10% per missing color failure up to 100%
        }
        private bool PassStat(int? mscore, int tscore, bool pass, int penalty)
        {
            if (mscore == null)
                return true;
            if (!pass)
                return mscore.Value <= tscore + penalty;
            return mscore.Value <= tscore || PercentSuccess(mscore.Value, tscore);
        }
        private int DiffSuccess(int? mscore, int tscore, bool pass, int penalty)
        {
            if (mscore == null)
                return 0;
            if (!pass)
                return tscore - (mscore.Value + penalty);
            return tscore - mscore.Value;
        }
        private bool PercentSuccess(int m, int t)
        {
            return (m - t) * 1.0 / m < (r.NextDouble() * .1);
        }
    }
}