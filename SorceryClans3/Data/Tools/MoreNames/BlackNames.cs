using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        private static string BlackName(int lvl, BoostStat prim, BoostStat sec)
        {
            //this is still a temp, ultimately each of these should return a random set from other functions
            switch (prim)
            {
                case BoostStat.Combat:
                    switch (sec)
                    {
                        case BoostStat.Combat: return BlackCCName();
                        case BoostStat.Magic: return BlackCMName();
                        case BoostStat.Subtlety: return BlackCSName();
                        case BoostStat.HP: return BlackCHName();
                        default: return BlackCKName();
                    }
                case BoostStat.Magic:
                    switch (sec)
                    {
                        case BoostStat.Combat: return BlackCMName();
                        case BoostStat.Magic: return BlackMMName();
                        case BoostStat.Subtlety: return BlackMSName();
                        case BoostStat.HP: return BlackMHName();
                        default: return BlackMKName();
                    }
                case BoostStat.Subtlety:
                    switch (sec)
                    {
                        case BoostStat.Combat: return BlackCSName();
                        case BoostStat.Magic: return BlackMSName();
                        case BoostStat.Subtlety: return BlackSSName();
                        case BoostStat.HP: return BlackSHName();
                        default: return BlackSKName();
                    }
                case BoostStat.HP:
                    switch (sec)
                    {
                        case BoostStat.Combat: return BlackCHName();
                        case BoostStat.Magic: return BlackMHName();
                        case BoostStat.Subtlety: return BlackSHName();
                        case BoostStat.HP: return BlackHHName();
                        default: return BlackHKName();
                    }
                case BoostStat.Heal:
                    switch (sec)
                    {
                        case BoostStat.Combat: return BlackCKName();
                        case BoostStat.Magic: return BlackMKName();
                        case BoostStat.Subtlety: return BlackSKName();
                        case BoostStat.HP: return BlackHKName();
                        default: return BlackKKName();
                    }
                default: return "Necromancy";
            }
        }
        private static string BlackCCName()
        {
            switch (r.Next(4))
            {
                case 0: return "Hungry Flesh";
                case 1: return "Bone Claws";
                case 2: return "Obsidian Hands";
                default: return "Skeletal Body";
            }
        }
        private static string BlackCMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Necromancy";
                case 1: return "Evil Hands";
                case 2: return "Obsidian Will";
                default: return "Unholy Spirit";
            }
        }
        private static string BlackCSName()
        {
            switch (r.Next(3))
            {
                case 0: return "Ghost Blood";
                case 1: return "Ghost Hands";
                default: return "Assassin Soul";
            }
        }
        private static string BlackCHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Wight Blood";
                case 1: return "Bone Mastery";
                default: return "Obsidian Skin";
            }
        }
        private static string BlackCKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Life Draining";
                case 1: return "Bone Magic";
                default: return "Obsidian Eyes";
            }
        }
        private static string BlackMMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Necromancy";
                case 1: return "Lich Blood";
                case 2: return "Necrotic Soul";
                default: return "Evil Spirit";
            }
        }
        private static string BlackMSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Shadow Blood";
                case 1: return "Shadowbending";
                case 2: return "Shadowy Aura";
                default: return "Evil Eyes";
            }
        }
        private static string BlackMHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Hungry Soul";
                case 1: return "Shadow Magic";
                case 2: return "Shadowy Will";
                default: return "Unholy Body";
            }
        }
        private static string BlackMKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Vampire Soul";
                case 1: return "Voodoo Magic";
                case 2: return "Unholy Will";
                default: return "Unholy Touch";
            }
        }
        private static string BlackSSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Shadowwalking";
                case 1: return "Soul Sense";
                case 2: return "Vampire Blood";
                default: return "Deathwalking";
            }
        }
        private static string BlackSHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Phantasm Blood";
                case 1: return "Ghost Body";
                default: return "Shadowy Eyes";
            }
        }
        private static string BlackSKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Shadowsoul";
                case 1: return "Hungry Eyes";
                default: return "Deathspeaking";
            }
        }
        private static string BlackHHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Shadow Body";
                case 1: return "Mummy Blood";
                case 2: return "Zombie Flesh";
                default: return "Evil Body";
            }
        }
        private static string BlackHKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Hungry Aura";
                case 1: return "Shadowy Hands";
                default: return "Necrotic Hands";
            }
        }
        private static string BlackKKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Death Mastery";
                case 1: return "Soul Manipulation";
                case 2: return "Hungry Flesh";
                default: return "Undying Spirit";
            }
        }
    }
}