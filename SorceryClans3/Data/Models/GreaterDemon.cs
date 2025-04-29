using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class GreaterDemon
    {
        public string title;
        public string dname;
        public string DemonName { get { return title + " " + dname; } }
        public DemonType Type { get; set; }
        public StatBlock MinReqs { get; set; }
        public int pb;
        public int cb;
        public int mb;
        public int sb;
        public int kb;
        public int hb;
        public int tb;
        public int gb;
        
        public int pr;
        public int? cr = null;
        public int? mr = null;
        public int? sr = null;
        public int? kr = null;
        public int? tr = null;
        
        private int DemonLevel { get; set; }
        public Soldier? Invested { get; set; }
        
        //public int[] curses;//curses!
        
        public GreaterDemon(int s) //seed runs 1-20+
        {
            Random r = new Random();
            int prim=0,sec=0,tert=0,tough=1;
            Type = (DemonType)r.Next(5);
            for (int i = 0; i < s*2/3+7; i++)
            {
                if (r.NextDouble() < .7)
                    prim++;
                else if (r.NextDouble() < .6)
                    sec++;
                else if (r.NextDouble() < .6)
                    tert++;
                else
                    tough++;
            }
            switch (Type)
            {
                case DemonType.Combat:  cb=prim; tough+=tert;
                    if (r.Next(2)==0) mb=sec; else sb=sec; break;
                case DemonType.Magic: mb=prim; tb=tert; gb=(tert+1)/2;
                    if (r.Next(2)==0) cb=sec; else kb=sec; break;
                case DemonType.Subtlety:  sb=prim; cb=sec; 
                     if (r.Next(2)==0) kb=tert; else tough+=tert; break;
                case DemonType.Healing:  kb=prim; mb=sec; tb=tert; break;
                case DemonType.Travel: default: tb=prim/2; gb=prim/2; sb=sec;
                    if (r.Next(2)==0) cb=tert; else tough+=tert; break;
            }
            hb = tough;
            pb = (r.Next(10)+1)*10+s*150;
            if (s < 5)
            {
                DemonLevel = 1;
                switch (Type)
                {
                    case DemonType.Combat: title="Ruffian"; break;
                    case DemonType.Magic: title="Adept"; break;
                    case DemonType.Subtlety: title="Servitor"; break;
                    case DemonType.Healing: title="Cleric"; break;
                    case DemonType.Travel: default: title="Lookout"; break;
                }
            }
            else if (s < 10)
            {
                DemonLevel = 2;
                switch (Type)
                {
                    case DemonType.Combat: title="Fighter"; break;
                    case DemonType.Magic: title="Shaman"; break;
                    case DemonType.Subtlety: title="Speaker"; break;
                    case DemonType.Healing: title="Priest"; break;
                    case DemonType.Travel: default: title="Scout"; break;
                }
            }
            else if (s < 15)
            {
                DemonLevel = 3;
                switch (Type)
                {
                    case DemonType.Combat: title="Warrior"; break;
                    case DemonType.Magic: title="Sorcerer"; break;
                    case DemonType.Subtlety: title="Rogue"; break;
                    case DemonType.Healing: title="Bishop"; break;
                    case DemonType.Travel: default: title="Traveler"; break;
                }
            }
            else if (s < 20)
            {
                DemonLevel = 4;
                switch (Type)
                {
                    case DemonType.Combat: title="Brbarian"; break;
                    case DemonType.Magic: title="Warlock"; break;
                    case DemonType.Subtlety: title="Assassin"; break;
                    case DemonType.Healing: title="Archbishop"; break;
                    case DemonType.Travel: default: title="Messenger"; break;
                }
            }
            else
            {
                DemonLevel = 5;
                switch (Type)
                {
                    case DemonType.Combat: title="Baron"; break;
                    case DemonType.Magic: title="Prince"; break;
                    case DemonType.Subtlety: title="Duke"; break;
                    case DemonType.Healing: title="Cardinal"; break;
                    case DemonType.Travel: default: title="Earl"; break;
                }
            }
            dname = Names.DemonName();
            pr = (int)(r.NextDouble()*(200+100*s) + s*300 + 1500);
            if (cb > 0) { cr = (int)(r.NextDouble()*4 + s/4 + cb/2); if (cr > 8) cr = 8 + r.Next(3); }
            if (mb > 0) { mr = (int)(r.NextDouble()*4 + s/4 + mb/2); if (mr > 8) mr = 8 + r.Next(3); }
            if (sb > 0) { sr = (int)(r.NextDouble()*4 + s/4 + sb/2); if (sr > 8) sr = 8 + r.Next(3); }
            if (kb > 0) { kr = (int)(r.NextDouble()*4 + s/4 + kb/2); if (kr > 8) kr = 8 + r.Next(3); }
            if (tb > 0) { tr = (int)(r.NextDouble()*4 + s/4 + tb/2); if (tr > 12) tr = 11 + r.Next(3); }
            MinReqs = new StatBlock(cr, mr, sr, null, kr, pr, tr, null, null, null);
        }
        public bool IsEligible(Soldier s)
        {
            return s.Type == SoldierType.Standard && s.Power == null && MinReqs.IsAbove(s);
        }
        public void Apply(Soldier s)
        {
            if (!IsEligible(s))
                return;
            s.ClanName = title;
            s.GivenName = dname;
            s.Power = GeneratePower();
            Invested = s;
        }
        public Power GeneratePower()
        {
            Random r = new Random();
            Power power = new Power();
            power.PowerName = "Power of " + DemonName;
            power.CBonusMax = cb;
            power.MBonusMax = mb;
            power.SBonusMax = sb;
            power.KBonusMax = kb;
            power.HBonusMax = hb;
            power.DBonusMax = tb;
            power.GBonusMax = gb;
            power.Color = MagicColor.Red;
            power.PowerIncrementForColor = 800 + r.Next(8) * 50;
            power.MaxColors = 2 + DemonLevel;
            power.MinPowerForColor = 2000 - power.PowerIncrementForColor;
            power.Mastery = 0.5;//0.1;//testing
            return power;
        }
    }
    public enum DemonType
    {
        Combat = 0,
        Magic = 1,
        Subtlety = 2,
        Healing = 3,
        Travel = 4
    }
}