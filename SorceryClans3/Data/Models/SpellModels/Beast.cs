namespace SorceryClans3.Data.Models
{
    public class Beast
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string BeastAdj { get; set; }
        public string BeastName { get; set; }
        public string FullName { get { return BeastAdj + " " + BeastName; } }
        public string ToolName { get; set; }
        public bool AvailableForHarvest { get { return Ecoable && Harvest == null && NumTamed > r.Next(_seed) + 5; } }//tmp for testing
        private bool Ecoable { get; set; }
        public Guid? Harvest { get; set; } = null;
        public int NumTamed { get; set; } = 0;
        private BeastEco Eco { get; set; }
        public PowerTemplate? TamePower { get; set; } = null;
        public StatBlock MinReqs { get; set; }
        public int pseed;
        public int cseed;
        public int mseed;
        public int sseed;
        public int hseed;
        public int? dseed;
        public int ride = 0;
        public int kseed; //healing beasts?? exciting...
        private int _seed;
        Random r = new Random();
        private HunterMission? TameMission { get; set; }
        private bool coinFlip()
        {
            return r.Next(2) == 0;
        }
        public Beast(int seed)
        {
            _seed = seed;
            int brute = 0, mystic = 0, mag = 0, stealth = 0, tough = 0, fight = 0, legend = 0, eco = 0, heal = 0;
            int? travel = null;
            bool hoofed = false;
            string bname = "", aname = "", lname = "";
            int minc, minm, mins;
            switch (r.Next(10))
            {
                case 0: bname = "Blue-"; stealth++; break;
                case 1: bname = "Red-"; brute++; fight++; break;
                case 2: bname = "Green-"; stealth++; break;
                case 3: bname = "Violet-"; mystic++; heal++; break;
                case 4: bname = "Golden-"; mag++; mystic++; break;
                case 5: bname = "Silver-"; mag++; break;
                case 6: bname = "Black-"; stealth++; break;
                case 7: bname = "Brown-"; tough += 2; break;
                case 8: bname = "Gray-"; fight++; break;
                default: bname = "White-"; fight++; break;
            }
            if (seed <= 15) //normal animal
            {
                int bseed = (int)(r.NextDouble() * (seed * 2 + 2) + seed / 3);
                switch (bseed)
                { //2020 making some tweaks, needs more beasts
                    case 0: case 1: aname = "Cat"; fight++; stealth += 2; if (r.NextDouble() < .8) eco = 2; else eco = 9; break;
                    case 2: if (coinFlip()) aname = "Raven"; else aname = "Crow"; mag += 2; stealth++; mystic++; eco = 5; break;
                    case 3: if (coinFlip()) aname = "Goat"; else aname = "Ibex"; fight += 2; tough++; hoofed = true; ride++; eco = 0; travel = 2; break;
                    case 4: if (coinFlip()) aname = "Raccoon"; else aname = "Skunk"; fight++; mag += 2; eco = 1; break;
                    case 5: if (coinFlip()) { aname = "Iguana"; eco = 7; } else { aname = "Bat"; eco = 5; } stealth += 2; mag++; break;
                    case 6: if (coinFlip()) aname = "Horse"; else aname = "Tapir"; brute++; fight += 2; tough++; hoofed = true; ride++; eco = 0; travel = 4; break;
                    case 7: aname = "Fox"; fight++; mag++; stealth++; eco = 2; break;
                    case 8: if (coinFlip()) aname = "Falcon"; else aname = "Hawk"; mystic++; stealth++; fight++; mag++; eco = 6; break;
                    case 9: if (coinFlip()) aname = "Monkey"; else aname = "Lemur"; stealth += 2; fight++; eco = 7; break;
                    case 10: if (coinFlip()) { aname = "Weasel"; eco = 2; } else { aname = "Otter"; eco = 8; } fight++; tough++; stealth++; break;
                    case 11: if (coinFlip()) aname = "Ostrich"; else aname = "Emu"; stealth++; fight++; mag++; mystic++; eco = 6; ride++; travel = 2; break;
                    case 12: aname = "Wolf"; fight += 2; stealth++; eco = 3; travel = 4; break;
                    case 13: if (coinFlip()) aname = "Badger"; else aname = "Wolverine"; mag++; stealth++; tough++; eco = 2; travel = 0; break;
                    case 14: if (coinFlip()) { aname = "Osprey"; eco = 9; } else { aname = "Eagle"; eco = 2; } fight += 2; stealth++; mystic++; break;
                    case 15: aname = "Hyena"; fight += 3; eco = 3; travel = 2; break;
                    case 16: if (coinFlip()) aname = "Crocodile"; else aname = "Alligator"; brute++; fight += 2; tough++; eco = 8; travel = 0; break;
                    case 17: aname = "Panther"; fight++; mag++; stealth++; eco = 3; travel = 1; break;
                    case 18: aname = "Cheetah"; fight++; stealth += 2; eco = 3; travel = 2; break;
                    case 19: if (coinFlip()) aname = "Viper"; else aname = "Cobra"; mag += 2; stealth++; mystic++; eco = 2; break;
                    case 20: aname = "Bear"; brute += 2; fight += 2; tough++; if (r.NextDouble() < .8) eco = 4; else eco = 9; travel = 1; break;
                    case 21: aname = "Jaguar"; fight++; stealth += 2; mystic++; if (coinFlip()) eco = 3; else eco = 9; travel = 1; break;
                    case 22: aname = "Lion"; fight += 2; stealth++; eco = 3; travel = 2; ride++; break;
                    case 23: aname = "Owl"; fight++; mag++; stealth++; eco = 2; mystic += 2; break;
                    case 24: if (coinFlip()) aname = "Rhino"; else aname = "Bison"; brute += 2; fight++; tough += 2; hoofed = true; ride += 2; eco = 0; travel = 2; break;
                    case 25: aname = "Tiger"; fight += 2; stealth++; tough++; eco = 3; travel = 2; ride++; break;
                    case 26: aname = "Elephant"; fight++; tough += 3; mag++; hoofed = true; ride += 2; eco = 0; travel = 3; break;
                    case 27: aname = "Mammoth"; fight++; mag++; tough += 3; hoofed = true; ride += 2; eco = 0; travel = 3; break;
                    default: aname = "Dire-wolf"; fight++; mag++; stealth++; eco = 3; travel = 2; ride++; break;
                }
            }
            else if (seed < 24)
            {
                eco = 3;
                legend++;
                switch (r.Next(10))
                {
                    case 0: aname = "Chimera"; fight += 3; ride++; eco = 4; travel = 2; break;
                    case 1: aname = "Owlbear"; fight++; mag += 2; eco = 4; travel = 1; break;
                    case 2: aname = "Raptor"; fight++; stealth += 2; tough++; travel = 2; break;
                    case 3: aname = "Wyvern"; stealth += 3; ride++; tough++; travel = 4; break;
                    case 4: aname = "Pegasus"; mag += 3; ride += 2; eco = 0; hoofed = true; travel = 4; break;
                    case 5: aname = "Roc"; fight += 2; stealth++; ride += 2; tough++; travel = 4; break;
                    case 6: aname = "Gryphon"; fight++; stealth++; mag++; ride++; travel = 4; break;
                    case 7: aname = "Manticore"; fight += 2; mag++; travel = 3; break;
                    case 8: aname = "Cockatrice"; mag++; stealth += 2; eco = 6; travel = 1; break;
                    default: aname = "Peryton"; mag += 2; stealth++; eco = 0; travel = 3; break;
                }
            }
            else if ((seed - 24) / 5.0 < r.NextDouble()) //greater
            {
                legend += 2;
                switch (r.Next(6))
                {
                    case 0: aname = "Basilisk"; fight++; stealth += 2; eco = 6; travel = 1; break;
                    case 1: aname = "Kitsune"; stealth += 2; mag++; eco = 2; travel = 1; break;
                    case 2: aname = "Unicorn"; mag += 2; fight++; hoofed = true; ride++; eco = 0; travel = 4; break;
                    case 3: aname = "Thunderbird"; fight += 2; mag++; tough++; legend++; ride++; eco = 3; travel = 4; break;
                    case 4: aname = "Saber-tooth"; fight += 2; stealth++; tough += 2; eco = 3; travel = 2; ride++; break;
                    default: aname = "Phoenix"; mag += 2; tough++; stealth++; legend++; eco = 6; travel = 4; break;
                }
            }
            else //dragon
            {
                eco = 3;
                legend += 3;
                aname = "Dragon";
                fight++; mag++; stealth++; ride += 2; travel = 4;
            }
            switch (r.Next(14))
            {
                case 0: bname = bname + "tufted"; stealth++; break;
                case 1: bname = bname + "tailed"; fight++; break;
                case 2: bname = bname + "faced"; mag++; break;
                case 3: bname = bname + "eyed"; mystic++; stealth++; break;
                case 4: bname = bname + "spotted"; stealth++; break;
                case 5: bname = bname + "winged"; mystic++; mag++; break;
                case 6: bname = bname + "crested"; heal++; break;
                case 7: bname = bname + "backed"; fight++; brute++; break;
                case 8: bname = bname + "chested"; tough += 2; break;
                case 9: bname = bname + "striped"; fight++; brute++; break;
                case 10: bname = bname + "streaked"; if (travel != null) travel++; else fight++; break;
                case 11: bname = bname + "plumed"; mag++; break;
                default:
                    if (hoofed) bname = bname + "hoofed"; else bname = bname + "clawed";
                    fight += 2; break;
            }
            switch (r.Next(4) + ride)
            {
                case 0: lname = "trap"; break;
                case 1: lname = "collar"; break;
                case 2: lname = "harness"; break;
                case 3: lname = "lasso"; break;
                case 4: lname = "bridle"; break;
                default: lname = "saddle"; break;
            }
            //RANDOMIZER
            if (legend < 2)
            {
                switch (r.Next(4))
                {
                    case 0: mag++; break;
                    case 1: stealth++; break;
                    case 2: if (travel != null) travel++; else heal++; break;
                    default: fight++; break;
                }
            }
            Eco = (BeastEco)eco;
            if (true)//(r.NextDouble() < ((seed+2)/15.0) && r.NextDouble() > .2) //TESTING
            {
                TamePower = new PowerTemplate(ID, seed / 10 + 1, MagicColor.Green)
                {
                    PowerName = aname + " Mastery",
                    Heritability = null,
                    Beast = this
                };
            }
            cseed = 2 + fight * 3 + legend * 2;
            mseed = 0 + mag * 3 + legend * 2;
            sseed = 2 + stealth * 3 + legend * 2;
            hseed = 4 + seed / 5 + brute + 3 * tough + legend * 2;
            kseed = heal == 0 ? 0 : 4 + 2 * kseed;
            dseed = travel == null ? null : 8 + travel * 2;
            //mystic mods to brute
            //H.p("B:" + brute + " M:" + mystic + " ");
            if (brute > mystic)
            {
                brute -= mystic;
                mystic = 0;
            }
            else
            {
                mystic -= brute;
                brute = 0;
            }
            //randomizer+specializer
            if (brute == 0 && mag >= 3)// && legend < 2)
            {
                int adj1 = r.Next(cseed);
                if (adj1 > 3) adj1 = 3;
                cseed -= adj1; mseed += adj1;
                //H.pl("Magic adjustment of " + adj1 + " on " + aname);
                adj1 = r.Next(sseed);
                if (adj1 > 3) adj1 = 3;
                sseed -= adj1; mseed += adj1;
                //H.pl("Magic adjustment of " + adj1 + " on " + aname);
            }
            if (brute == 0 && stealth >= 3)// && legend < 2)
            {
                int adj1 = r.Next(cseed);
                if (adj1 > 3) adj1 = 3;
                cseed -= adj1; sseed += adj1;
                //H.pl("Stealth adjustment of " + adj1 + " on " + aname);
                adj1 = r.Next(mseed);
                if (adj1 > 3) adj1 = 3;
                mseed -= adj1; sseed += adj1;
                //H.pl("Stealth adjustment of " + adj1 + " on " + aname);
            }
            //brute mods
            brute = brute * 2;
            while (brute > 0)
            {
                int adj1 = r.Next(mseed); if (adj1 > 3) adj1 = 3;
                int adj2 = r.Next(sseed); if (adj2 > 3) adj2 = 3;
                int adj3 = r.Next(kseed); if (adj3 > 3) adj3 = 3;
                switch (r.Next(3))
                {
                    case 0: cseed += adj1; break;
                    case 1: hseed += adj2; break;
                    default: if (dseed != null) dseed += adj3; break;
                }
                switch (r.Next(3))
                {
                    case 0: mseed -= adj1; break;
                    case 1: sseed -= adj2; break;
                    default: kseed -= adj3; break;
                }
                brute--;
            }
            //mystic mods
            mystic = mystic * 2;
            while (mystic > 0)
            {
                int adj1 = r.Next(cseed); if (adj1 > 3) adj1 = 3;
                int adj2 = r.Next(hseed); if (adj2 > 3) adj2 = 3;
                int adj3 = r.Next(dseed ?? 0); if (adj3 > 3) adj3 = 3;
                switch (r.Next(3))
                {
                    case 0: cseed -= adj1; break;
                    case 1: hseed -= adj2; break;
                    default: if (dseed != null) dseed -= adj3; break;
                }
                switch (r.Next(3))
                {
                    case 0: mseed += adj1; break;
                    case 1: sseed += adj2; break;
                    default: kseed += adj3; break;
                }
                mystic--;
            }
            if (hseed < 4) hseed = 4;
            if (cseed > (seed + 8 + 5 * legend)) cseed = seed + 8 + 5 * legend;
            if (mseed > (seed + 8 + 5 * legend)) mseed = seed + 8 + 5 * legend;
            if (sseed > (seed + 8 + 5 * legend)) sseed = seed + 8 + 5 * legend;
            if (cseed > 25 + 2 * legend) cseed = 25 + 2 * legend;
            if (mseed > 25 + 2 * legend) mseed = 25 + 2 * legend;
            if (sseed > 25 + 2 * legend) sseed = 25 + 2 * legend;
            if (legend >= 2 && mseed < 10) //legendary creatures with little magic should be über-mighty
            {
                if (coinFlip()) cseed += 4 + r.Next(3);
                else sseed += 4 + r.Next(3);
            }
            if (cseed < 0) cseed = 0;
            if (mseed < 0) mseed = 0;
            if (sseed < 0) sseed = 0;
            minc = r.Next(seed / 4) + r.Next(seed / 4) + cseed / 2;
            minm = r.Next(seed / 4) + r.Next(seed / 4) + mseed / 2;
            mins = r.Next(seed / 4) + r.Next(seed / 4) + sseed / 2;
            if (cseed < 2) minc = 0;
            if (mseed < 2) minm = 0;
            if (sseed < 2) mins = 0;
            if (minc > cseed) minc = cseed;
            if (minm > mseed) minm = mseed;
            if (mins > sseed) mins = sseed;
            if (minc > seed + 2) minc = seed + 2;
            if (minm > seed + 2) minm = seed + 2;
            if (mins > seed + 2) mins = seed + 2;
            if (minc > 12) minc = 11 + r.Next(3);
            if (minm > 12) minm = 11 + r.Next(3);
            if (mins > 12) mins = 11 + r.Next(3);
            if (TamePower != null)
            {
                minc += TamePower.CBonus; if (minc < 8) minc += r.Next(3);
                minm += TamePower.MBonus; if (minm < 8) minm += r.Next(3);
                mins += TamePower.SBonus; if (mins < 8) mins += r.Next(3);
            }
            pseed = seed * 500 + r.Next(3) * 100;
            Ecoable = coinFlip();
            MinReqs = new StatBlock(minc, minm, mins, null, null, pseed, null, null, null, null);
            BeastName = aname;
            BeastAdj = bname;
            ToolName = lname;
            /*TameMission = new HunterMission(pseed * 30, true, true)
            {
                Type = MissionType.TameBeast
            };*/
        }
        public BeastHarvest? CreateHarvest(int seed)
        {
            if (!Ecoable || Harvest != null)
                return null;
            BeastHarvest harvest = new BeastHarvest(seed, Eco, this);
            this.Harvest = harvest.ID;
            return harvest;
        }
        public bool IsEligible(Soldier soldier)
        {
            if (soldier.Type != SoldierType.Standard)
                return false;
            if (!MinReqs.IsAbove(soldier))
                return false;
            if (soldier.SubSoldiers.Any(e => e.Type == SoldierType.Beast))
                return false;
            if (TamePower == null)
                return true;
            if (soldier.Power == null)
                return false;
            return soldier.Power.ID == TamePower.ID;
        }
        public Mission GetMission
        {
            get
            {
                return TameMission;
            }
        }
        public HunterMission StartMission()
        {
            HunterMission ret = TameMission;
            /*TameMission = new HunterMission(pseed * 30, true, true)
            {
                Type = MissionType.TameBeast
            };*/
            return ret;
        }
        public Soldier GenerateBeast(string? soldname = null)
        {
            Soldier ret = new Soldier()
            {
                ClanName = soldname == null ? BeastAdj : soldname + "'s",
                GivenName = BeastName,
                PowerLevel = r.Next(pseed, (int)(1.2 * pseed)),
                ComBase = cseed + r.Next(3),
                MagBase = mseed + r.Next(3),
                SubBase = sseed + r.Next(3),
                HPBase = hseed + r.Next(5),
                //Medical = //come back to this
                Power = GenPower(mseed),
                TravelBase = dseed,
                Type = SoldierType.Beast,
                MountCountBase = ride,
                TypeID = this.ID
            };
            if (soldname != null) //taming event
                NumTamed++;
            ret.HPCurrent = ret.HPMax;
            ret.CalcLimit();
            return ret;
        }
        private Power? GenPower(int mscore)
        {
            if (mscore < 8)
                return null;
            int mp = 0;
            if (mscore < 10)
                mp = 1;
            else if (mscore < 12)
                mp = 2;
            else if (mscore < 16)
                mp = 3;
            else
                mp = 4;
            return new Power()
            {
                MBonusMax = mp,
                Mastery = 0.1,
                Color = MagicColor.Green,
                MinPowerForColor = 1000,
                PowerIncrementForColor = 1000,
                MaxColors = mp
            };
        }
    }
    public enum BeastEco
    {
        //for its ecological role, 0=herbivore, 1=omni, 2=carn, 3=hypercarnivore, 4==hypercarn/omn,5==insect,6==insect/carn, 7=invert/plant, 8=aquatic-any, 9=fish
        Herbivore,
        Omnivore,
        Carnivore,
        Hypercarnivore,
        HyperOmnivore,
        Insect,
        InsectCarnivore,
        InsectHerbivore,
        AquaticAny,
        AquaticFish
    }
    public enum BeastIcon
    {
        Lion,
        Tiger,
        Bear,
        Badger
        //to do more, lots more...
    }
    public static class BeastUtils
    {
        public static string BeastImg(this BeastIcon icon)
        {
            switch (icon)
            {
                case BeastIcon.Lion: return "<svg viewBox=\"0 0 64 64\" <path d=\"M40.504 23.025c-1.332 0-2.404 1.338-2.404 2.998c0 1.664 1.072 3.002 2.404 3.002c1.324 0 2.396-1.338 2.396-3.002c0-1.66-1.072-2.998-2.396-2.998\" ></path><path d=\"M23.497 23.025c-1.323 0-2.397 1.337-2.397 2.998c0 1.664 1.074 3.002 2.397 3.002c1.331 0 2.403-1.338 2.403-3.002c0-1.661-1.072-2.998-2.403-2.998\"></path><path d=\"M62 14.793c-1.41-.063-2.295-.626-3.021-1.491c-.143-2.924-1.181-5.745-2.689-6.97c-.847-.688-2.041-1.036-3.549-1.036c-.121 0-.246.009-.369.013c-4.245-2.274-8.68-.098-8.68-.098c.29-1.553-.44-2.385.109-3.211C35.57 2 32.004 5.462 32 5.465C31.998 5.462 28.432 2 20.201 2c.549.826-.182 1.658.109 3.211c0 0-4.435-2.177-8.68.098c-.123-.004-.249-.013-.371-.013c-1.507 0-2.701.349-3.548 1.036c-1.509 1.225-2.547 4.046-2.69 6.97c-.725.865-1.611 1.428-3.021 1.491c0 0 .124 3.829 3.991 6.383c0 0-4.056 3.867-3.375 12.084c0 0 1.636-1.996 5.462-2.748c-4.506 3.78-1.878 9.375-5.214 13.238c0 0 4.59 1.008 6.451-.337c-.729 5.923 2.873 8.233 2.873 11.132c4.087-1.344 4.659-5.377 4.659-5.377c0 7.468 2.584 7.203 2.584 11.173c0 0 4.533-.714 6.395-4.452c.74 3.525 4.25 3.906 6.174 6.111h.002c1.924-2.205 5.434-2.586 6.174-6.111c1.861 3.738 6.395 4.452 6.395 4.452c0-3.97 2.584-3.705 2.584-11.173c0 0 .572 4.033 4.658 5.377c0-2.898 3.602-5.209 2.873-11.132c1.861 1.345 6.451.337 6.451.337c-3.336-3.863-.707-9.458-5.213-13.238c3.826.752 5.461 2.748 5.461 2.748c.682-8.217-3.375-12.084-3.375-12.084c3.867-2.554 3.99-6.383 3.99-6.383M10.509 11.31c-1.352 1.101-2.155 5.724.625 7.715a33.13 33.13 0 0 0-.953 1.855c-2.183-1.488-2.902-3.831-3.107-5.633c-.364-3.187.688-6.379 1.899-7.362c.474-.385 1.265-.589 2.287-.589c2.709 0 6.188 1.404 8.303 3.237c-1.08.502-2.1 1.155-3.07 1.927c-1.853-1.392-4.822-2.092-5.984-1.15m25.336 41.794c.093-1.29-.261-2.348-.261-2.348c-.316 1.918-1.172 3.289-1.172 3.289c.037-1.645-.801-2.35-.801-2.35c-.781 2.23-1.598 3.096-1.598 3.096s-.815-.865-1.597-3.096c0 0-.837.705-.8 2.35c0 0-.856-1.371-1.172-3.289c0 0-.354 1.058-.262 2.348c0 0-6.492-4.197-7.51-9.869c2.084 2.341 6.661 6.132 11.33 1.223c4.688 4.928 9.283 1.088 11.355-1.25c-.999 5.686-7.512 9.896-7.512 9.896m-5.517-18.37c-.504-.68-1.025-1.382-1.622-1.822c-.309-.229-.575-.42-.81-.584c.935-.388 2.84-1.053 4.116-1.053c1.274 0 3.272.586 4.284.923c-.277.191-.602.421-.998.714c-.598.441-1.113 1.146-1.611 1.828c-.561.766-1.195 1.634-1.67 1.634h-.002c-.474-.003-1.119-.873-1.687-1.64m13.668 7.191l-.021.067l-.039.059c-.021.032-.16.232-.392.515c-1.942 1.406-6.552 3.867-10.544.14v-5.869l3.82-1.615c.582-.268 1.059-.996 1.059-1.614l-.004-1.728s-2.06-1.604-5.862-1.604s-5.874 1.604-5.874 1.604l-.01 1.727c0 .618.478 1.347 1.059 1.614L31 36.833v5.879c-3.959 3.689-8.522 1.297-10.488-.113a8.203 8.203 0 0 1-.419-.548l-.039-.059l-.021-.067c-.183-.57-.398-1.702-.233-2.81c.162-1.076.678-2.085.948-2.567c.347-.62 5.38-4.659 5.392-4.668c1.627-2.867 1.826-3.148 3.228-3.761c.591-.257 1.614-.386 2.638-.386s2.045.129 2.637.386c1.401.612 1.606.893 3.234 3.76c0 0 5.059 4.048 5.406 4.669c.27.482.787 1.493.947 2.567c.165 1.108-.051 2.239-.234 2.81m4.936-13.273c-.462 2.172-3.547 4.354-4.745 7.486c-.012-.023-.021-.057-.032-.079c-.291-.519-2.146-2.157-5.519-4.87c-1.565-2.751-1.913-3.252-3.595-3.986c-.974-.425-2.449-.47-3.037-.47c-.59 0-2.064.045-3.037.469c-1.689.738-2.033 1.239-3.612 4.018c-3.348 2.695-5.191 4.323-5.48 4.84c-.013.022-.021.058-.033.082c-1.204-3.133-4.311-5.316-4.771-7.489c-.47-2.214 1.551-7.256 3.454-8.321c.341-.189 1.545 0 1.545 0c.356-.476 2.285-6.041 7.318-6.959c2.988-.546 4.625 2.008 4.625 2.008s1.615-2.554 4.604-2.008c5.031.918 6.961 6.483 7.318 6.959c0 0 1.203-.189 1.545 0c1.901 1.064 3.922 6.106 3.452 8.32m7.996-13.404c-.205 1.802-.924 4.144-3.107 5.632a33.11 33.11 0 0 0-.953-1.855c2.781-1.991 1.977-6.614.625-7.715c-1.162-.941-4.131-.241-5.982 1.15c-.971-.771-1.99-1.425-3.07-1.927c2.113-1.833 5.592-3.237 8.301-3.237c1.023 0 1.814.204 2.289.589c1.209.983 2.262 4.177 1.897 7.363\"></path></svg>";
                case BeastIcon.Tiger: return "<svg viewBox=\"0 0 512 512\" ><path d=\"M104.365,252.937c0.89-7.979,5.322-11.527,3.548-32.806c-1.774-21.287-15.958-19.506-13.301-8.87 c2.665,10.636-5.322,50.538-5.322,71.824c0,21.279,15.965,31.04,36.362,27.492c20.396-3.547,31.923-15.965,20.396-15.965 C107.913,301.708,101.813,275.915,104.365,252.937z\"></path> <path d=\"M511.671,257.964c-10.644-13.006-31.47-35.206-40.196-54.395c-5.911-12.999-7.972-13.965-24.857-47.684 c-1.608-3.208-7.179-15.701-0.861-22.653c44.34-48.772,20.011-121.592,0.891-123.253c-56.75-2.665-73.599,27.484-93.112,44.332 c-19.506,15.965-56.75,1.774-97.535,1.774c-40.792,0-78.037,14.191-97.542-1.774C138.952,37.463,122.104,7.314,65.354,9.979 c-19.12,1.66-43.45,74.481,0.884,123.253c6.326,6.952,0.754,19.445-0.854,22.653c-16.893,33.719-18.946,34.686-24.857,47.684 c-8.726,19.188-29.56,41.389-40.196,54.395c-3.774,4.605,26.005,17.731,26.005,17.731S2.693,317.077,6.241,341.904 c15.957-3.548,24.827-7.096,24.827-7.096s0.294,47.299,11.821,61.483c10.643-5.91,27.197-15.377,27.197-15.377 s-4.733,16.562,2.363,44.929c20.094-10.635,29.56-11.821,29.56-11.821s8.568,42.567,23.642,49.655 c9.752-7.088,27.19-9.451,27.19-9.451s3.691,2.341,10.009,4.892c5.881,9.549,15.173,21.317,34.323,30.784 c0.574,0.287,51.587,13.074,58.524,11.156c0,0,23.054,6.371,58.524-11.156c18.94-9.361,28.239-20.97,34.128-30.467 c6.816-2.672,10.802-5.208,10.802-5.208s17.445,2.363,27.198,9.451c15.075-7.088,23.642-49.655,23.642-49.655 s9.458,1.186,29.56,11.821c7.096-28.367,2.363-44.929,2.363-44.929s16.554,9.466,27.189,15.377 c11.534-14.184,11.829-61.483,11.829-61.483s8.862,3.548,24.827,7.096c3.548-24.827-20.102-66.209-20.102-66.209 S515.437,262.569,511.671,257.964z M442.208,36.58c21.264,13.814,15.966,59.407-3.547,81.57c-6.206,7.096-17.732,6.212-22.163,0.89 c-4.439-5.314-39.449-35.568-38.128-44.34C381.027,56.969,424.477,25.046,442.208,36.58z M69.785,36.58 c17.739-11.534,61.189,20.389,63.846,38.12c1.314,8.772-33.696,39.026-38.128,44.34c-4.438,5.322-15.965,6.206-22.17-0.89 C53.827,95.987,48.521,50.394,69.785,36.58z M255.698,467.225c0,0-22.751,15.595-58.056-1.721 c17.332-0.468,37.833-6.598,58.063-25.763h0.589c20.018,18.969,40.301,25.159,57.512,25.748 C278.472,482.836,255.698,467.225,255.698,467.225z M443.982,341.61c-11.262,37.992-67.386-16.849-83.351-18.623 c-0.883,27.492,20.396,26.601,30.149,42.559c9.753,15.965-31.032,54.983-38.128,61.189c-7.096,6.204-23.053,13.889-39.902,13.889 s-42.468-9.678-42.468-17.732c0-3.933,0.558-11.979,1.14-19.204c18.245-4.581,36.369-19.12,47.631-29.673 c7.066-6.62,7.142-31.84,7.142-31.84l-9.014-57.354c0,0-5.321-16.75,15.445-19.309c36.354-4.484,48.401-23.416,48.401-23.416 s23.921-28.473-9.036-28.473c-32.957,0-78.143,4.929-76.09,24.291c3.178,29.998,3.095,30.014,9.851,108.881h-25.356h-48.794H206.24 c6.756-78.867,6.681-78.882,9.859-108.881c2.045-19.362-43.133-24.291-76.097-24.291c-32.958,0-9.028,28.473-9.028,28.473 s12.04,18.932,48.393,23.416c20.767,2.558,15.445,19.309,15.445,19.309l-9.006,57.354c0,0,0.076,25.22,7.134,31.84 c11.262,10.552,29.387,25.092,47.639,29.673c0.574,7.224,1.132,15.271,1.132,19.204c0,8.054-25.612,17.732-42.461,17.732 c-16.848,0-32.813-7.685-39.902-13.889c-7.096-6.206-47.888-45.224-38.135-61.189c9.76-15.958,31.04-15.068,30.149-42.559 c-15.958,1.774-72.081,56.614-83.351,18.623C55.3,298.756,59.149,196.187,97.277,154.511c26.752-29.251,46.56-57.165,89.382-71.228 c1.532-0.393,3.827-1.05,7.005-2c1.404-0.431,2.756-0.816,4.069-1.193c7.367-1.834,15.361-3.307,24.11-4.364 c17.18-0.392,15.376,9.406,15.376,12.637c0,4.438-3.147,6.559-17.573,10.281c-47.888,10.643-73.598,41.682-79.811,54.983 c-7.202,15.429,3.306,22.752,13.95,7.677c10.635-15.075,32.421-31.228,50.107-38.045c18.366-7.088,24.404-9.186,33.326-7.088 c2.59,0.619,7.345,4.922,7.345,10.236c0,5.321-6.559,8.666-12.591,12.591c-26.503,13.648-31.84,28.812-33.614,34.134 c-1.774,5.314,5.405,11.066,8.945,5.752c13.12-23.876,38.943-30.586,38.943-25.258c0,5.314,0,26.602,0,36.354 c0,9.752,0.883,21.279,9.753,21.279c8.862,0,9.752-11.527,9.752-21.279c0-9.753,0-31.04,0-36.354 c0-5.329,25.816,1.382,38.936,25.258c3.548,5.314,10.719-0.438,8.946-5.752c-1.774-5.322-7.104-20.487-33.607-34.134 c-6.031-3.925-12.598-7.27-12.598-12.591c0-5.314,4.755-9.617,7.352-10.236c8.915-2.098,14.954,0,33.319,7.088 c17.686,6.817,39.472,22.97,50.115,38.045c10.636,15.074,21.152,7.752,13.942-7.677c-6.205-13.301-31.922-44.34-79.803-54.983 c-14.425-3.722-17.573-5.843-17.573-10.281c0-3.231-1.804-13.029,15.376-12.637c8.749,1.057,16.736,2.529,24.103,4.364 c1.313,0.377,2.672,0.762,4.069,1.193c3.178,0.951,5.473,1.608,7.012,2c42.816,14.063,62.623,41.977,89.383,71.228 C452.852,196.187,456.694,298.756,443.982,341.61z\"></path> <path d=\"M404.08,220.131c-1.774,21.279,2.665,24.827,3.548,32.806c2.552,22.978-3.548,48.772-41.676,41.676 c-11.526,0,0,12.418,20.396,15.965c20.396,3.548,36.354-6.212,36.354-27.492c0-21.287-7.979-61.188-5.322-71.824 C420.046,200.625,405.854,198.844,404.08,220.131z\"></path></svg>";
                case BeastIcon.Bear: return "<svg viewBox=\"0 0 400 400\" ><path d=\"M122.587 174.541C96.186 141.452 136.886 122.943 146.727 162.305\"></path> <path d=\"M249.239 140.48C265.712 119.094 289.684 147.75 270.403 164.62\"></path> <path d=\"M187.07 235.717C178.667 251.304 185.107 298.322 210.879 292.595C233.547 287.557 229.785 254.969 227.413 238.363\"></path> <path d=\"M198.666 278.4C205.668 275.301 210.55 274.053 221.131 278.285\"></path> <path d=\"M116.244 169.912C94.7386 235.717 148.382 253.574 155.324 253.574\" ></path> <path d=\"M270.403 164.621C290.642 195.155 277.564 241.351 250.475 250.158\"></path> <path d=\"M148.445 199.264C148.817 275.57 180.967 292.264 201.95 292.264\"></path> <path d=\"M253.677 199.264C254.547 271.283 230.262 280.455 221.131 287.303\"></path> <path d=\"M94.97 359.286C46.2918 56.1862 170.072 34.3782 209.383 35.0571C331.145 37.1599 327.65 314.238 303.782 363.915\" ></path> <path d=\"M168.296 275.57C159.648 300.217 150.372 333.721 146.727 359.286\" ></path> <path d=\"M231.805 275.57C234.87 308.331 240.726 336.244 243.312 363.915\" ></path> <path d=\"M181.405 215.98C180.086 212.735 177.183 211.573 173.118 209.831\" ></path> <path d=\"M113.7 359.65C112.227 358.021 113.698 355.474 113.7 351.521\" ></path> <path d=\"M129.959 361.392C129.96 357.327 129.958 355.474 129.959 351.521\" ></path> <path d=\"M265.841 365.457C265.842 362.554 265.84 361.281 265.841 357.328\" ></path> <path d=\"M282.101 365.457C282.102 363.135 282.1 361.281 282.101 357.328\" ></path> <path d=\"M229.222 219.089C229.222 215.639 230.606 212.735 233.713 211.592\" ></path> <path d=\"M138.779 138.6C164.705 109.187 220.774 108.105 250.475 132.856\" ></path> </svg>";
                case BeastIcon.Badger: return "<svg viewBox=\"0 0 512 512\" ><path d=\"M468.702,169.851c16.707-24.204,26.045-48.263,29.044-74.631c3.806-33.539-10.486-64.032-38.231-81.568 c-29.963-18.937-68.8-18.121-101.35,2.128c-15.107,9.396-29.282,24.029-42.248,43.553c-18.889-3.915-38.891-5.895-59.916-5.895 c-21.025,0-41.028,1.98-59.916,5.895c-12.966-19.523-27.141-34.157-42.247-43.552C121.286-4.47,82.451-5.286,52.486,13.652 C24.74,31.188,10.448,61.68,14.255,95.223c2.997,26.365,12.335,50.424,29.043,74.628C4.621,236.167,1.439,307.409,1.439,328.743 v10.552l9.531,4.529c31.857,15.137,120.607,60.468,148.346,88.083c10.894,10.846,19.549,22.075,27.918,32.934 C205.926,489.094,223.58,512,256,512c32.42,0,50.074-22.907,68.765-47.159c8.369-10.859,17.023-22.088,27.918-32.934 c27.738-27.615,116.49-72.947,148.346-88.083l9.531-4.529v-10.552C510.56,307.409,507.378,236.167,468.702,169.851z M35.068,318.394c1.237-28.403,8.358-89.588,44.517-143.634c10.229-15.29,21.869-28.585,34.86-39.91 c28.232,69.042,52.381,169.113,65.574,270.655C146.367,374.313,63.676,332.427,35.068,318.394z M293.29,450.932 c-14.292,18.21-23.552,27.676-37.289,27.676c-13.739,0-22.997-9.466-37.288-27.676c-0.109-1.156-0.219-2.313-0.331-3.469h75.239 C293.509,448.619,293.397,449.775,293.29,450.932z M331.98,405.506c13.193-101.543,37.343-201.614,65.575-270.656 c12.991,11.325,24.63,24.62,34.86,39.91c36.159,54.046,43.279,115.231,44.517,143.634 C448.323,332.427,365.631,374.313,331.98,405.506z\"></path><path d=\"M382.572,287.579c-11.274,0-20.42,9.214-20.42,20.582s9.145,20.582,20.42,20.582c11.26,0,20.405-9.214,20.405-20.582 S393.832,287.579,382.572,287.579z\"></path> <path d=\"M129.44,287.579c-11.274,0-20.42,9.214-20.42,20.582s9.145,20.582,20.42,20.582c11.26,0,20.405-9.214,20.405-20.582 S140.701,287.579,129.44,287.579z\"></path> </svg>";
                default: return "";
            }
        }
    }
}