using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string CityName(int lvl)
        {
            string ret = "";
            switch (lvl)
            {
                case 0: case 1: ret = r.Next(2) == 0 ? "Village" : "Hamlet"; break;
                case 2: ret = r.Next(2) == 0 ? "Township" : "Town"; break;
                case 3: ret = r.Next(2) == 0 ? "Shire" : "City"; break;
                case 4: ret = r.Next(2) == 0 ? "County" : "Metropolis"; break;
                case 5: ret = r.Next(2) == 0 ? "City-State" : "Nation"; break;
            }
            switch (r.Next(4))
            {
                case 0: return $"{ret.NameCase()} {Names.ClanName()}";
                case 1: return $"{ret.NameCase()} of {Names.ClanName()}";
                case 2: return $"{Names.ClanName()} {ret.NameCase()}";
                default: return $"{ret.NameCase()} of {Names.CityAdjName()}";
            }
        }
        private static string CityAdjName()
        {
            switch (r.Next(10)) //can probably go even more ham at some point
            {
                case 0: return "Trees";
                case 1: return "Stones";
                case 2: return "Flames";
                case 3: return "Flowers";
                case 4: return "Hills";
                case 5: return "Rivers";
                case 6: return "Glass";
                case 7: return "Coal";
                case 8: return "Dogs";
                default: return "Winds";
            }
        }
    }
}