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
        public static string ContractName()
        {
            switch (r.Next(10))
            {
                case 0: return $"The Merchants' {ContractNoun()} of";
                case 1: return $"The {ContractNoun()} of the Church of";
                case 2: return $"The Traders' {ContractNoun()} of";
                case 3: return $"The {RoyalFigure()} of";
                case 4: return $"The Guards of The {RoyalFigure()} of";
                case 5: return $"The Royal {TradeNoun()} of";
                case 6: return $"The Council of The {RoyalFigure()} of";
                case 7: case 8: return $"The Guild of the {TradeNoun()} of";
                default: return $"The Brokers' {ContractNoun()} of";
            }
        }
        private static string RoyalFigure()
        {
            switch (r.Next(7))
            {
                case 0: return "Prince";
                case 1: return "Queen";
                case 2: return "Vizier";
                case 3: return "Princess";
                case 4: return "Empress";
                case 5: return "Minister";
                default: return "King";
            }
        }
        private static string ContractNoun()
        {
            switch (r.Next(4))
            {
                case 0: return "Guild";
                case 1: return "Council";
                case 2: return "Court";
                default: return "League";
            }
        }
        private static string TradeNoun()
        {
            switch (r.Next(7))
            {
                case 0: return "Goldsmiths";
                case 1: return "Silversmiths";
                case 2: return "Bankers";
                case 3: return "Medics";
                case 4: return "Arms Dealers";
                case 5: return "Apothecaries";
                default: return "Alchemists";
                
            }
        }
    }
}