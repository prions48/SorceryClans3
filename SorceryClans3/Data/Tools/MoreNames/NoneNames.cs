using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string NoneName(BoostStat prim, BoostStat sec)
        {
            string ret = "";
            switch (sec)
            {
                case BoostStat.Combat: ret = CAdj(); break;
                case BoostStat.Magic: ret = MAdj(); break;
                case BoostStat.Subtlety: ret = SAdj(); break;
                case BoostStat.HP: ret = HAdj(); break;
                case BoostStat.Heal: ret = KAdj(); break;
            }
            ret = ret + " ";
            switch (prim)
            {
                case BoostStat.Combat: ret += CNoun(); break;
                case BoostStat.Magic: ret += MNoun(); break;
                case BoostStat.Subtlety: ret += SNoun(); break;
                case BoostStat.HP: ret += HNoun(); break;
                case BoostStat.Heal: ret += KNoun(); break;
            }
            return ret;
        }
        private static string CAdj()
        {
            switch (r.Next(2))
            {
                case 0: return "Mighty";
                default: return "Toxic";
            }
        }
        private static string MAdj()
        {
            switch (r.Next(2))
            {
                case 0: return "Psychic";
                default: return "Arcane";
            }
        }
        private static string SAdj()
        {
            switch (r.Next(2))
            {
                case 0: return "Silent";
                default: return "Elusive";
            }
        }
        private static string HAdj()
        {
            switch (r.Next(2))
            {
                case 0: return "Stoic";
                default: return "Resilient";
            }
        }
        private static string KAdj()
        {
            switch (r.Next(2))
            {
                case 0: return "Patient";
                default: return "Vital";
            }
        }
        private static string CNoun()
        {
            switch (r.Next(3))
            {
                case 0: return "Fists";
                case 1: return "Bones";
                default: return "Arms";
            }
        }
        private static string MNoun()
        {
            switch (r.Next(3))
            {
                case 0: return "Mind";
                case 1: return "Spirit";
                default: return "Soul";
            }
        }
        private static string SNoun()
        {
            switch (r.Next(3))
            {
                case 0: return "Aura";
                case 1: return "Feet";
                default: return "Will";
            }
        }
        private static string HNoun()
        {
            switch (r.Next(3))
            {
                case 0: return "Body";
                case 1: return "Skin";
                default: return "Flesh";
            }
        }
        private static string KNoun()
        {
            switch (r.Next(2))
            {
                case 0: return "Breath";
                default: return "Hands";
            }
        }
    }
}