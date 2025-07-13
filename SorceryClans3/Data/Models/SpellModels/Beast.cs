namespace SorceryClans3.Data.Models
{
    public class Beast
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string BeastAdj { get; set; }
        public string BeastName { get; set; }
        public string FullName { get { return BeastAdj + " " + BeastName; } }
        public string ToolName { get; set; }
        public bool AvailableForHarvest { get { return Ecoable && Harvest == null /*&& NumTamed > 1 r.Next(10) + 5*/; } }//tmp for testing
        private bool Ecoable { get; set; }
        public Guid? Harvest { get; set; } = null;
        public int NumTamed { get; set; } = 1;
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
        Random r = new Random();
        private HunterMission? TameMission { get; set; }
        private bool coinFlip()
        {
            return r.Next(2) == 0;
        }
        public Beast(int seed)
        {
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
                case 7: bname = "Brown-"; tough+=2; break;
                case 8: bname = "Gray-"; fight++; break;
                default: bname = "White-"; fight++; break;
            }
            if (seed <= 15) //normal animal
            {
                int bseed = (int)(r.NextDouble()*(seed*2+2)+seed/3);
                switch (bseed) { //2020 making some tweaks, needs more beasts
                    case 0: case 1: aname = "Cat"; fight++; stealth+=2; if (r.NextDouble() < .8) eco=2; else eco=9; break;
                    case 2: if (coinFlip()) aname = "Raven"; else aname = "Crow"; mag+=2; stealth++; mystic++; eco=5; break;
                    case 3: if (coinFlip()) aname = "Goat"; else aname = "Ibex"; fight+=2; tough++; hoofed = true; ride++; eco=0; travel=2; break;
                    case 4: if (coinFlip()) aname = "Raccoon"; else aname = "Skunk"; fight++; mag+=2; eco=1; break;
                    case 5: if (coinFlip()) { aname = "Iguana"; eco=7; } else { aname = "Bat"; eco=5; } stealth+=2; mag++; break;
                    case 6: if (coinFlip()) aname = "Horse"; else aname = "Tapir"; brute++; fight+=2; tough++; hoofed = true; ride++; eco=0; travel=4; break;
                    case 7: aname = "Fox"; fight++; mag++; stealth++; eco=2; break;
                    case 8: if (coinFlip()) aname = "Falcon"; else aname = "Hawk"; mystic++; stealth++; fight++; mag++; eco=6; break;
                    case 9: if (coinFlip()) aname = "Monkey"; else aname = "Lemur"; stealth+=2; fight++; eco=7; break;
                    case 10: if (coinFlip()) { aname = "Weasel"; eco=2; } else  { aname = "Otter"; eco=8; } fight++; tough++; stealth++; break;
                    case 11: if (coinFlip()) aname = "Ostrich"; else aname = "Emu"; stealth++; fight++; mag++; mystic++; eco=6; ride++; travel=2; break;
                    case 12: aname = "Wolf"; fight+=2; stealth++; eco=3; travel=4; break;
                    case 13: if (coinFlip()) aname = "Badger"; else aname = "Wolverine"; mag++; stealth++; tough++; eco=2; travel=0; break;
                    case 14: if (coinFlip()) { aname = "Osprey"; eco=9; } else { aname = "Eagle"; eco=2; } fight+=2; stealth++; mystic++; break;
                    case 15: aname = "Hyena"; fight+=3; eco=3; travel=2; break;
                    case 16: if (coinFlip()) aname = "Crocodile"; else aname = "Alligator"; brute++; fight+=2; tough++; eco=8; travel=0; break;
                    case 17: aname = "Panther"; fight++; mag++; stealth++; eco=3; travel=1; break;
                    case 18: aname = "Cheetah"; fight++; stealth+=2; eco=3; travel=2; break;
                    case 19: if (coinFlip()) aname = "Viper"; else aname = "Cobra"; mag+=2; stealth++; mystic++; eco=2; break;
                    case 20: aname = "Bear"; brute+=2; fight+=2; tough++; if (r.NextDouble() < .8) eco=4; else eco=9; travel=1; break;
                    case 21: aname = "Jaguar"; fight++; stealth+=2; mystic++; if (coinFlip()) eco=3; else eco=9; travel=1; break;
                    case 22: aname = "Lion"; fight+=2; stealth++; eco=3; travel=2; ride++; break;
                    case 23: aname = "Owl"; fight++; mag++; stealth++; eco=2; mystic+=2; break;
                    case 24: if (coinFlip()) aname = "Rhino"; else aname = "Bison"; brute+=2; fight++; tough+=2; hoofed = true; ride+=2; eco=0; travel=2; break;
                    case 25: aname = "Tiger"; fight+=2; stealth++; tough++; eco=3; travel=2; ride++; break;
                    case 26: aname = "Elephant"; fight++; tough+=3; mag++; hoofed = true; ride+=2; eco=0; travel=3; break;
                    case 27: aname = "Mammoth"; fight++; mag++; tough+=3; hoofed = true; ride+=2; eco=0; travel=3; break;
                    default: aname = "Dire-wolf"; fight++; mag++; stealth++; eco=3; travel=2; ride++; break;
                }
            }
            else if (seed < 24)
            {
                eco=3;
                legend++;
                switch (r.Next(10))
                {
                    case 0:	aname = "Chimera"; fight+=3; ride++; eco=4; travel=2; break;
                    case 1: aname = "Owlbear"; fight++; mag+=2; eco=4; travel=1; break;
                    case 2: aname = "Raptor"; fight++; stealth+=2; tough++; travel=2; break;
                    case 3: aname = "Wyvern"; stealth+=3; ride++; tough++; travel=4; break;
                    case 4: aname = "Pegasus"; mag+=3; ride+=2; eco=0; hoofed = true; travel=4; break;
                    case 5: aname = "Roc"; fight+=2; stealth++; ride+=2; tough++; travel=4; break;
                    case 6: aname = "Gryphon"; fight++; stealth++; mag++; ride++; travel=4; break;
                    case 7: aname = "Manticore"; fight+=2; mag++; travel=3; break;
                    case 8: aname = "Cockatrice"; mag++; stealth+=2; eco=6; travel=1; break;
                    default: aname = "Peryton"; mag+=2; stealth++; eco=0; travel=3; break;
                }
            }
            else if ((seed-24)/5.0 < r.NextDouble()) //greater
            {
                legend+=2;
                switch (r.Next(6))
                {
                    case 0: aname = "Basilisk"; fight++; stealth+=2; eco=6; travel=1; break;
                    case 1: aname = "Kitsune"; stealth+=2; mag++; eco=2; travel=1; break;
                    case 2: aname = "Unicorn"; mag+=2; fight++; hoofed = true; ride++; eco=0; travel=4; break; 
                    case 3: aname = "Thunderbird"; fight+=2; mag++; tough++; legend++; ride++; eco=3; travel=4; break;
                    case 4: aname = "Saber-tooth"; fight+=2; stealth++; tough+=2; eco=3; travel=2; ride++; break;
                    default: aname = "Phoenix"; mag+=2; tough++; stealth++; legend++; eco=6; travel=4; break;
                }
            }
            else //dragon
            {
                eco=3;
                legend+=3;
                aname = "Dragon";
                fight++; mag++; stealth++; ride+=2; travel=4;
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
                case 8: bname = bname + "chested"; tough+=2; break;
                case 9: bname = bname + "striped"; fight++; brute++; break;
                case 10: bname = bname + "streaked"; if (travel != null) travel++; else fight++; break;
                case 11: bname = bname + "plumed"; mag++; break;
                default: if (hoofed) bname = bname + "hoofed"; else bname = bname + "clawed";
                            fight+=2; break;
            }
            switch (r.Next(4)+ride)
            {
                case 0: lname = "trap"; break;
                case 1: lname = "collar"; break;
                case 2: lname = "harness"; break;
                case 3: lname = "lasso"; break;
                case 4: lname = "bridle"; break;
                default: lname = "saddle"; break;
            }
            //RANDOMIZER
            if (legend < 2) {
                switch (r.Next(4))
                {
                    case 0: mag++; break;
                    case 1: stealth++; break;
                    case 2: if (travel != null) travel++; else heal++; break;
                    default: fight++; break;
                }
            }
            Eco = (BeastEco)eco;
            if (r.NextDouble() < ((seed+2)/15.0) && r.NextDouble() > .2)
            {
                TamePower = new PowerTemplate(ID, seed / 10 + 1, MagicColor.Green)
                {
                    PowerName = aname + " Mastery",
                    Heritability = null
                };
            }
            cseed = 2+fight*3+legend*2;
            mseed = 0+mag*3+legend*2;
            sseed = 2+stealth*3+legend*2;
            hseed = 4+seed/5+brute+3*tough+legend*2;
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
                    case 0: cseed+=adj1; break;
                    case 1: hseed+=adj2; break;
                    default: if (dseed != null) dseed+=adj3; break;
                }
                switch (r.Next(3))
                {
                    case 0: mseed-=adj1; break;
                    case 1: sseed-=adj2; break;
                    default: kseed-=adj3; break;
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
                    case 0: cseed-=adj1; break;
                    case 1: hseed-=adj2; break;
                    default: if (dseed != null) dseed-=adj3; break;
                }
                switch (r.Next(3))
                {
                    case 0: mseed+=adj1; break;
                    case 1: sseed+=adj2; break;
                    default: kseed+=adj3; break;
                }
                mystic--;
            }
            if (hseed < 4) hseed = 4;
            if (cseed > (seed + 8 + 5*legend)) cseed = seed + 8+5*legend;
            if (mseed > (seed + 8 + 5*legend)) mseed = seed + 8+5*legend;
            if (sseed > (seed + 8 + 5*legend)) sseed = seed + 8+5*legend;
            if (cseed > 25+2*legend) cseed = 25+2*legend;
            if (mseed > 25+2*legend) mseed = 25+2*legend;
            if (sseed > 25+2*legend) sseed = 25+2*legend;
            if (legend >= 2 && mseed < 10) //legendary creatures with little magic should be über-mighty
            {
                if (coinFlip()) cseed += 4 + r.Next(3);
                else sseed += 4 + r.Next(3);
            }
            if (cseed < 0) cseed = 0;
            if (mseed < 0) mseed = 0;
            if (sseed < 0) sseed = 0;
            minc = r.Next(seed/4) + r.Next(seed/4) + cseed/2;
            minm = r.Next(seed/4) + r.Next(seed/4) + mseed/2;
            mins = r.Next(seed/4) + r.Next(seed/4) + sseed/2;
            if (cseed < 2) minc = 0;
            if (mseed < 2) minm = 0;
            if (sseed < 2) mins = 0;
            if (minc > cseed) minc = cseed;
            if (minm > mseed) minm = mseed;
            if (mins > sseed) mins = sseed;
            if (minc > seed+2) minc = seed+2;
            if (minm > seed+2) minm = seed+2;
            if (mins > seed+2) mins = seed+2;
            if (minc > 12) minc = 11 + r.Next(3);
            if (minm > 12) minm = 11 + r.Next(3);
            if (mins > 12) mins = 11 + r.Next(3);
            if (TamePower != null)
            {
                minc += TamePower.CBonus; if (minc < 8) minc += r.Next(3);
                minm += TamePower.MBonus; if (minm < 8) minm += r.Next(3);
                mins += TamePower.SBonus; if (mins < 8) mins += r.Next(3);
            }
            pseed = seed*500 + r.Next(3)*100;
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
            if (!Ecoable)
                return null;
            BeastHarvest harvest = new BeastHarvest(seed, Eco, this);
            this.Harvest = harvest.ID;
            return harvest;
        }
        public bool IsEligible(Soldier soldier)
        {
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
}