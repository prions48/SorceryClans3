using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Abstractions
{
    public interface IPower
    {
        public string PowerName { get; set; }
        public bool IsTemplate { get; }
        public int CBonusMax { get; set; }
		public int MBonusMax { get; set; }
		public int SBonusMax { get; set; }
		public int HBonusMax { get; set; }
		public int KBonusMax { get; set; }
		public int DBonusMax { get; set; }
		public int GBonusMax { get; set; }
        public int CBonus { get; }
        public int MBonus { get; }
        public int SBonus { get; }
        public int HBonus { get; }
        public int KBonus { get; }
        public int DBonus { get; }
        public int GBonus { get; }
        public string DisplayType { get; }
        public string DisplayPercent { get; }
        public string Icon { get; }
        public MagicColor Color { get; set; }
        public int MinPowerForColor { get; set; }
		public int PowerIncrementForColor { get; set; }
		public int MaxColors { get; set; }
        public MudBlazor.Color IconColor { get; }
        public IList<MagicColor> GetColors(int powerlevel);
    }
}