using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        private static string PurpleName(int lvl, BoostStat prim, BoostStat sec)
        {
            //this is still a temp, ultimately each of these should return a random set from other functions
            switch (prim)
            {
                case BoostStat.Combat:
                    switch (sec)
                    {
                        case BoostStat.Combat: return PurpleCCName();
                        case BoostStat.Magic: return PurpleCMName();
                        case BoostStat.Subtlety: return PurpleCSName();
                        case BoostStat.HP: return PurpleCHName();
                        default: return PurpleCKName();
                    }
                case BoostStat.Magic:
                    switch (sec)
                    {
                        case BoostStat.Combat: return PurpleCMName();
                        case BoostStat.Magic: return PurpleMMName();
                        case BoostStat.Subtlety: return PurpleMSName();
                        case BoostStat.HP: return PurpleMHName();
                        default: return PurpleMKName();
                    }
                case BoostStat.Subtlety:
                    switch (sec)
                    {
                        case BoostStat.Combat: return PurpleCSName();
                        case BoostStat.Magic: return PurpleMSName();
                        case BoostStat.Subtlety: return PurpleSSName();
                        case BoostStat.HP: return PurpleSHName();
                        default: return PurpleSKName();
                    }
                case BoostStat.HP:
                    switch (sec)
                    {
                        case BoostStat.Combat: return PurpleCHName();
                        case BoostStat.Magic: return PurpleMHName();
                        case BoostStat.Subtlety: return PurpleSHName();
                        case BoostStat.HP: return PurpleHHName();
                        default: return PurpleHKName();
                    }
                case BoostStat.Heal:
                    switch (sec)
                    {
                        case BoostStat.Combat: return PurpleCKName();
                        case BoostStat.Magic: return PurpleMKName();
                        case BoostStat.Subtlety: return PurpleSKName();
                        case BoostStat.HP: return PurpleHKName();
                        default: return PurpleKKName();
                    }
                default: return "Necromancy";
            }
        }
        private static string PurpleCCName()
        {
            switch (r.Next(4))
            {
                case 0: return "Ogre Blood";
                case 1: return "Troll Spirit";
                case 2: return "Battle Aura";
                default: return "Soul of the Sword";
            }
        }
        private static string PurpleCMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Summer Spirit";
                case 1: return "Elven Blood";
                case 2: return "Hunter Spirit";
                default: return "Wyld Blood";
            }
        }
        private static string PurpleCSName()
        {
            switch (r.Next(3))
            {
                case 0: return "Gossamer Weaving";
                case 1: return "Autumn Spirit";
                default: return "Hunter Aura";
            }
        }
        private static string PurpleCHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Troll Blood";
                case 1: return "Raven Wings";
                default: return "Hunter Will";
            }
        }
        private static string PurpleCKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Springtime Hands";
                case 1: return "Elven Will";
                default: return "Heart of the Staff";
            }
        }
        private static string PurpleMMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Sylvan Blood";
                case 1: return "Summer Soul";
                case 2: return "Wyld Magic";
                default: return "Indomitable Will";
            }
        }
        private static string PurpleMSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Trickster Spirit";
                case 1: return "Faeriedust Magic";
                case 2: return "Heart of the Ring";
                default: return "Tarot Magic";
            }
        }
        private static string PurpleMHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Sprite Blood";
                case 1: return "Soul of the Ring";
                case 2: return "Bat Wings";
                default: return "Summer Aura";
            }
        }
        private static string PurpleMKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Summer Hands";
                case 1: return "Butterfly Wings";
                case 2: return "Body of the Sword";
                default: return "Heart of the Cup";
            }
        }
        private static string PurpleSSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Illusion Mastery";
                case 1: return "Autumn Aura";
                case 2: return "Tarot Reading";
                default: return "Pixie Spirit";
            }
        }
        private static string PurpleSHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Shapeshifting";
                case 1: return "Body of the Ring";
                default: return "Heart of the Sword";
            }
        }
        private static string PurpleSKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Glamour Magic";
                case 1: return "Body of the Cup";
                default: return "Autumn Breath";
            }
        }
        private static string PurpleHHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Winter Body";
                case 1: return "Soul of the Staff";
                case 2: return "Metal Wings";
                default: return "Troll Body";
            }
        }
        private static string PurpleHKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Eagle Wings";
                case 1: return "Winter Breath";
                default: return "Body of the Staff";
            }
        }
        private static string PurpleKKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Summer Hands";
                case 1: return "Springtime Breath";
                case 2: return "Sylvan Spirit";
                default: return "Soul of the Cup";
            }
        }
    }
}