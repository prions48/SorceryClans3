namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static (string,double) WeatherName(int lvl)
        {
            switch (r.Next(lvl + 2) + lvl / 2)
            {
                case 0: return ("Breeze",0.5);
                case 1: return ("Cloud",0.8);
                case 2: return ("Rain",0.7);
                case 3: return ("Snow",0);
                case 4: return ("Wind",0.5);
                case 5: return ("Tremor",0.9);
                case 6: return ("Thunderstorm",0.8);
                case 7: return ("Sleet",0.8);
                case 8: return ("Hailstorm",0.2);
                case 9: return ("Lightning Storm",0);
                case 10: return ("Tornado",0);
                case 11: return ("Blizzard",0.9);
                case 12: return ("Earthquake",0.7);
                case 13: return ("Tsunami",0.4);
                case 14: return ("Cyclone",0);
                case 15: return ("Volcano",0);
                default: return ("Hurricane",0);
            }
        }
        public static string WeatherAdj(int lvl)
        {
            switch (r.Next(lvl / 2 + 2) + lvl / 2)
            {
                case 0: return "Small";
                case 1: return "Swirling";
                case 2: return "Dreary";
                case 3: return "Insistent";
                case 4: return "Seasonal";
                case 5: return "Irritating";
                case 6: return "Bitter";
                case 7: return "Stubborn";
                case 8: return "Twisting";
                case 9: return "Angry";
                case 10: return "Spiraling";
                case 11: return "Furious";
                case 12: return "Howling";
                default: return "Devastating";
            }
        }
    }
}