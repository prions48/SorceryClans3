using System.ComponentModel.DataAnnotations;
using SorceryClans3.Data.Abstractions;
using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class Power : IPower
    {
        [Key] public Guid ID { get; set; }
        public Guid PowerID { get; set; }
        public bool IsTemplate { get { return false; } }
        public string PowerName { get; set; }
        public int CBonusMax { get; set; }
        public int MBonusMax { get; set; }
        public int SBonusMax { get; set; }
        public int HBonusMax { get; set; }
        public int KBonusMax { get; set; }
        public int DBonusMax { get; set; }
        public int GBonusMax { get; set; }
        public int CBonus { get { return (int)Math.Round(CBonusMax * Mastery); } }
        public int MBonus { get { return (int)Math.Round(MBonusMax * Mastery); } }
        public int SBonus { get { return (int)Math.Round(SBonusMax * Mastery); } }
        public int HBonus { get { return (int)Math.Round(HBonusMax * Mastery); } }
        public int KBonus { get { return (int)Math.Round(KBonusMax * Mastery); } }
        public int DBonus { get { return (int)Math.Round(DBonusMax * Mastery); } }
        public int GBonus { get { return (int)Math.Round(GBonusMax * Mastery); } }
        public double Mastery { get; set; }
        public string DisplayType { get { return "Mastery: "; } }
        public string DisplayPercent { get { return Mastery.ToString("P1"); } }
        public MagicColor Color { get; set; }
        public int MinPowerForColor { get; set; }
        public int PowerIncrementForColor { get; set; }
        public int MaxColors { get; set; }

        public Power()
        {

        }
        public string Icon
        {
            get
            {
                return this.Color.Icon();
            }
        }
        public MudBlazor.Color IconColor
        {
            get
            {
                return this.Color.Color();
            }
        }
        public List<MagicColor> GetColors(int powerlevel)
        {
            List<MagicColor> colors = new List<MagicColor>();
            int c = 0;
            for (int i = MinPowerForColor; i <= powerlevel && i < MinPowerForColor + PowerIncrementForColor * MaxColors; i += PowerIncrementForColor)
            {
                c++;
            }
            double m = Mastery;
            if (m > 1.0)
                m = 1.0;
            c = (int)(c * ((1.1 + m) / 2));
            for (int i = 0; i < c; i++)
                colors.Add(this.Color);
            return colors;
        }
        public void IncreaseMastery(double factor = 0.04)
        {
            if (Mastery >= 1.0)
                return;
            Random r = new();
            Mastery += (r.NextDouble() * factor) + factor;
            if (Mastery > 1.0)
                Mastery = 1.0;
        }
    }
    
}