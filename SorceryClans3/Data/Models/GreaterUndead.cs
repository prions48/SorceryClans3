namespace SorceryClans3.Data.Models
{
    public class GreaterUndead
    {
        public string utype;
        public string adj;
        public string UndeadName { get { return adj + " " + utype; } }
        public int pbonus;
        public int cbonus;
        public int mbonus;
        public int sbonus;
        public int hbonus;
        
        private StatBlock MinReqs;
        public int preq;
        public int creq;
        public int mreq;
        public int sreq;
        
        public GreaterUndead(int seed)
        {
            Random r = new Random();
            //only available when seed is greater than 3
            int unit = (seed-1)/3 + 2;
            pbonus = 500 * unit + r.Next(4)*50;
            cbonus = 0;
            mbonus = 0;
            sbonus = 0;
            hbonus = 0;
            if (seed < 10) //seed 4...9
            {
                switch (r.Next(3)+(seed-3)/2) {
                    case 0: utype = "Fiend"; cbonus = 2 * unit; sbonus = unit; break;
                    case 1: utype = "Mordicant"; cbonus = 3 * unit; break;
                    case 2: utype = "Revenant"; cbonus = unit; mbonus = unit; hbonus = unit; break;
                    case 3: utype = "Wraith"; mbonus = unit; sbonus = 2 * unit; break;
                    case 4: utype = "Banshee"; mbonus = 2 * unit; cbonus = unit; break;
                    case 5: utype = "Wight"; cbonus = 2 * unit; hbonus = unit; break;
                    default: utype = "Bunny"; break;
                }
                switch (r.Next(6)+((seed-4)/2)) {
                    case 0: adj = "Crawling"; cbonus+=unit; sbonus+=unit; break;
                    case 1: adj = "Slime"; cbonus+=unit; cbonus+=unit; cbonus+=unit; break;
                    case 2: adj = "Blood"; mbonus+=2*unit; break;
                    case 3: adj = "Fire"; cbonus+=unit; mbonus+=unit; break;
                    case 4: adj = "Hungry"; cbonus+=unit; hbonus+=3; break;
                    case 5: adj = "Ice"; mbonus+=2*unit; break;
                    case 6: adj = "Silent"; sbonus+=2*unit; break;
                    case 7: adj = "Shrieking"; cbonus+=unit; hbonus+=unit; break;
                    default: adj = "Fluffy"; break;
                }
            }
            else //big!
            {
                switch (r.Next(4)) {
                    case 0: utype = "Lich"; cbonus = unit; mbonus = 2 * unit; break;
                    case 1: utype = "Vampire"; cbonus = unit; mbonus = unit; sbonus = unit; break;
                    case 2: utype = "Nightwalker"; cbonus = 2*unit; sbonus = unit; break;
                    case 3: utype = "Spectre"; sbonus = 2 * unit; mbonus = unit; break;
                    default: utype = "Bunny"; break;
                }
                switch (r.Next(8))
                {
                    case 0: adj = "Blood"; mbonus+=2*unit; break;
                    case 1: adj = "Fire"; cbonus+=unit; mbonus+=unit; break;
                    case 2: adj = "Deathly"; mbonus+=unit; sbonus+=unit; break;
                    case 3: adj = "Undying"; hbonus+=2*unit; break;
                    case 4: adj = "Nether"; mbonus += unit; cbonus += unit/2; sbonus += unit/2; break;
                    case 5: adj = "Astral"; sbonus += 2 * unit; break;
                    case 6: adj = "Obsidian"; cbonus += 2 * unit; break;
                    case 7: adj = "Winged"; cbonus += unit; sbonus += unit; break;
                    default: adj = "Fluffy"; break;
                }
            }
            preq = (seed-3)*700;
            creq = (int)(cbonus * r.Next(2));
            mreq = (int)(mbonus * (r.NextDouble()+.5));
            sreq = (int)(sbonus * (r.NextDouble()+.5));
            if (creq > seed/2+3) creq = seed/2+3;
            if (mreq > seed/2+3) mreq = seed/2+3;
            if (sreq > seed/2+3) sreq = seed/2+3;
            MinReqs = new StatBlock(creq, mreq, sreq, null, null, preq, null, null, null, null);
        }
        public bool IsEligible(Soldier sold)
        {
            return sold.Type == SoldierType.Standard && MinReqs.IsAbove(sold);
        }
        public void Apply(Soldier sold)
        {
            sold.Type = SoldierType.GreaterUndead;
            sold.PowerLevel += this.pbonus;
            sold.ComBase += this.cbonus;
            sold.MagBase += this.mbonus;
            sold.SubBase += this.sbonus;
            sold.HPBase += this.hbonus;
            sold.HPCurrent = sold.HPMax;
        }
    }
}