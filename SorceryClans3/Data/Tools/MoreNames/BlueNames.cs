using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        private static string BlueName(int lvl, BoostStat prim, BoostStat sec)
        {
            //this is still a temp, ultimately each of these should return a random set from other functions
            switch (prim)
            {
                case BoostStat.Combat: switch (sec)
                {
                    case BoostStat.Combat: return BlueCCName();
                    case BoostStat.Magic: return BlueCCName();
                    case BoostStat.Subtlety: return BlueCSName();
                    case BoostStat.HP: return BlueCHName();
                    default: return BlueCKName();
                }
                case BoostStat.Magic: switch (sec)
                {
                    case BoostStat.Combat: return BlueCMName(); 
                    case BoostStat.Magic: return BlueMMName(); 
                    case BoostStat.Subtlety: return BlueMSName(); 
                    case BoostStat.HP: return BlueMHName();
                    default: return BlueMKName(); 
                }
                case BoostStat.Subtlety: switch (sec)
                {
                    case BoostStat.Combat: return BlueCSName(); 
                    case BoostStat.Magic: return BlueMSName(); 
                    case BoostStat.Subtlety: return BlueSSName(); 
                    case BoostStat.HP: return BlueSHName(); 
                    default: return BlueSKName();
                }
                case BoostStat.HP: switch (sec)
                {
                    case BoostStat.Combat: return BlueCHName(); 
                    case BoostStat.Magic: return BlueMHName(); 
                    case BoostStat.Subtlety: return BlueSHName(); 
                    case BoostStat.HP: return BlueHHName(); 
                    default: return BlueHKName();
                }
                case BoostStat.Heal: switch (sec)
                {
                    case BoostStat.Combat: return BlueCKName(); 
                    case BoostStat.Magic: return BlueMKName();
                    case BoostStat.Subtlety: return BlueSKName(); 
                    case BoostStat.HP: return BlueHKName(); 
                    default: return BlueKKName();
                }
                default: return "Spirit Magic";
            }
        }
        private static string BlueCCName()
        {
            switch (r.Next(4))
            {
                case 0: return "Earthbending";
                case 1: return "Stone Hands";
                case 2: return "Icebending";
                default: return "Fiery Aura";
            }
        }
        private static string BlueCMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Waterbending";
                case 1: return "Sky Magic";
                case 2: return "Moon Aura";
                default: return "Water Weaving";
            }
        }
        private static string BlueCSName()
        {
            switch (r.Next(3))
            {
                case 0: return "Airbending";
                case 1: return "Wind Magic";
                default: return "Water Walking";
            }
        }
        private static string BlueCHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Stone Skin";
                case 1: return "Sandbending";
                default: return "Stone Will";
            }
        }
        private static string BlueCKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Earth Hands";
                case 1: return "Water Magic";
                default: return "River Magic";
            }
        }
        private static string BlueMMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Spirit Magic";
                case 1: return "Moonlight Magic";
                case 2: return "Starlight Magic";
                default: return "Ice Magic";
            }
        }
        private static string BlueMSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Mystic Eyes";
                case 1: return "Water Breath";
                case 2: return "Starlight Aura";
                default: return "Sand Magic";
            }
        }
        private static string BlueMHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Water Body";
                case 1: return "Cloud Magic";
                case 2: return "Storm Magic";
                default: return "Earth Magic";
            }
        }
        private static string BlueMKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Ocean Magic";
                case 1: return "Spirit Body";
                case 2: return "Spirit Breath";
                default: return "Fountain Magic";
            }
        }
        private static string BlueSSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Ocean Spirit";
                case 1: return "Spirit Eyes";
                case 2: return "Airsense";
                default: return "Earthsense";
            }
        }
        private static string BlueSHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Stone Spirit";
                case 1: return "Crystal Magic";
                default: return "Ocean Body";
            }
        }
        private static string BlueSKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Water Hands";
                case 1: return "Mystic Hands";
                default: return "Crystalbending";
            }
        }
        private static string BlueHHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Stone Body";
                case 1: return "Stormy Aura";
                default: return "Ice Blood";
            }
        }
        private static string BlueHKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Ocean Hands";
                case 1: return "Mystic Heart";
                default: return "Water Skin";
            }
        }
        private static string BlueKKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Spiritbending";
                case 1: return "Water Healing";
                default: return "Water Touch";
            }
        }
    }
}