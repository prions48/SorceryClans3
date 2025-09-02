using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class Clan
    {
        public Guid ID { get; set; }
        public string ClanName { get; set; }
        public int ComElite { get; set; }
        public int MagElite { get; set; }
        public int SubElite { get; set; }
        public int HPElite { get; set; }
        public int HealElite { get; set; }
        public int EliteLevel { get; set; }
        public string DisplayStats
        {
            get
            {
                string ret = "";
                if (ComElite != 0)
                    ret += "C:" + (ComElite > 0 ? "+" : "") + ComElite + "  ";
                if (MagElite != 0)
                    ret += "M:" + (MagElite > 0 ? "+" : "") + MagElite + "  ";
                if (SubElite != 0)
                    ret += "S:" + (SubElite > 0 ? "+" : "") + SubElite + "  ";
                if (HPElite != 0)
                    ret += "HP:" + (HPElite > 0 ? "+" : "") + HPElite + "  ";
                if (HealElite != 0)
                    ret += "K:" + (HealElite > 0 ? "+" : "") + HealElite + "  ";
                return ret;
            }
        }
        public PowerTemplate? Power { get; set; }
        public StyleTemplate? Style { get; set; }
        public Clan()
        {
            ClanName = Names.ClanName();
            ID = Guid.NewGuid();
            SetStats();
        }
        public Clan(int lvl, bool? style = null)
        {
            ClanName = Names.ClanName();
            ID = Guid.NewGuid();
            SetStats(lvl, style: style);
        }
        public Clan(int lvl, MagicColor? color, bool? style)
        {
            ClanName = Names.ClanName();
            ID = Guid.NewGuid();
            SetStats(lvl, color, style);
        }
        private void SetStats(int elite = 0, MagicColor? color = null, bool? style = null)
        {
            int pts = elite * 3;
            Random r = new Random();
            for (int i = 0; i < pts; i++)
            {
                if (r.NextDouble() < .1)
                {
                    HealElite++;
                }
                else if (r.NextDouble() < .25)
                {
                    HPElite += r.Next(2)+1;
                }
                else if (r.NextDouble() < .33333)
                {
                    ComElite++;
                }
                else if (r.NextDouble() < .5)
                {
                    MagElite++;
                }
                else
                {
                    SubElite++;
                }
            }
            if (r.NextDouble() < elite * 0.25 && style != true)
            {
                Power = new PowerTemplate(this.ID, elite, color);
            }
            else if (r.NextDouble() < .5 || style == true)
            {
                Style = new StyleTemplate(elite);
            }
        }
    }
}