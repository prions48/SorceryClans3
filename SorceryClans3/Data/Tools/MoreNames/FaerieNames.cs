using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string CourtName(FaerieSeason season)
        {
            switch (season)
            {
                case FaerieSeason.Spring: return SpringCourtName();
                case FaerieSeason.Summer: return SummerCourtName();
                case FaerieSeason.Autumn: return AutumnCourtName();
                case FaerieSeason.Winter: return WinterCourtName();
                default: return "";
            }
        }
        public static string SpringCourtName()
        {
            if (r.Next(3) == 0) //green
            {
                string[] ret = ["Viridian", "Celadon", "Chartreuse",
                        "Olive", "Shamrock", "Teal" ];
                return ret[r.Next(ret.Length)];
            }
            else if (r.Next(2) == 0) //gems
            {
                string[] ret = ["Emerald", "Tourmaline", "Alexandrite",
                        "Jade", "Peridot", "Malachite", "Spinel"];
                return ret[r.Next(ret.Length)];
            }
            else //flowers
            {
                string[] ret = ["Rose", "Lily", "Orchid",
                        "Daisy", "Tulip", "Iris", "Chrysanthemum"];
                return ret[r.Next(ret.Length)];
            }
        }
        public static string SummerCourtName()
        {
            if (r.Next(3) == 0) //red
            {
                string[] ret = ["Crimson", "Scarlet", "Vermilion", "Cardinal",
                        "Burgundy", "Coral", "Cerise" ];
                return ret[r.Next(ret.Length)];
            }
            else if (r.Next(2) == 0) //gems
            {
                string[] ret = ["Ruby", "Topaz", "Garnet",
                        "Rhodonite", "Pyrope", "Corundum", "Jasper"];
                return ret[r.Next(ret.Length)];
            }
            else //trees
            {
                string[] ret = ["Oak", "Pine", "Elm", "Willow", "Magnolia", "Sequoia",
                        "Rowan", "Hawthorn", "Eucalyptus", "Redwood"];
                return ret[r.Next(ret.Length)];
            }
        }
        public static string AutumnCourtName()
        {
            if (r.Next(3) == 0) //yellow/orange
            {
                string[] ret = ["Gold", "Amber", "Ivory", "Maize",
                        "Marigold", "Saffron", "Poppy" ];
                return ret[r.Next(ret.Length)];
            }
            else if (r.Next(2) == 0) //gems
            {
                string[] ret = ["Beryl", "Quartz", "Citrine",
                        "Heliodor", "Chrysoberyl", "Calcite", "Aragonite"];
                return ret[r.Next(ret.Length)];
            }
            else //harvest
            {
                string[] ret = ["Wheat", "Corn", "Barley", "Millet",
                        "Rice", "Sorghum", "Cotton", "Pumpkin"];
                return ret[r.Next(ret.Length)];
            }
        }
        public static string WinterCourtName()
        {
            if (r.Next(3) == 0) //blue
            {
                string[] ret = ["Cerulean", "Azure", "Cyan", "Indigo",
                        "Aquamarine", "Periwinkle", "Ultramarine" ];
                return ret[r.Next(ret.Length)];
            }
            else if (r.Next(2) == 0) //gems
            {
                string[] ret = ["Sapphire", "Turquoise", "Opal",
                        "Chalcedony", "Obsidian" ];
                return ret[r.Next(ret.Length)];
            }
            else //stone
            {
                string[] ret = ["Granite", "Basalt", "Marble", "Sandstone",
                        "Slate", "Chalk", "Limestone", "Coal"];
                return ret[r.Next(ret.Length)];
            }
        }
    }
}