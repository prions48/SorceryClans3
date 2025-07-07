namespace SorceryClans3.Data.Models
{
    public class LesserUndead
    {
        public Guid ID { get; set; }
        private string utype;
        private string adj;
        public string tool;
        
        private int pscore;
        private int cscore;
        private int mscore;
        private int sscore;
        private int hscore;
        private int tscore;
        public string UndeadName
        {
            get
            {
                return adj + " " + utype;
            }
        }
        public LesserUndead(int seed)
        {
            Random r = new Random();
            switch (r.Next(6)) {
                case 0: tool = "gem"; break;
                case 1: tool = "crystal"; break;
                case 2: tool = "rune"; break;
                case 3: tool = "sigil"; break;
                case 4: tool = "dagger"; break;
                default: tool = "blade"; break;
            }
            pscore = (int)(seed*Math.Sqrt(seed)) * 100 + r.Next(seed/2)*100 + (seed+1)*200;
            cscore = 4;
            mscore = 0;
            sscore = 0;
            //seed runs from 1 to 10  //2020 uhhh does it really though?
            if (seed <= 3)
            {
                int adjseed = r.Next(8);
                hscore = 4;
                if (r.NextDouble() > .8)
                {
                    if (r.NextDouble() < .6)
                        utype = "Gorecrow";
                    else
                        utype = "Gorestoat";
                    cscore = 0;
                    tscore = 12;
                    mscore = 0;
                    sscore = seed;
                    adjseed = r.Next(5);
                }
                else if (r.NextDouble() > .5)
                {
                    utype = "Zombie";
                    tscore = 6;
                    cscore = seed+1;
                    mscore = 0;
                    sscore = 0;
                    hscore+=seed+3;
                }
                else
                {
                    utype = "Skeleton";
                    tscore = 9;
                    cscore = seed/2 + seed%2;
                    mscore = seed/2;
                    sscore = 0;
                }
                switch (adjseed) {
                    case 0: adj = "Large"; cscore+=seed/2+2; hscore+=seed+2; break;
                    case 1: adj = "Black"; sscore+=seed+1; tscore++; break;
                    case 2: adj = "Red"; cscore++; mscore+=seed; sscore+=seed/2+1; break;
                    case 3: adj = "Angry"; cscore +=seed+2; break;
                    case 4: adj = "Silver"; cscore++; mscore += seed/2+2; tscore+=seed/3; break;
                    case 5: adj = "Water"; cscore += seed/2+1; sscore += seed/2+1; break;
                    case 6: adj = "Enchanted"; mscore+=seed+2; tscore++; break;
                    case 7: adj = "Armored"; cscore+=seed/2+2; tscore--; hscore+=seed+1; break;
                    default: adj = "Fluffy"; break;
                }
            }
            else if (seed >= 8) //2020 this needs some spiffing up
            {
                adj = "Skeletal";
                hscore = seed+2+r.Next(4);
                switch (r.Next(4)+seed/10+seed/12+seed/15) { //0-3 for <10 up to 3-6 for >=15
                    case 0: utype = "Steed"; cscore = 4+seed; sscore = seed/2; tscore=15; break;
                    case 1: utype = "Warrior"; cscore = 4+seed/2; mscore = 4+seed/2; sscore=4+seed/2; tscore=12; break;
                    case 2: utype = "Hound"; cscore = seed/2+seed/4+r.Next(3); sscore = seed+2; tscore=15; break;
                    case 3: utype = "Warlock"; mscore = seed+2; cscore = seed/2; tscore=10; break;
                    default:
                    cscore = seed/3; mscore = seed/3; sscore = seed/3;
                    if (r.Next(3) == 0) { utype = "Behemoth"; cscore += seed/2; }
                    else if (r.Next(2) == 0) { utype = "Giant"; sscore += seed/2; }
                    else { utype = "Dragon"; mscore += seed/2; }
                    hscore += seed; int bextra = r.Next(seed/2)+seed/4; tscore=12;
                    if (r.Next(3)==0) { mscore += bextra; cscore += bextra/2; }
                    else if (r.Next(2)==0) { cscore+=bextra/3; sscore += bextra/2; }
                    else cscore += bextra; break; //seed>=10 has a chance!
                }
            }
            else //seed 4...7
            {
                hscore = seed+2;
                switch (r.Next(3)+(seed-4)/2) {
                    case 0: utype = "Skeleton"; cscore = seed+2; tscore=10; hscore++; break;
                    case 1: utype = "Ghost"; sscore = seed+1; tscore=10; break;
                    case 2: utype = "Mummy"; mscore = seed+1; tscore=8; break;
                    case 3: utype = "Ghoul"; cscore = seed+3; tscore=9; break;
                    default: utype = "Bunny"; break;
                }
                switch (r.Next(6)+((seed-4)/2)*2) {
                    case 0: adj = "Crawling"; sscore+=3; cscore+=2; tscore-=2; break;
                    case 1: adj = "Slime"; hscore+=3;  cscore++;break;
                    case 2: adj = "Blood"; mscore+=3; hscore++; break;
                    case 3: adj = "Fire"; cscore+=3; mscore++; break;
                    case 4: adj = "Hungry"; hscore+=3; cscore++; tscore++; break;
                    case 5: adj = "Ice"; mscore+=3; cscore++; break;
                    case 6: adj = "Silent"; sscore+=3; cscore++; break;
                    case 7: adj = "Shrieking"; cscore+=3; mscore++; tscore++; break;
                    default: adj = "Silly"; break;
                }
                //Well this explains it.  I really changed stuff around unnecessarily didn't I. 
                //Gonna comment this out.  It's been retooled to give more modularity anyway
                //double factor = ((seed-4)/2)*.5 + 1;
                //H.pl(cscore+ " " + mscore + " " + sscore + " " + hscore + " " + factor);
                //int ct = (int)(factor * cscore);
                //int mt = (int)(factor * mscore);
                //int st = (int)(factor * sscore);
                //cscore = ct;
                //mscore = mt;
                //sscore = st;
                //H.pl(cscore + " " + mscore + " " + sscore + " " + hscore);
            }
        }
        
        public Soldier GenerateSoldier()
        {
            Random r = new Random();
            int tp = (int)(pscore * (.9 + r.NextDouble()*.1));
            int tc = cscore + r.Next(cscore/3);
            int tm = mscore + r.Next(mscore/3);
            int ts = sscore + r.Next(sscore/3);
            int th = hscore + r.Next(hscore/3);
            int tt = tscore + r.Next(tscore/5);
            Soldier s = new Soldier()
            {
                PowerLevel = tp,
                ClanName = adj,
                GivenName = utype,
                Type = SoldierType.LesserUndead,
                ComBase = tc,
                MagBase = tm,
                SubBase = ts,
                HPBase = th,
                TravelBase = tt,
                TypeID = this.ID
            };
            s.HPCurrent = s.HPMax;
            s.CalcLimit();
            return s;
        }
    }
}