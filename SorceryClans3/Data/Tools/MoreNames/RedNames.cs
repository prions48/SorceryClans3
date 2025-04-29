using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
		public static string RedName(BoostStat prim, BoostStat sec)
		{
			switch (prim)
			{
				case BoostStat.Combat: switch (sec)
					{
						case BoostStat.Combat: return RedCCName();
						case BoostStat.Magic: return RedCMName();
						case BoostStat.Subtlety: return RedCSName();
						case BoostStat.HP: return RedCHName();
						default: return RedCKName();
                    }
				case BoostStat.Magic: switch (sec)
					{
                        case BoostStat.Combat: return RedCMName();
                        case BoostStat.Magic: return RedMMName();
                        case BoostStat.Subtlety: return RedMSName();
                        case BoostStat.HP: return RedMHName();
                        default: return RedMKName();
                    }
                case BoostStat.Subtlety: switch (sec)
                    {
                        case BoostStat.Combat: return RedCSName();
                        case BoostStat.Magic: return RedMSName();
                        case BoostStat.Subtlety: return RedSSName();
                        case BoostStat.HP: return RedSHName();
                        default: return RedSKName();
                    }
                case BoostStat.HP: switch (sec)
                    {
                        case BoostStat.Combat: return RedCHName();
                        case BoostStat.Magic: return RedMHName();
                        case BoostStat.Subtlety: return RedSHName();
                        case BoostStat.HP: return RedHHName();
                        default: return RedHKName();
                    }
                case BoostStat.Heal: switch (sec)
                    {
                        case BoostStat.Combat: return RedCKName();
                        case BoostStat.Magic: return RedMKName();
                        case BoostStat.Subtlety: return RedSKName();
                        case BoostStat.HP: return RedHKName();
                        default: return RedKKName();
                    }
                default: return "Demonic Magic";

            }
		}
		private static string RedCCName()
		{
			switch (r.Next(4))
			{
				case 0: return "Firebending";
				case 1: return "Fiery Body";
				case 2: return "Monstrous Blood";
				default: return "Lavabending";
            }
		}
        private static string RedCMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Fire Magic";
                case 1: return "Magma Magic";
                case 2: return "Infernal Will";
                default: return "Fiery Hands";
            }
        }
        private static string RedCSName()
        {
            switch (r.Next(3))
            {
                case 0: return "Fiery Eyes";
                case 1: return "Infernal Aura";
                default: return "Demon Blood";
            }
        }
        private static string RedCHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Fiery Blood";
                case 1: return "Chaos Body";
                default: return "Lava Body";
            }
        }
        private static string RedCKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Fiery Soul";
                case 1: return "Demonic Hands";
                default: return "Infernal Breath";
            }
        }
        private static string RedMMName()
        {
            switch (r.Next(4))
            {
                case 0: return "Witch Blood";
                case 1: return "Fiery Aura";
                case 2: return "Chaos Summoning";
                default: return "Volcano Magic";
            }
        }
        private static string RedMSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Cursed Eyes";
                case 1: return "Devilish Body";
                case 2: return "Devilish Will";
                default: return "Chaotic Soul";
            }
        }
        private static string RedMHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Cursed Soul";
                case 1: return "Chaos Magic";
                case 2: return "Chaotic Will";
                default: return "Fiery Will";
            }
        }
        private static string RedMKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Voodoo Magic";
                case 1: return "Marrow Magic";
                case 2: return "Witch Aura";
                default: return "Bloody Hands";
            }
        }
        private static string RedSSName()
        {
            switch (r.Next(4))
            {
                case 0: return "Devilish Hands";
                case 1: return "Slime Magic";
                case 2: return "Infernal Eyes";
                default: return "Bloodbending";
            }
        }
        private static string RedSHName()
        {
            switch (r.Next(3))
            {
                case 0: return "Devilish Eyes";
                case 1: return "Demonic Soul";
                default: return "Basalt Hands";
            }
        }
        private static string RedSKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Cursed Fate";
                case 1: return "Devilish Spirit";
                default: return "Infernal Hands";
            }
        }
        private static string RedHHName()
        {
            switch (r.Next(4))
            {
                case 0: return "Magma Body";
                case 1: return "Basalt Skin";
                case 2: return "Cursed Blood";
                default: return "Demonic Body";
            }
        }
        private static string RedHKName()
        {
            switch (r.Next(3))
            {
                case 0: return "Cursed Body";
                case 1: return "Blood Magic";
                default: return "Infernal Soul";
            }
        }
        private static string RedKKName()
        {
            switch (r.Next(4))
            {
                case 0: return "Cursed Hands";
                case 1: return "Infernal Touch";
                case 2: return "Devilish Blood";
                default: return "Fiery Spirit";
            }
        }
    }
}