using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string WhiteName(BoostStat prim, BoostStat sec)
        {
            switch (prim)
            {
                case BoostStat.Combat:
                    switch (sec)
                    {
                        case BoostStat.Combat: return WhiteCCName();
                        case BoostStat.Magic: return WhiteCMName();
                        case BoostStat.Subtlety: return WhiteCSName();
                        case BoostStat.HP: return WhiteCHName();
                        default: return WhiteCKName();
                    }
                case BoostStat.Magic:
                    switch (sec)
                    {
                        case BoostStat.Combat: return WhiteCMName();
                        case BoostStat.Magic: return WhiteMMName();
                        case BoostStat.Subtlety: return WhiteMSName();
                        case BoostStat.HP: return WhiteMHName();
                        default: return WhiteMKName();
                    }
                case BoostStat.Subtlety:
                    switch (sec)
                    {
                        case BoostStat.Combat: return WhiteCSName();
                        case BoostStat.Magic: return WhiteMSName();
                        case BoostStat.Subtlety: return WhiteSSName();
                        case BoostStat.HP: return WhiteSHName();
                        default: return WhiteSKName();
                    }
                case BoostStat.HP:
                    switch (sec)
                    {
                        case BoostStat.Combat: return WhiteCHName();
                        case BoostStat.Magic: return WhiteMHName();
                        case BoostStat.Subtlety: return WhiteSHName();
                        case BoostStat.HP: return WhiteHHName();
                        default: return WhiteHKName();
                    }
                case BoostStat.Heal:
                    switch (sec)
                    {
                        case BoostStat.Combat: return WhiteCKName();
                        case BoostStat.Magic: return WhiteMKName();
                        case BoostStat.Subtlety: return WhiteSKName();
                        case BoostStat.HP: return WhiteHKName();
                        default: return WhiteKKName();
                    }
                default: return "Angelic Magic";

            }
        }
        private static string WhiteCCName()
        {
            switch (r.Next(4))
            {
                case 0: return "Perfect Body";
                case 1: return "Solar Will";
                case 2: return "Golden Fists";
                default: return "Lightning Fists";
            }
        }
        private static string WhiteCMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Lightning Breath";
                case 1: return "Golden Aura";
                case 2: return "Divine Will";
                default: return "Silver Fists";
            }
        }
        private static string WhiteCSName()
        {
            switch (r.Next(3))
            {
                case 0: return "Perfect Aura";
                case 1: return "Divine Fists";
                default: return "Silver Eyes";
            }
        }
        private static string WhiteCHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Solar Body";
                case 1: return "Golden Skin";
                default: return "Celestial Spirit";
            }
        }
        private static string WhiteCKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Perfect Hands";
                case 1: return "Blessed Spirit";
                default: return "Golden Eyes";
            }
        }
        private static string WhiteMMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Sunfire Magic";
                case 1: return "Lightning Aura";
                case 2: return "Divine Magic";
                default: return "Golden Blood";
            }
        }
        private static string WhiteMSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Light Magic";
                case 1: return "Silver Blood";
                case 2: return "Lightning Will";
                default: return "Lightning Spirit";
            }
        }
        private static string WhiteMHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Golden Will";
                case 1: return "Silver Breath";
                case 2: return "Golden Aura";
                default: return "Divine Body";
            }
        }
        private static string WhiteMKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Holy Blood";
                case 1: return "Golden Hands";
                case 2: return "Golden Breath";
                default: return "Divine Soul";
            }
        }
        private static string WhiteSSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Perfect Breath";
                case 1: return "Lightning Eyes";
                case 2: return "Silver Spirit";
                default: return "Celestial Eyes";
            }
        }
        private static string WhiteSHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Solar Breath";
                case 1: return "Silver Hands";
                default: return "Perfect Will";
            }
        }
        private static string WhiteSKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Blessed Hands";
                case 1: return "Silver Soul";
                default: return "Blessed Eyes";
            }
        }
        private static string WhiteHHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Lightning Body";
                case 1: return "Solar Soul";
                case 2: return "Golden Body";
                default: return "Celestial Blood";
            }
        }
        private static string WhiteHKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Lightning Hands";
                case 1: return "Celestial Body";
                default: return "Divine Spirit";
            }
        }
        private static string WhiteKKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Healing Spirit";
                case 1: return "Holy Breath";
                case 2: return "Celestial Hands";
                default: return "Blessed Will";
            }
        }
    }
}