using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class SpiritWeather : Artifact
    {
        public Guid ID { get; set; }
        public Guid TypeID { get; set; }
        public string WeatherName { get { return EffectAdj + " " + EffectName; } }
        public string EffectName { get; set; } = string.Empty;
        public string EffectAdj { get; set; } = string.Empty;
        public MapLocation Location { get; set; }
        public double EffectiveMajorRange { get; set; }
        public double EffectiveMinorRange { get; set; }
        public double StealthOnly { get; set; }
        private int EffectScale { get; set; } = 1;//larger area
        private int EffectScope { get; set; } = 1;//larger power
        private Random r = new();
        public SpiritWeather(SpiritWeather weather)
        {
            TypeID = weather.ID;
            EffectName = weather.EffectName;
            EffectAdj = weather.EffectAdj;
            Location = weather.Location;
            EffectiveMajorRange = weather.EffectiveMajorRange;
            EffectiveMinorRange = weather.EffectiveMinorRange;
            StealthOnly = weather.StealthOnly;
            EffectScale = weather.EffectScale;
            EffectScope = weather.EffectScope;
            ArtifactName = weather.ArtifactName;
            ComBoost = weather.ComBoost;
            MagBoost = weather.MagBoost;
            SubBoost = weather.SubBoost;
        }
        public SpiritWeather(int lvl)
        {
            Level = lvl;
            for (int i = 2; i < Level; i++) //defaults to 1 and 1, even if overall level is 1
            {
                if (EffectScope < 10 && (r.Next(2) == 0 || EffectScale >= 10))
                    EffectScope++;
                else //overflow will take scope past 10 but not scale
                    EffectScale++;
            }
            var name = Names.WeatherName(EffectScale);
            EffectName = name.Item1;
            StealthOnly = name.Item2;
            EffectAdj = Names.WeatherAdj(EffectScope);
            Location = new MapLocation(20, 5);
            EffectiveMajorRange = 5 * EffectScale + r.NextDouble() * 5;
            EffectiveMinorRange = 5 * EffectScale + r.NextDouble() * 5 + EffectiveMajorRange;
            ComBoost = 1 + lvl / 8;
            MagBoost = 1 + lvl / 8;
            SubBoost = 1 + lvl / 8;
            ArtifactName = "The Staff of the " + EffectName; //tmp
        }
        public int ComEffect(MapLocation? location, bool force = false)
        {
            bool inner = location == null || InMajorRange(location);
            if (r.NextDouble() < StealthOnly && !force)
                return 0;
            return (int)(EffectScale * (force ? StealthOnly : 1) * 100000 / (inner ? 1 : 2)); //tmp
        }
        public int SubEffect(MapLocation? location)
        {
            return (int)(EffectScale * 25000 * (1 + StealthOnly)) / (location == null || InMajorRange(location) ? 1 : 2);
        }
        public bool InRange(MapLocation location)
        {
            return Location.GetDistance(location) < EffectiveMinorRange;
        }
        public bool InMajorRange(MapLocation location)
        {
            return Location.GetDistance(location) < EffectiveMajorRange;
        }
        public string DisplayStealth
        {
            get
            {
                if (StealthOnly < 0.05)
                    return "Full Scale Assault";
                if (StealthOnly < 0.35)
                    return "Widespread Chaos";
                if (StealthOnly < 0.55)
                    return "Visibility and Mobility Hindrance";
                if (StealthOnly < 0.85)
                    return "Confusion and Distraction";
                return "Stealth Only";
            }
        }
        public Artifact GenerateArtifact()
        {
            return new SpiritWeather(this);
        }
    }
}