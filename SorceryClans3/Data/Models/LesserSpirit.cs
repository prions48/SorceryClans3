namespace SorceryClans3.Data.Models
{
    public class LesserSpirit
    {
        private string EName { get; set; }
        private string SName { get; set; }
        public string GetName { get { return EName + " " + SName; } }
        private int PowerLevel { get; set; }
        private int Com { get; set; }
        private int Mag { get; set; }
        private int Sub { get; set; }
        private int HP { get; set; }
        private int Heal { get; set; }
        private int Tra { get; set; }
        public Soldier GenerateSoldier()
        {
            return new Soldier()
            {
                Type = SoldierType.LesserSpirit,
                ClanName = EName,
                GivenName = SName,
                PowerLevel = PowerLevel,
                ComBase = Com,
                MagBase = Mag,
                SubBase = Sub,
                TravelBase = Tra,
                HPBase = HP,
                HPCurrent = HP,
                Medical = new Medical()
                {
                    HealBase = Heal,
                    MedicalPowerBase = Heal > 10 ? 100 : 10 * Heal,
                    Assessed = true,
                    Trained = Heal > 0
                }
            };
        }
        public LesserSpirit(int rank, bool healer)
        {
            Random r = new Random();
            int seed = rank / 2;
            string ntype, etype;
            int cb=0,mb=0,sb=0,db=0;
            int tough = 1;
            int prim=0,sec=0,tert=0;
            int addseed = (int)(Math.Sqrt(seed+1));
            switch (r.Next(4))
            {
                case 0: /*ctype = "Summoning";*/ cb += addseed/2; mb += addseed/2; sb += addseed/2; break;
                case 1: /*ctype = "Conjuring";*/ mb += addseed; break;
                case 2: /*ctype = "Binding";*/ cb += addseed; tough++; break;
                default: /*ctype = "Creating";*/ sb += addseed; break;
            }
            prim = seed + 5;
            sec = seed;
            tert = seed / 2 + 1;
            if (healer)
            {
                switch (r.Next(5)+rank/3)
                {
                    case 0: case 1: ntype = "Cat"; sb += prim; cb += sec; mb += tert; db = 10; break;
                    case 2: db = 7; ntype = "Turtle"; cb += prim; tough += sec; mb += tert; break;
                    case 3: db = 10; ntype = "Lynx"; sb += prim; cb += sec; mb += tert; break;
                    case 4: db = 16; ntype = "Raven"; mb += prim; sb += sec; cb += tert; break;
                    case 5: db = 16; ntype = "Eagle"; cb += prim; sb += sec; mb += tert; break;
                    case 6: db = 10; ntype = "Naiad"; mb += prim; sb += sec; cb += tert; break;
                    case 7: db = 13; ntype = "Stag"; sb += prim; cb += sec; mb += tert; break;
                    case 8: db = 14; ntype = "Lion"; mb += prim; sb += sec; cb += tert; break;
                    case 9: db = 14; ntype = "Unicorn"; mb += prim; cb += sec; sb += tert; break;
                    case 10: db = 13; ntype = "Guardian"; cb += prim; mb += sec; tough += tert; break;
                    case 11: db = 13; ntype = "Deva"; mb += prim; sb += sec; cb += tert; break;
                    case 12: db = 13; ntype = "Eladrin"; cb += prim; mb += sec; sb += tert; break;
                    default: db = 13; ntype = "Archon"; mb += prim; cb += sec; sb += tert; break;
                }
                switch (r.Next(6))
                {
                    case 0: etype = "Water"; sb += seed; break;
                    case 1: etype = "Wood"; cb += seed; break;
                    case 2: etype = "Life"; mb += seed; break;
                    case 4: etype = "Holy"; cb += seed; break;
                    case 5: etype = "Spirit"; mb += seed - 1; db+=2; break;
                    default: etype = "Energy"; sb += seed - 1; db+=2; break;
                }
            }
            else
            {
                switch (r.Next(8)+rank/3) //2020 added some
                {
                    case 0: db = 15; ntype = "Bird"; sb += prim; cb += sec; sb += tert; break;
                    case 1: db = 13; ntype = "Coyote"; mb += prim+tert; sb += sec; break;
                    case 3: db = 12; ntype = "Ifrit"; mb += prim; sb += sec; cb += tert; break;
                    case 2: db = 12; ntype = "Spider"; sb += prim+tert; mb += sec; tough++; break;
                    case 4: db = 13; ntype = "Hound"; cb += sec; sb += prim; mb += tert; tough++; break;
                    case 5: db = 10; ntype = "Kelpie"; cb += prim; sb += sec; mb += tert; tough++; break;
                    case 6: db = 12; ntype = "Ogre"; cb += prim+tert; mb+=sec; break;
                    case 7: db = 12; ntype = "Golem"; cb += prim; mb += sec; sb += tert; tough+=2; break;
                    case 8: db = 12; ntype = "Mantis"; mb += prim; sb += sec; cb += tert; break;
                    case 9: db = 12; ntype = "Naga"; sb += prim; mb +=sec; cb += tert; break;
                    case 10: db = 13; ntype = "Scorpion"; mb += prim; sb += sec; cb +=tert; break;
                    case 11: db = 14; ntype = "Djinn"; sb += prim; mb += sec; cb += tert; break;
                    case 12: db = 14; ntype = "Giant"; cb += prim; mb += sec; sb += tert; break;
                    case 13: db = 14; ntype = "Elemental"; mb += prim+tert; cb += sec; tough++; break;
                    default: db = 15; ntype = "Dragon"; mb += prim; cb += sec; sb += tert; tough++; break;
                }
                switch (r.Next(15))
                {
                    case 0: etype = "Fire"; cb += seed/2; mb += seed/2; db++; break;
                    case 1: etype = "Shadow"; sb += seed; tough++; break;
                    case 2: etype = "Water"; mb += seed/2; sb += seed/2; break;
                    case 3: etype = "Air"; sb += seed; db++; break;
                    case 4: etype = "Stone"; cb += seed; tough++; break;
                    case 5: etype = "Blood"; cb += seed/2; sb += seed/2; tough++; break;
                    case 6: etype = "Lightning"; mb += seed; break;
                    case 7: etype = "Poison"; mb += seed; tough++; break;
                    case 8: etype = "Crystal"; mb += seed; db++; break;
                    case 9: etype = "Steel"; cb += seed; tough++; break;
                    case 10: etype = "Sand"; cb += seed/2; sb += seed/2; tough++; break;
                    case 11: etype = "Wood"; mb += seed/2; sb += seed/2; db++; break;
                    case 12: etype = "Iron"; cb += seed; db++;break;
                    case 13: etype = "Lava"; mb += seed/2; cb += seed/2; tough++; break;
                    default: etype = "Dust"; sb += seed; db++; break;
                }
            }
            int newhp = 6+seed/4+3*tough;
            int medsk = 0;
            if (healer)
            {
                medsk = 3 + rank/3;
                cb /= 2;
                sb /= 2;
                mb /= 2;
            }
            EName = etype;
            SName = ntype;
            PowerLevel = 500*rank;
            Com = cb;
            Mag = mb;
            Sub = sb;
            Tra = db;
            Heal = medsk;
            HP = newhp;
        }
    }
}