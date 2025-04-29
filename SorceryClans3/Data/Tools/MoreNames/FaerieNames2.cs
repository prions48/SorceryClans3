using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string TrueName()
        {
            int nsyl = r.Next(5) + 5;
            string ret = "";
            for (int i = 0; i < nsyl; i++)
            {
                ret += FaerieSyllable();
            }
            if (r.Next(2) == 0)
                ret += FaerieConsonant();
            return ret;
        }
        private static string FaerieVowel()
        {
            switch (r.Next(7)) //kek
            {
                case 0: return "a";
                case 1: return "e";
                case 2: return "i";
                case 3: return "o";
                case 4: return "u";
                default: return "y";
            }
        }
        private static string FaerieConsonant()
        {
            switch (r.Next(16))
            {
                case 0: return "c";
                case 1: return "t";
                case 2: return "r";
                case 3: return "b";
                case 4: return "s";
                case 5: return "d";
                case 6: return "l";
                case 7: return "ll";
                case 8: return "m";
                case 9: return "n";
                case 10: return "sh";
                case 11: return "h";
                case 12: return "p";
                case 13: return "v";
                case 14: return "g";
                default: return "w";
            }
        }
        private static string FaerieSyllable()
        {
            return FaerieConsonant() + FaerieVowel() + (r.Next(3) == 0 ? FaerieVowel() : "");
        }
        public static string FaerieTitle(int rank)
        {
            switch (rank)
            {
                case 0: return "Untitled";
                case 1: return "Serf";
                case 2: return "Serf";
                case 3: return "Yeoman";
                case 4: return "Knave";
                case 5: return "Knight";
                case 6: return "Lord";
                case 7: return "Viscount";
                case 8: return "Margrave";
                case 9: return "Prince";
                default: return "Monarch";
            }
        }
        public static string FaerieType(FaerieSeason season, int rank)
        {
            if (season == FaerieSeason.None)
                return WildFaeries(rank);
            return "Faerie";//to do later
        }
        private static string WildFaeries(int rank)
        {
            switch (r.Next(rank))
            {
                case 0: case 1: case 2: return "Redcap";
                case 3: return "Brownie";
                case 4: return "Boggart";
                case 5: return "Kelpie";
                case 6: case 7: return "Hobgoblin";
                default: return "Fomorian";
            }
        }
    }
}