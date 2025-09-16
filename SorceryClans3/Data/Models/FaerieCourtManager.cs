namespace SorceryClans3.Data.Models
{
    public class FaerieCourtManager
    {
        public IList<FaerieCourt> Courts { get; set; } = new List<FaerieCourt>();
        public FaerieCourtManager()
        {
            InitCourts();
        }
        public FaerieCourtManager(int numcourts)
        {
            InitCourts(numcourts);
        }
        public void InitCourts(int numcourts = 10)
        {
            if (numcourts < 4)
                numcourts = 4;
            if (numcourts > 60)
                numcourts = 60;
            for (int i = 0; i < 4; i++)
            {
                FaerieCourt court = new FaerieCourt((FaerieSeason)((i % 4)+1), new MapLocation { X = 50 * (i < 2 ? -1 : 1), Y = 50 * (i % 2 == 0 ? -1 : 1)});
                Courts.Add(court);
            }
            for (int i = 4; i < numcourts; i++)
            {
                FaerieCourt court = new FaerieCourt((FaerieSeason)((i % 4)+1), RandomLevel());
                while (Courts.Select(e => e.CourtName).Contains(court.CourtName))
                    court.ShuffleName();
                Courts.Add(court);
            }
        }
        private int RandomLevel()
        {
            Random r = new Random();
            List<int> nums = new List<int>();
            for (int i = 1; i <= 9; i++)
                for (int j = i; j <= 10; j++)
                    nums.Add(i);
            return nums[r.Next(nums.Count)];
        }
        public FaerieCourt? GetRandomCourt(double limdist = 100, double nullpct = 0)
        {
            return GetRandomCourt(new MapLocation(0,0), limdist, nullpct);
        }
        public FaerieCourt? GetRandomCourt(MapLocation loc, double limdist = 100, double nullpct = 0)
        {
            Random r = new();
            if (r.NextDouble() < nullpct || limdist <= 0)
                return null;
            Dictionary<FaerieCourt, double> CourtPct = new Dictionary<FaerieCourt, double>();
            foreach (FaerieCourt court in Courts)
            {
                double dist = court.Location.GetDistance(loc);
                if (dist < limdist)
                {
                    CourtPct.TryAdd(court, limdist - dist);
                }
            }
            if (CourtPct.Count == 0)
                return null;
            double sum = CourtPct.Values.Sum() * r.NextDouble();
            foreach (KeyValuePair<FaerieCourt, double> court in CourtPct)
            {
                sum -= court.Value;
                if (sum <= 0)
                {
                    return court.Key;
                }
            }
            return null;
        }
        public Faerie GetRandomFaerie(int lvl, bool allownoncourt, int dist = 100, double nullpct = 0)
        {
            int nattempts = 0;
            Faerie? faerie = null;
            while (faerie == null)
            {
                FaerieCourt? court = GetRandomCourt(dist+nattempts, nullpct);
                if (court != null)
                {
                    faerie = court.GetFaerie(lvl);
                }
                if (faerie != null)
                    return faerie;
                if (allownoncourt)
                    return new Faerie(null, lvl);
                nattempts++;
            }
            return faerie;
        }
        public void NormalizeLocations()
        {
            //map normalizing algorithm lol
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < Courts.Count; j++)
                {
                    double closest = 500;
                    int closestindex = -1;
                    for (int k = 0; k < Courts.Count; k++)
                    {
                        if (j == k)
                            continue;
                        double distance = Courts[j].Location.GetDistance(Courts[k].Location);
                        if (distance < closest)
                        {
                            closest = distance;
                            closestindex = k;
                        }
                    }
                    if (closestindex == -1)
                        continue;
                    Direction dir = Courts[j].Location.GetDirection(Courts[closestindex].Location);
                    switch (dir)
                    {
                        case Direction.Ypos: Courts[j].Location.MoveDown(); break;
                        case Direction.Yneg: Courts[j].Location.MoveUp(); break;
                        case Direction.Xpos: Courts[j].Location.MoveLeft(); break;
                        case Direction.Xneg: Courts[j].Location.MoveRight(); break;
                    }
                }
            }
        }
    }
}