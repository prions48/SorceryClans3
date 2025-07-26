using SorceryClans3.Data.Models;

namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string UndeadArtName(int lvl)
        {
            return UndeadAdj(lvl) + " " + UndeadName(lvl);
        }
        private static string UndeadName(int lvl)
        {
            switch (r.Next(5 + lvl / 2))
            {
                case 0: return "Egui";
                case 1: return "Haint";
                case 2: return "Poltergeist";
                case 3: return "Dybbuk";
                case 4: return "Obambo";
                case 5: return "Llorona";
                case 6: return "Shade";
                case 7: return "Yurei";
                case 8: return "Phantome";
                case 9: return "Kikimora";
                case 10: return "Draugr";
                case 11: return "Gwisin";
                case 12: return "Krasue";
                case 13: return "Preta";
                default: return "Ghost";
            }
        }
        private static string UndeadAdj(int lvl)
        {
            switch (r.Next(5 + lvl / 2))
            {
                case 0: return "Hungry ";
                case 1: return "Angry ";
                case 2: return "Chilling ";
                case 3: return "Silent ";
                case 4: return "Screaming ";
                case 5: return "Singing ";
                case 6: return "Lost ";
                case 7: return "Wailing ";
                case 8: return "Screeching ";
                case 9: return "Burning ";
                case 10: return "Moaning ";
                case 11: return "Bleeding ";
                case 12: return "Violent ";
                default: return "Despairing ";
            }
        }
        public static (string,ArtIcon) UndeadArt()
        {
            switch (r.Next(10))
            {
                case 0: return ("Sword", ArtIcon.Sword);
                case 1: return ("Bell", ArtIcon.Bell);
                case 2: return ("Dagger", ArtIcon.Knife);
                case 3: return ("Hammer", ArtIcon.Hammer);
                case 4: return ("Jewel", ArtIcon.Jewel);
                case 5: return ("Axe", ArtIcon.Axe);
                case 6: return ("Amulet", ArtIcon.Necklace);
                case 7: return ("Wand", ArtIcon.Wand);
                case 8: return ("Rod", ArtIcon.Wand);
                default: return ("Lantern", ArtIcon.Candle);
            }
        }
    }
}