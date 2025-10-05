namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string RivalNoun()
        {
            switch ((int)(r.NextDouble() * 36)) {
                case 0: return "Dragons";;
                case 1: return "Dreams";
                case 2: return "Ice";
                case 3: return "Storms";
                case 4: return "Towers";
                case 5: return "Lights";
                case 6: return "Stone";
                case 7: return "Jaws";
                case 8: return "Eyes";
                case 9: return "Pain";
                case 10: return "Nails";
                case 11: return "Horns";
                case 12: return "Claws";
                case 13: return "Walls";
                case 14: return "Veils";
                case 15: return "Fires";
                case 16: return "Clouds";
                case 17: return "Arms";
                case 18: return "Demons";
                case 19: return "Spirits";
                case 20: return "Jewels";
                case 21: return "Anvils";
                case 22: return "Feathers";
                case 23: return "Sands";
                case 24: return "Trees";
                case 25: return "Horses";
                case 26: return "Tigers";
                case 27: return "Trumpets";
                case 28: return "Mountains";
                case 29: return "Blades";
                case 30: return "Spears";
                case 31: return "Lanterns";
                case 32: return "Crowns";
                case 33: return "Nights";
                case 34: return "Watchers";
                case 35: return "Spies";
                default: return "Teletubbies";
            }
        }
    }
}