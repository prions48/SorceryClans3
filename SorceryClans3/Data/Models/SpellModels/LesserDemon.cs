namespace SorceryClans3.Data.Models
{
    public class LesserDemon
    {
        private string dtype = "";
        private string dname = "";
        public string DemonName { get { return dtype + " " + dname; } }
        private int power;
        private int cbase;
        private int mbase;
        private int sbase;
        private int hbase;
        private int tbase;
        private int duration;
        private string plural = "";

        public LesserDemon(int seed)
        {
            Random r = new Random();
            int factor = r.Next(4) + 1;
            power = (seed + 1) * 800 + r.Next(40) * 10;
            duration = r.Next(seed + 10) + (5 - factor) * 40;
            cbase = 1;
            mbase = 1;
            sbase = 1;
            hbase = 3;
            tbase = 12;
            //seed runs 1-10 or more
            //adjective can be any
            switch (r.Next(18))
            {
                case 0: dtype = "Slime"; cbase++; sbase++; hbase++; break;
                case 1: dtype = "Vine"; mbase++; tbase++; sbase++; break;
                case 2: dtype = "Oil"; cbase++; mbase += 2; sbase--; hbase++; break;
                case 3: dtype = "Scum"; mbase++; sbase++; hbase++; break;
                case 4: dtype = "Flame"; cbase += 2; mbase += 2; sbase--; break;
                case 5: dtype = "Fanged"; cbase++; hbase += 2; break;
                case 6: dtype = "Iron"; cbase += 2; hbase++; break;
                case 7: dtype = "Clawed"; cbase += 2; mbase--; sbase++; hbase++; break;
                case 8: dtype = "Screaming"; cbase += 3; tbase += 2; sbase--; hbase++; break;
                case 9: dtype = "Swamp"; mbase++; sbase++; hbase++; break;
                case 10: dtype = "Filth"; cbase++; sbase++; hbase++; break;
                case 11: dtype = "Horned"; cbase++; tbase += 2; hbase++; break;
                case 12: dtype = "Stone"; cbase += 2; hbase++; break;
                case 13: dtype = "Smoke"; cbase--; tbase += 2; mbase++; sbase += 2; hbase++; break;
                case 14: dtype = "Mist"; cbase--; mbase += 2; sbase += 2; break;
                case 15: dtype = "Barbed"; cbase++; tbase += 2; hbase++; break;
                case 16: dtype = "Bone"; cbase += 2; mbase--; hbase += 2; break;
                case 17: dtype = "Blood"; mbase += 2; hbase++; break;
            }
            //scales with power
            switch ((int)(r.Next(6) + seed * .7))
            {
                case 0: case 1: dname = "Toad"; cbase++; mbase += 2; break;
                case 2: dname = "Viper"; sbase += 3; break;
                case 3: dname = "Imp"; mbase++; hbase += 2; break;
                case 4: dname = "Bat"; sbase += 2; tbase += 2; break;
                case 5: dname = "Wretch"; cbase += 2; hbase++; plural = "Wretches"; break;
                case 6: dname = "Fiend"; tbase++; cbase++; mbase++; sbase++; break;
                case 7: dname = "Ape"; cbase += 2; sbase++; break;
                case 8: dname = "Spider"; cbase++; sbase++; hbase++; break;
                case 9: dname = "Vulture"; mbase++; sbase++; hbase++; break;
                case 10: dname = "Satyr"; tbase += 2; cbase += 2; hbase++; break;
                case 11: dname = "Fury"; tbase += 2; cbase += 2; mbase++; plural = "Furies"; break;
                case 12: dname = "Felhound"; tbase += 3; cbase++; sbase++; hbase++; break;
                case 13: dname = "Giant"; tbase++; cbase++; hbase += 2; break;
                default:
                    if (r.Next(4) == 0) { dname = "Succubus"; mbase++; sbase += 2; plural = "Succubi"; }
                    else if (r.Next(3) == 0) { dname = "Balrog"; cbase++; mbase++; sbase++; }
                    else if (r.Next(2) == 0) { dname = "Felguard"; cbase++; mbase++; hbase++; }
                    else { dname = "Minotaur"; cbase += 2; hbase++; }
                    tbase++; break;
            }
            if (plural == "") plural = dname + "s";
            cbase = (int)(0.3 * (seed / 2 + 5) * cbase * (1 + factor / 5.0)); if (cbase > 0) cbase += r.Next(2);
            mbase = (int)(0.3 * (seed / 2 + 5) * mbase * (1 + factor / 5.0)); if (mbase > 0) mbase += r.Next(2);
            sbase = (int)(0.3 * (seed / 2 + 5) * sbase * (1 + factor / 5.0)); if (sbase > 0) sbase += r.Next(2);
            hbase = (int)(0.3 * (seed / 2 + 5) * hbase * (1 + factor / 5.0)); if (hbase > 0) hbase += r.Next(2);
        }
        public Soldier GenerateSoldier()
        {
            Random r = new Random();
            int hp = (int)((r.NextDouble() * .1 + .95) * hbase);
            Soldier ret = new Soldier()
            {
                Type = SoldierType.LesserDemon,
                ClanName = dtype,
                GivenName = dname,
                PowerLevel = (int)((r.NextDouble() * .1 + .95) * power),
                ComBase = (int)((r.NextDouble() * .1 + .95) * cbase),
                MagBase = (int)((r.NextDouble() * .1 + .95) * mbase),
                SubBase = (int)((r.NextDouble() * .1 + .95) * sbase),
                TravelBase = (int)((r.NextDouble() * .1 + .95) * tbase),
                HPBase = hp,
                HPCurrent = hp,
                RemainingActive = duration //add randomness?
            };
            return ret;
        }
        public string DisplayWithCount(int ct, bool includenum)
        {
            if (ct == 1)
                return $"{(includenum ? ct + " " : "")}{DemonName}";
            return $"{(includenum ? ct + " " : "")}{dtype + " " + plural}";
        }
    }
}