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
        public PowerTemplate? Power { get; set; }
        public StyleTemplate? Style { get; set; }
        public Clan()
        {
            ClanName = Names.ClanName();
            ID = Guid.NewGuid();
            SetStats();
        }
        public Clan(int lvl)
        {
            ClanName = Names.ClanName();
            ID = Guid.NewGuid();
            SetStats(lvl);
        }
        public Clan(int lvl, MagicColor? color)
        {
            ClanName = Names.ClanName();
            ID = Guid.NewGuid();
            SetStats(lvl, color);
        }
        private void SetStats(int elite = 0, MagicColor? color = null)
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
            if (r.NextDouble() < elite * 0.25)
            {
                Power = new PowerTemplate(this.ID, elite, color);
            }
            else if (r.NextDouble() < .5)
            {
                Style = new StyleTemplate(elite);
            }
        }
    }
}