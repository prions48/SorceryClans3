using System.ComponentModel.DataAnnotations;
using SorceryClans3.Data.Abstractions;
using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public enum FaerieSeason
    {
        None = 0,
        Spring = 1,
        Summer = 2,
        Autumn = 3,
        Winter = 4
    }
    public class FaerieProfile
    {
        public FaerieCourt Court { get; set; }
        public int Level { get; set; }
        public int NumFaeries { get; set; }
        public List<Faerie> Faeries { get; set; }
        public int Remaining { get { return NumFaeries - Faeries.Count; } }
        public Faerie GetFaerie()
        {
            Random r = new Random();
            if (r.NextDouble() < Faeries.Count * 1.0 / NumFaeries)
                return Faeries[r.Next(Faeries.Count)];
            Faerie faerie = new Faerie(Court, Level);
            Faeries.Add(faerie);
            return faerie;
        }
    }
    public class FaerieCourt : IMap
    {
        [Key] public Guid ID { get; set; }
        public FaerieSeason Season { get; set; }
        public string CourtName { get; set; }
        public string TooltipText => CourtName;
        public int CourtPower { get; set; }
        public int CourtLevel { get; set; }
        public int NumFaeries { get; set; }
        public MapLocation Location { get; set; } = new MapLocation();
        public List<Faerie> Faeries { get; set; } = new List<Faerie>();
        private List<FaerieProfile> Profiles = new List<FaerieProfile>();
        private int GatheredIntel { get; set; } = 0;
        public void GatherIntel(int intel = 100)
        {
            if (PerceivedLevel != CourtLevel)
            {
                Random r = new Random();
                if (r.Next(3) == 0 && (GatheredIntel % 500) + intel >= 500)
                {
                    if (PerceivedLevel > CourtLevel)
                        PerceivedLevel--;
                    else    
                        PerceivedLevel++;   
                }
            }
            GatheredIntel += intel;
        }
        public FaerieCourt()
        {
            Random r = new Random();
            CourtLevel = r.Next(10)+1; //not the real distribution
            ID = Guid.NewGuid();
            Season = (FaerieSeason)(r.Next(4)+1);
            CourtName = Names.CourtName(Season);
            SetProfiles();
        }
        public FaerieCourt(FaerieSeason season)
        {
            Random r = new Random();
            CourtLevel = r.Next(10)+1; //not the real distribution
            ID = Guid.NewGuid();
            Season = season;
            CourtName = Names.CourtName(Season);
            SetProfiles();
        }
        public FaerieCourt(FaerieSeason season, int lvl)
        {
            CourtLevel = lvl;
            ID = Guid.NewGuid();
            Season = season;
            CourtName = Names.CourtName(Season);
            SetProfiles();
        }
        public FaerieCourt(FaerieSeason season, MapLocation location)
        {
            CourtLevel = 10;
            Location = location;
            ID = Guid.NewGuid();
            Season = season;
            CourtName = Names.CourtName(Season);
            SetProfiles();
        }
        public Faerie? GetFaerie(int lvl)
        {
            FaerieProfile? profile = Profiles.FirstOrDefault(e => e.Level == lvl);
            if (profile == null)
                return null;
            return profile.GetFaerie();
        }
        public void ShuffleName()
        {
            CourtName = Names.CourtName(Season);
        }
        private void SetProfiles()
        {
            if (CourtLevel <= 0)
                CourtLevel = 1;
            if (CourtLevel > 10)
                CourtLevel = 10;
            Random r = new Random();
            PerceivedLevel = r.Next(12);
            NumFaeries = r.Next(10) + r.Next(2 * CourtLevel) + 4 * CourtLevel + 12;
            int maxlvl = CourtLevel;
            if (maxlvl == 8)
                maxlvl = 9;
            if (maxlvl == 7)
                maxlvl = 8;
            if (maxlvl < 7)
                maxlvl = 7;
            for (int i = 1; i <= maxlvl; i++)
            {
                Profiles.Add(new FaerieProfile { Level = i, NumFaeries = 1, Court = this } );
            }
            for (int i = maxlvl; i < NumFaeries; i++)
            {
                Profiles[RandIndex(maxlvl-1)].NumFaeries++;
            }
        }
        private int RandIndex(int lvl)
        {
            Random r = new Random();
            List<int> nums = new List<int>();
            for (int i = 0; i < lvl; i++)
                for (int j = i; j < lvl; j++)
                    nums.Add(i);
            return nums[r.Next(nums.Count)];
        }
        public MudBlazor.Color Color
        {
            get
            {
                switch (Season)
                {
                    case FaerieSeason.Spring: return MudBlazor.Color.Success;
                    case FaerieSeason.Summer: return MudBlazor.Color.Error;
                    case FaerieSeason.Autumn: return MudBlazor.Color.Warning;
                    case FaerieSeason.Winter: return MudBlazor.Color.Info;
                    default: return MudBlazor.Color.Dark;
                }
            }
        }
        private int PerceivedLevel { get; set; }
        public string CourtInfoAdj
        {
            get
            {
                if (GatheredIntel < 500)
                    return "Mysterious";
                if (GatheredIntel < 1000)
                {
                    if (PerceivedLevel <= 5)
                        return "Humble";
                    else
                        return "Mighty";
                }
                if (GatheredIntel < 3000)
                {
                    if (PerceivedLevel <= 3)
                        return "Humble";
                    else if (PerceivedLevel <= 6)
                        return "Modest";
                    else
                        return "Mighty";
                }
                if (GatheredIntel < 5000)
                {
                    if (PerceivedLevel <= 2)
                        return "Humble";
                    if (PerceivedLevel <= 5)
                        return "Modest";
                    if (PerceivedLevel <= 8)
                        return "Impressive";
                    return "Mighty";
                }
                if (GatheredIntel < 8000)
                {
                    if (PerceivedLevel <= 2)
                        return "Humble";
                    if (PerceivedLevel <= 4)
                        return "Modest";
                    if (PerceivedLevel <= 6)
                        return "Robust";
                    if (PerceivedLevel <= 8)
                        return "Impressive";
                    return "Mighty";
                }
                switch (PerceivedLevel)
                {
                    case 1: return "Meek";
                    case 2: return "Humble";
                    case 3: return "Adequate";
                    case 4: return "Modest";
                    case 5: return "Robust";
                    case 6: return "Stalwart";
                    case 7: return "Impressive";
                    case 8: return "August";
                    case 9: return "Mighty";
                    case 10: return "Legendary";
                    default: return "Mysterious";
                }
            }
        }
    }
    
}