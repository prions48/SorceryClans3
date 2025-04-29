using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string SuperName(BoostStat prim, BoostStat sec)
        {
            switch (prim)
            {
                case BoostStat.Combat:
                    switch (sec)
                    {
                        case BoostStat.Combat: return SuperCCName();
                        case BoostStat.Magic: return SuperCMName();
                        case BoostStat.Subtlety: return SuperCSName();
                        case BoostStat.HP: return SuperCHName();
                        default: return SuperCKName();
                    }
                case BoostStat.Magic:
                    switch (sec)
                    {
                        case BoostStat.Combat: return SuperCMName();
                        case BoostStat.Magic: return SuperMMName();
                        case BoostStat.Subtlety: return SuperMSName();
                        case BoostStat.HP: return SuperMHName();
                        default: return SuperMKName();
                    }
                case BoostStat.Subtlety:
                    switch (sec)
                    {
                        case BoostStat.Combat: return SuperCSName();
                        case BoostStat.Magic: return SuperMSName();
                        case BoostStat.Subtlety: return SuperSSName();
                        case BoostStat.HP: return SuperSHName();
                        default: return SuperSKName();
                    }
                case BoostStat.HP:
                    switch (sec)
                    {
                        case BoostStat.Combat: return SuperCHName();
                        case BoostStat.Magic: return SuperMHName();
                        case BoostStat.Subtlety: return SuperSHName();
                        case BoostStat.HP: return SuperHHName();
                        default: return SuperHKName();
                    }
                case BoostStat.Heal:
                    switch (sec)
                    {
                        case BoostStat.Combat: return SuperCKName();
                        case BoostStat.Magic: return SuperMKName();
                        case BoostStat.Subtlety: return SuperSKName();
                        case BoostStat.HP: return SuperHKName();
                        default: return SuperKKName();
                    }
                default: return "Sharingan";

            }
        }
        private static string SuperCCName()
        {
            return "One for All";
        }
        private static string SuperCMName()
        {
            return "Magnet Mastery";
        }
        private static string SuperCSName()
        {
            return "Super Speed";
        }
        private static string SuperCHName()
        {
            return "Lycanthropy";
        }
        private static string SuperCKName()
        {
            return "Quantum Manipulation";
        }
        private static string SuperMMName()
        {
            return "Gravity Magic";
        }
        private static string SuperMSName()
        {
            return "Psychic Mastery";
        }
        private static string SuperMHName()
        {
            return "Telepathy";
        }
        private static string SuperMKName()
        {
            return "Time Manipulation";
        }
        private static string SuperSSName()
        {
            return "Shapeshifting";
        }
        private static string SuperSHName()
        {
            return "Teleportation";
        }
        private static string SuperSKName()
        {
            return "Matter Transmutation";
        }
        private static string SuperHHName()
        {
            return "Healing Factor";
        }
        private static string SuperHKName()
        {
            return "Life Force Mastery";
        }
        private static string SuperKKName()
        {
            return "Psionic Healing";
        }
    }
}