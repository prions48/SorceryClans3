using SorceryClans3.Data.Models;

namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string GreenName(BoostStat prim, BoostStat sec)
        {
            switch (prim)
            {
                case BoostStat.Combat:
                    switch (sec)
                    {
                        case BoostStat.Combat: return GreenCCName();
                        case BoostStat.Magic: return GreenCMName();
                        case BoostStat.Subtlety: return GreenCSName();
                        case BoostStat.HP: return GreenCHName();
                        default: return GreenCKName();
                    }
                case BoostStat.Magic:
                    switch (sec)
                    {
                        case BoostStat.Combat: return GreenCMName();
                        case BoostStat.Magic: return GreenMMName();
                        case BoostStat.Subtlety: return GreenMSName();
                        case BoostStat.HP: return GreenMHName();
                        default: return GreenMKName();
                    }
                case BoostStat.Subtlety:
                    switch (sec)
                    {
                        case BoostStat.Combat: return GreenCSName();
                        case BoostStat.Magic: return GreenMSName();
                        case BoostStat.Subtlety: return GreenSSName();
                        case BoostStat.HP: return GreenSHName();
                        default: return GreenSKName();
                    }
                case BoostStat.HP:
                    switch (sec)
                    {
                        case BoostStat.Combat: return GreenCHName();
                        case BoostStat.Magic: return GreenMHName();
                        case BoostStat.Subtlety: return GreenSHName();
                        case BoostStat.HP: return GreenHHName();
                        default: return GreenHKName();
                    }
                case BoostStat.Heal:
                    switch (sec)
                    {
                        case BoostStat.Combat: return GreenCKName();
                        case BoostStat.Magic: return GreenMKName();
                        case BoostStat.Subtlety: return GreenSKName();
                        case BoostStat.HP: return GreenHKName();
                        default: return GreenKKName();
                    }
                default: return "Wood Magic";

            }
        }
        private static string GreenCCName()
        {
            switch (r.Next(4))
            {
                case 0: return "Shark Magic";
                case 1: return "Wolf Blood";
                case 2: return "Eagle Totem";
                default: return "Equestrian Soul";
            }
        }
        private static string GreenCMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Thorn Magic";
                case 1: return "Talon Magic";
                case 2: return "Owl Totem";
                default: return "Whale Totem";
            }
        }
        private static string GreenCSName()
        {
            switch (r.Next(3))
            {
                case 0: return "Tiger Totem";
                case 1: return "Animal Shapeshifting";
                default: return "Hawk Blood";
            }
        }
        private static string GreenCHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Toad Magic";
                case 1: return "Boar Magic";
                default: return "Bear Totem";
            }
        }
        private static string GreenCKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Elephant Totem";
                case 1: return "Flower Magic";
                default: return "Venom Mastery";
            }
        }
        private static string GreenMMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Shaman Blood";
                case 1: return "Scarab Totem";
                case 2: return "Peacock Magic";
                default: return "Druid Spirit";
            }
        }
        private static string GreenMSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Wood Magic";
                case 1: return "Forestwalking";
                case 2: return "Fox Totem";
                default: return "Dragonfly Totem";
            }
        }
        private static string GreenMHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Scorpion Blood";
                case 1: return "Druid Blood";
                case 2: return "Rhino Blood";
                default: return "Dragon Blood";
            }
        }
        private static string GreenMKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Slug Magic";
                case 1: return "Fox Blood";
                case 2: return "Dolphin Totem";
                default: return "Crow Totem";
            }
        }
        private static string GreenSSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Plantbending";
                case 1: return "Insect Mastery";
                case 2: return "Spider Totem";
                default: return "Cat Magic";
            }
        }
        private static string GreenSHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Snake Magic";
                case 1: return "Feral Eyes";
                default: return "Spider Blood";
            }
        }
        private static string GreenSKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Moth Totem";
                case 1: return "Raven Magic";
                default: return "Venom Blood";
            }
        }
        private static string GreenHHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Living Spirit";
                case 1: return "Turtle Totem";
                case 2: return "Beetle Magic";
                default: return "Feral Body";
            }
        }
        private static string GreenHKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Butterfly Magic";
                case 1: return "Plant Magic";
                default: return "Lion Totem";
            }
        }
        private static string GreenKKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Herbal Magic";
                case 1: return "Bamboo Spirit";
                case 2: return "Deer Magic";
                default: return "Rabbit Totem";
            }
        }
    }
}