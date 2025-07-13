using System.ComponentModel.DataAnnotations;
using SorceryClans3.Data.Abstractions;
using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class PowerTemplate : IPower
    {
        [Key] public Guid ID { get; set; } = Guid.NewGuid();
        public Guid? ClanID { get; set; }
        public bool IsTemplate { get { return true; } }
        public string PowerName { get; set; }
		public int CBonusMax { get; set; }
		public int MBonusMax { get; set; }
		public int SBonusMax { get; set; }
		public int HBonusMax { get; set; }
		public int KBonusMax { get; set; }
		public int DBonusMax { get; set; }
        public int GBonusMax { get; set; }
        public int CBonus { get { return CBonusMax; } }
		public int MBonus { get { return MBonusMax; } }
		public int SBonus { get { return SBonusMax; } }
		public int HBonus { get { return HBonusMax; } }
		public int KBonus { get { return KBonusMax; } }
		public int DBonus { get { return DBonusMax; } }
		public int GBonus { get { return GBonusMax; } }
        public double? Heritability { get; set; }
        public string DisplayPercent { get { if (Heritability == null) return ""; return Heritability.Value.ToString("P1"); } }
        public string DisplayType { get { if (Heritability == null) return ""; return "Heritability: "; } }
        public MagicColor Color { get; set; }
		public int MinPowerForColor { get; set; }
		public int PowerIncrementForColor { get; set; }
		public int MaxColors { get; set; }
        
        public PowerTemplate()
        {
            InitPower();
        }
        public PowerTemplate(Guid clanid, int lvl)
        {
            ClanID = clanid;
            InitPower(lvl);
        }
        public PowerTemplate(Guid clanid, MagicColor? color)
        {
            ClanID = clanid;
            InitPower(1, color);
        }
        public PowerTemplate(Guid clanid, int lvl, MagicColor? color)
        {
            ClanID = clanid;
            InitPower(lvl, color);
        }
        public PowerTemplate(Guid clanid, int lvl, MagicColor? color, BoostStat prim)
        {
            ClanID = clanid;
            InitPower(lvl, color, prim);
        }
        private void InitPower(int lvl = 1, MagicColor? forcecolor = null, BoostStat? prim = null, BoostStat? sec = null, BoostStat? tert = null)
        {
            Random r = new Random();
            Heritability = 0.15 + (0.03 * (lvl+1)) + r.NextDouble()*(0.1 * lvl);
            //very simple atm
            if (forcecolor == null)
            {
                //this.Color = (MagicColor)r.Next(7);
                this.Color = MagicColor.Green; //TMP OVERRIDE FOR TESTING
            }
            else
            {
                this.Color = forcecolor.Value;
            }
            if (prim == null)
            {
                prim = (BoostStat)(int)(r.NextDouble()*3.3);
                if (prim == BoostStat.HP)
                    prim = BoostStat.Heal;
            }
            if (sec == null)
            {
                sec = (BoostStat)(int)(r.NextDouble()*4.3);
            }
            if (tert == null)
            {
                tert = (BoostStat)r.Next(4);
            }
            int pts = lvl * 2 + 3;
            int betterpower = 0, betterpower2 = 0, betterpower3 = 0;
            switch (prim)
            {
                case BoostStat.Combat: CBonusMax = 2; break;
                case BoostStat.Magic: MBonusMax = 2; break;
                case BoostStat.Subtlety: SBonusMax = 2; break;
                case BoostStat.HP: HBonusMax = 4; break;//shouldn't happen unless predefined
                case BoostStat.Heal: KBonusMax = 3; pts--; break;
            }
            switch (sec)
            {
                case BoostStat.Combat: CBonusMax++; break;
                case BoostStat.Magic: MBonusMax++; break;
                case BoostStat.Subtlety: SBonusMax++; break;
                case BoostStat.HP: HBonusMax+=2; break;
                case BoostStat.Heal: KBonusMax+=2; pts--; break;
            }
            for (int i = 0; i < pts; i++)
            {
                if (i > lvl && r.NextDouble() < .3 && Color != MagicColor.None)
                {
                    if (r.NextDouble() < .333 && betterpower < 4)
                    {
                        betterpower++;
                        if (r.NextDouble() < .5)
                        {
                            betterpower++;
                            i++;
                        }
                        continue;
                    }
                    else if (r.NextDouble() < .5 && betterpower2 < 3)
                    {
                        betterpower2++;
                        if (r.NextDouble() < .5)
                        {
                            betterpower2++;
                            i++;
                        }
                        continue;
                    }
                    else if (betterpower3 < 3)
                    {
                        betterpower3++;
                        continue;
                    }
                }
                switch (SelectStat(prim.Value,sec.Value,tert.Value))
                {
                    case BoostStat.Combat: CBonusMax++; break;
                    case BoostStat.Magic: MBonusMax++; break;
                    case BoostStat.Subtlety: SBonusMax++; break;
                    case BoostStat.HP: HBonusMax += r.Next(3) + 1; break;
                    case BoostStat.Heal: int k = r.Next(lvl/2+2); 
                    KBonusMax = KBonusMax + k; i += k; ; break;
                }
            }
            this.PowerName = Names.PowerName(this.Color, lvl, prim.Value, sec.Value);
            MinPowerForColor = (r.Next(10) * 50) + (1500 - betterpower*300);
			PowerIncrementForColor = (r.Next(2+lvl) * 50) + 800 + (100*lvl) - (betterpower*250) - (100*betterpower2);
            if (PowerIncrementForColor < 250)
                PowerIncrementForColor = 250;
			MaxColors = r.Next(lvl/2+2, 3+(lvl/2)) + betterpower2*2;
            Heritability += 0.25*betterpower3;
            if (Heritability > .95)
                Heritability = .95;
        }
        public List<MagicColor> GetColors(int powerlevel)
        {
            if (this.Color == MagicColor.None)
                return new List<MagicColor> { MagicColor.None };
            List<MagicColor> ret = new List<MagicColor>();
            for (int i = 0; i < MaxColors; i++)
                ret.Add(this.Color);
            return ret;
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
        public Power? GeneratePower(bool force = false)
		{
			Random r = new Random();
            if (Heritability == null || r.NextDouble() < Heritability || force)
            {
                return new Power()
                {
                    ID = this.ID,
                    PowerName = this.PowerName,
                    CBonusMax = this.CBonusMax,
                    MBonusMax = this.MBonusMax,
                    SBonusMax = this.SBonusMax,
                    HBonusMax = this.HBonusMax,
                    KBonusMax = this.KBonusMax,
                    GBonusMax = this.GBonusMax,
                    Mastery = .1 + .2*r.NextDouble(),
                    Color = this.Color,
                    PowerID = this.ID,
                    MinPowerForColor = this.MinPowerForColor,
                    PowerIncrementForColor = this.PowerIncrementForColor,
                    MaxColors = this.MaxColors,
                };
            }
            return null;
		}
        private BoostStat SelectStat(BoostStat stat1, BoostStat stat2, BoostStat stat3)
        {
            Random r = new Random();
            if (r.NextDouble() < .5)
                return stat1;
            if (r.NextDouble() < .7)
                return stat2;
            return stat3;
        }
    }
}