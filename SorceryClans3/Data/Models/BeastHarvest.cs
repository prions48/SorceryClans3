namespace SorceryClans3.Data.Models
{
    public class BeastHarvest
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid HunterID { get; set; }
        Random r = new Random();
        private bool coinFlip()
        {
            return r.Next(2) == 0;
        }
        private int lvl { get; set; }
        public string source { get; set; }
        public string item { get; set; }
        public string hunter { get; set; }
        public string SpellName { get { return item + " of the " + source + " (hunted by the " + hunter + ")"; } }
        public bool pwr { get; set; }
        public int yield { get; set; }
        public int cbonus { get; set; }
        public int mbonus { get; set; }
        public int sbonus { get; set; }
        public int zbonus { get; set; }
        public int lbonus { get; set; }
        public int tbonus { get; set; }
        public int hbonus { get; set; }
        public int kbonus { get; set; }
        public int dbonus { get; set; }
        public int danger { get; set; }
        public int droprate { get; set; }
        public StatBlock MaxStats { get; set; }
        private HunterMission Mission { get; set; }
        public string HarvestName
        {
            get
            {
                return item + " of the " + source;
            }
        }
        public bool IsEligible(Soldier soldier)
        {
            if (soldier.Medical == null && kbonus > 0)
                return false;
            if (pwr && soldier.Power != null)
                return false;
            if (soldier.Boosts.Contains(this.ID))
                return false;
            return MaxStats.IsBelow(soldier);
        }
        public void Apply(Soldier soldier)
        {
            if (!IsEligible(soldier))
                return;
            if (soldier.Medical != null)//should be always true for now
                soldier.Medical.HealBase += kbonus;
            soldier.Boosts.Add(this.ID);
            soldier.CharismaBase += zbonus;
            soldier.LogisticsBase += lbonus;
            soldier.TacticsBase += tbonus;
            if (pwr)
            {
                soldier.Power = GeneratePower();
            }
            else
            {
                soldier.ComBase += cbonus;
                soldier.MagBase += mbonus;
                soldier.SubBase += sbonus;
                soldier.HPBase += hbonus;
                soldier.TravelBase += dbonus;
            }
        }
        public Power GeneratePower()
        {
            return new Power()
                {
                    CBonusMax = cbonus,
                    MBonusMax = mbonus,
                    SBonusMax = sbonus,
                    HBonusMax = hbonus,
                    KBonusMax = kbonus,
                    DBonusMax = dbonus,
                    Color = MagicColor.Green,
                    Mastery = 0.1 + r.NextDouble()*.2,
                    MaxColors = r.Next(4)+1,
                    PowerIncrementForColor = 800 + 100*r.Next(5),
                    MinPowerForColor = 1500 + 100*r.Next(5),
                    PowerName = "Magic of the " + source
                };
        }
        public HunterMission GetMission
        {
            get
            {
                return Mission;
            }
        }
        public HunterMission StartMission()
        {
            HunterMission mission = Mission;
            Mission = new HunterMission(lvl * 3000, true, true)
            {
                Type = MissionType.Harvest
            };
            return mission;
        }
        public BeastHarvest(int lvl, BeastEco eco, Beast beast)
        {
            if (lvl == 0)
                lvl = 1;
            this.lvl = lvl;
            Mission = new HunterMission(lvl * 3000, true, true);
            HunterID = beast.ID;
            hunter = beast.FullName;
            if (eco == BeastEco.Herbivore)
            {
                generatePlant(lvl);
            }
            else if (eco == BeastEco.Omnivore)
            {
                if (coinFlip())
                    generatePlant(lvl);
                else
                    generateSmallAnimal(lvl);
            }
            else if (eco == BeastEco.Carnivore)
            {
                generateSmallAnimal(lvl);
            }
            else if (eco == BeastEco.Hypercarnivore)
            {
                generateLargeAnimal(lvl);
            }
            else if (eco == BeastEco.HyperOmnivore)
            {
                if (coinFlip())
                    generatePlant(lvl);
                else
                    generateLargeAnimal(lvl);
            }
            else if (eco == BeastEco.Insect)
            {
                generateInvertebrate(lvl);
            }
            else if (eco == BeastEco.InsectCarnivore)
            {
                if (coinFlip())
                    generateSmallAnimal(lvl);
                else
                    generateInvertebrate(lvl);
            }
            else if (eco == BeastEco.AquaticAny || (eco == BeastEco.AquaticFish && coinFlip()))
            {
                generateAquaticInvert(lvl);
            }
            else if (eco == BeastEco.AquaticFish)
            {
                generateAquaticVert(lvl);
            }
            else //eco ==7
            {
                if (coinFlip())
                    generatePlant(lvl);
                else
                    generateInvertebrate(lvl);
            }
            if (pwr)
                danger = lvl/4+1;
            else
                danger = lvl/6;
            droprate = 10-lvl/4-lvl/7;
            //H.p("VERBOSE MODE: danger:" + danger + " droprate:" + droprate);
            int variance = r.Next(4);
            while (variance > 0 && danger < 10 && droprate < 12)
            {
                danger++;
                droprate++;
                variance--;
            }
            if (danger < 1) danger = 1;
            if (droprate > 13) droprate = 13;
            if (droprate < 1) droprate = 1;
            //H.pl("  VAR: danger:" + danger + " droprate:" + droprate);
            int? cmax=null, mmax=null, smax=null, hmax=null,kmax=null,dmax=null,tmax=null,lmax=null,zmax=null;
            int extrab = 0;
            if (pwr)
                extrab = r.Next(1, 4);
            if (cbonus > 0) cmax = 5 + lvl/4 + cbonus/3 + r.Next(3) + extrab;
            if (mbonus > 0) mmax = 5 + lvl/4 + mbonus/3 + r.Next(3) + extrab;
            if (sbonus > 0) smax = 5 + lvl/4 + sbonus/3 + r.Next(3) + extrab;
            if (dbonus > 0) dmax = 5 + lvl/4 + dbonus/3 + r.Next(3) + extrab;
            if (zbonus > 0) zmax = 5 + lvl/4 + zbonus/3 + r.Next(3) + extrab;
            if (lbonus > 0) lmax = 5 + lvl/4 + lbonus/3 + r.Next(3) + extrab;
            if (tbonus > 0) tmax = 8 + lvl/3 + tbonus/2 + r.Next(3) + extrab;
            if (hbonus > 0) hmax = 8 + lvl/3 + hbonus/2 + r.Next(3) + extrab;
            if (kbonus > 0) kmax = 8 + lvl/4 + kbonus/4;
            MaxStats = new StatBlock(cmax, mmax, smax, hmax, kmax, null, dmax, zmax, lmax, tmax);
        }
        public void createStats(int c, int m, int s, int h, int k, int d, int z, int l, int t)
        {
            if (k==1 && coinFlip()) {k = 0; d++; }
            else if (k > 1) k *= 2;
            int pts=0;
            pwr = false;
            if (r.NextDouble() < lvl / 25.0 || r.NextDouble() < .1)
                pwr = true;
            if (source == "Heart-Shaped Herb") //muahahaha
            {
                pwr=true;
                pts+=10;
                yield=1;
            }
            if (pwr)
                pts += 3 + (int)(2.5*lvl);
            else
                pts += 3 + (int)(1.5*lvl);
            cbonus=0;
            mbonus=0;
            sbonus=0;
            dbonus=0;
            zbonus=0;
            lbonus=0;
            tbonus=0;
            hbonus=0;
            kbonus=0;
            pts = (int)(1.0 / Math.Sqrt(yield) * pts + .5);
            while (pts > 0)
            {
                if (z * 1.0 / (z+l+t+d+c+m+s+h+k) > r.NextDouble() && pts >= leadCost())
                {
                    zbonus++;
                    pts-=leadCost();
                    if (pts >= leadCost() && coinFlip())
                    {
                        zbonus++;
                        pts -= leadCost();
                    }
                }
                else if (l * 1.0 / (l+t+d+c+m+s+h+k) > r.NextDouble() && pts >= leadCost())
                {
                    lbonus++;
                    pts-=leadCost();
                    if (pts >= leadCost() && coinFlip())
                    {
                        lbonus++;
                        pts -= leadCost();
                    }
                }
                else if (t * 1.0 / (t+d+c+m+s+h+k) > r.NextDouble() && pts >= leadCost())
                {
                    tbonus++;
                    pts-=leadCost();
                    if (pts >= leadCost() && coinFlip())
                    {
                        tbonus++;
                        pts -= leadCost();
                    }
                }
                else if (d * 1.0 / (d+c+m+s+h+k) > r.NextDouble() && pts >= dCost())
                {
                    dbonus++;
                    pts-=dCost();
                    if (pts >= dCost() && coinFlip())
                    {
                        dbonus++;
                        pts -= dCost();
                    }
                }
                else if (c * 1.0 / (c+m+s+h+k) > r.NextDouble() && pts >= cCost())
                {
                    cbonus++;
                    pts-=cCost();
                    if (pts >= cCost() && coinFlip())
                    {
                        cbonus++;
                        pts -= cCost();
                    }
                }
                else if (m * 1.0 / (m+s+h+k) > r.NextDouble() && pts >= mCost())
                {
                    mbonus++;
                    pts-=mCost();
                    if (pts >= mCost() && coinFlip())
                    {
                        mbonus++;
                        pts -= mCost();
                    }
                }
                else if (s * 1.0 / (s+h+k) > r.NextDouble() && pts >= sCost())
                {
                    sbonus++;
                    pts-=sCost();
                    if (pts >= sCost() && coinFlip())
                    {
                        sbonus++;
                        pts -= sCost();
                    }
                }
                else if (k * 1.0 / (h+k) > r.NextDouble() && pts >= kCost())
                {
                    kbonus++;
                    pts-=kCost();
                    if (pts >= kCost() && r.NextDouble() < .8) //+1heal is lame, increase the odds of having more than one
                    {
                        kbonus++;
                        pts -=kCost();
                    }
                }
                else if (h == 0 && yield > 5 && r.NextDouble() < .7) //one extra thing to spend pts on if HP is not called for
                {
                    yield++;
                    pts--;
                }
                else //hp, also by default
                {
                    hbonus++;
                    pts--;
                }
            }
        }
        
        private int cCost()
        {
            if (cbonus <= 3)
                return 3;
            return 3+(cbonus-3);
        }
        private int mCost()
        {
            if (mbonus <= 3)
                return 5;
            return 5+(mbonus-3);
        }
        private int sCost()
        {
            if (sbonus <= 3)
                return 4;
            return 4+(sbonus-3);
        }
        private int dCost()
        {
            if (dbonus <= 5)
                return 1;
            return 1+(dbonus-5);
        }
        private int leadCost()
        {
            return 2;
        }
        private int kCost()
        {
            if (kbonus <= 3)
                return 6;
            return 6+(kbonus-3);
        }
        
        public void generateAquaticInvert(int lvl)
        {
            int c=0,m=0,s=0,h=0,k=0,d=0,z=0,l=0,t=0;
            int arth=0,moll=0,echin=0;
            bool poison = false, ceph = false, claw=false;
            switch (r.Next(6)+lvl/5)
            {
                case 0: source = "Clam"; moll=1; c++; h++; break;
                case 1: source = "Sea Urchin"; echin=1; poison=true; m++; s++; break;
                case 2: source = "Crawfish"; claw=true; s+=2; arth=1; break;
                case 3: source = "Crab"; claw=true; c+=2; arth=1; break;
                case 4: source = "Sea Star"; s++; h+=2; echin=1; break;
                case 5: source = "Lobster"; claw=true; m+=2; d++; arth=1; break;
                case 6: source = "Sea Slug"; moll=1; poison=true; h++; m++; break;
                case 7: source = "Jellyfish"; c+=2; poison=true; break;
                case 8: source = "Horsheshoe Crab"; claw=true; m++; h++; poison=true; arth=1; break; 
                default: source = "Octopus"; k+=2; moll=1; ceph=true; poison=true; break;
            }
            if (arth == 1)
            {
                switch (r.Next(5))
                {
                    case 0: item = "Leg"; s++; d++; yield=r.Next(4)+4; break;
                    case 1: item = "Claw"; c++; yield=2; break;
                    case 2: item = "Eye"; m++; yield=r.Next(2)+1; break;
                    case 3: item = "Eggs"; h++; l++; yield=r.Next(10)+7; break;
                    case 4: if (poison) item = "Poison"; else item = "Antenna";
                            k++; yield=2; break;
                }
            }
            else if (echin == 1)
            {
                switch (r.Next(3))
                {
                    case 0: item = "Leg"; s++; yield=r.Next(4)+4; break;
                    case 1: item = "Spines"; c++; yield=10; break;
                    case 2: if (poison) item = "Poison"; else item = "Shell";
                            m++; yield=1; break;
                }
            }
            else if (moll == 1)
            {
                switch (r.Next(5))
                {
                    case 0: item = "Heart"; s++; yield=r.Next(2)+2; break;
                    case 1: item = "Mantle"; c++; yield=1; break;
                    case 2: if (ceph) item = "Eye"; else item = "Flesh"; m++; yield=r.Next(2)+1; break;
                    case 3: if (poison) item = "Poison"; else item = "Blood";
                            k++; yield=2; break;
                    case 4: if (ceph) item = "Tentacle"; else item = "Slime"; yield=r.Next(5)+4; z++; break;
                    case 5: if (ceph) item = "Ink Sac"; else item = "Foot"; yield=1; s++; break;
                }
            }
            else //jellyfish
            {
                switch (r.Next(3))
                {
                    case 0: item = "Tentacle"; s++; yield=r.Next(6)+6; break;
                    case 1: item = "Flesh"; c++; l++; yield=1; break;
                    case 2: if (poison) item = "Poison"; else item = "Polyps"; yield=3; m++; break;
                }
            }
            switch (r.Next(9))
            {
                case 0: source = "spotted " + source; d++; break;
                case 1: source = "ringed " + source; m++; break;
                case 2: source = "striped " + source; l++; break;
                case 3: source = "speckled " + source; h++; break;
                case 4: source = "banded " + source; k++; break;
                case 5: source = "bellied " + source; c++; break;
                case 6: source = "plumed " + source; z++; break;
                case 7: source = "blotched " + source; s++; break;
                case 8:
                    if (claw) source = "clawed " + source;
                    else source = "blooded " + source;
                    t++; break;
            }
            switch (r.Next(9))
            {
                case 0: source = "Red-" + source; c++; break;
                case 1: source = "Gold-" + source; m++; break;
                case 2: source = "Blue-" + source; s++; break;
                case 3: source = "Black-" + source; h++; break;
                case 4: source = "Purple-" + source; k++; break;
                case 5: source = "Green-" + source; d++; break;
                case 6: source = "Silver-" + source; t++; break;
                case 7: source = "White-" + source; z++; break;
                case 8: source = "Pink-" + source; l++; break;
            }
            createStats(c, m, s, h, k, d, z, l, t);
        }
        
        public void generateAquaticVert(int lvl)
        {
            int c=0,m=0,s=0,h=0,k=0,d=0,z=0,l=0,t=0,poison=0;
            switch (r.Next(8))
            {
                case 0: source = "Trout"; c++; h++; break;
                case 1: source = "Salmon"; l++; m++; break;
                case 2: source = "Tuna"; z++; c++; break;
                case 3: source = "Mackerel"; s++; t++; break;
                case 4: source = "Pufferfish"; poison=2; k+=2; break;
                case 5: source = "Bass"; c++; m++; break;
                case 6: source = "Catfish"; h++; d++; break;
                case 7: source = "Lionfish"; m++; l++; poison=2; break;
            }
            switch (r.Next(8)+poison)
            {
                case 0: item = "Fin"; yield=3; h++; t++; break;
                case 1: item = "Eye"; yield=2; m++; z++; break;
                case 2: item = "Eggs"; yield=6; l++; break;
                case 3: item = "Scales"; yield=12; c++; break;
                case 4: item = "Liver"; yield=1; m++; danger++; break;
                case 5: item = "Heart"; yield=1; z+=2; break;
                case 6: item = "Spine"; yield=1; c++; s++; break;
                case 7: item = "Gills"; yield=1; d+=2; break;
                case 8: case 9: item = "Poison"; yield=1; k++; break;
            }
            switch (r.Next(7))
            {
                case 0: source = "bellied " + source; s++; break; 
                case 1: source = "finned " + source; c++; break; 
                case 2: source = "tailed " + source; h++; break; 
                case 3: source = "headed " + source; m++; break;
                case 4: source = "spotted " + source; s++; break;
                case 5: source = "eyed " + source; m++; break;
                case 6: source = "toothed " + source; c++; break;
            }
            switch (r.Next(9))
            {
                case 0: source = "Red-" + source; c++; break;
                case 1: source = "Gold-" + source; m++; break;
                case 2: source = "Blue-" + source; s++; break;
                case 3: source = "Black-" + source; h++; break;
                case 4: source = "Purple-" + source; k++; break;
                case 5: source = "Green-" + source; d++; break;
                case 6: source = "Silver-" + source; t++; break;
                case 7: source = "White-" + source; z++; break;
                case 8: source = "Pink-" + source; l++; break;
            }
            createStats(c, m, s, h, k, d, z, l, t);
        }
        
        public void generatePlant(int lvl)
        {
            int c=0,m=0,s=0,h=0,k=0,d=0,z=0,l=0,t=0;
            int type; //0=mushroom, 1=lower, 2=angio, 3=tree
            int fruit=0,seed=0,flower=0,root=0,leaf=0,bark=0,nut=0,thorn=0;
            int name = (int)(r.NextDouble()*lvl/2+lvl/2);
            switch (name)
            {
                case 0: source = "Fern"; root=1; leaf=1; bark=1; type=1; m++; break; 
                case 1: source = "Vine"; flower=1; seed=1; root=1; thorn=1; type=2; s++; break; 
                case 2: source = "Mushroom"; fruit=1; root=1; type=0; h++; break; 
                case 3: source = "Shrub"; seed=1; flower=1; root=1; leaf=1; nut=1; type=2; c++; break; 
                case 4: source = "Weed"; seed=1; flower=1; root=1; leaf=1; thorn=1; type=2; h++; break; 
                case 5: source = "Clover"; flower=1; root=1; leaf=1; type=2; k++; break;
                case 6: source = "Lichen"; fruit=1; bark=1; type=1; c++; break; 
                case 7: source = "Grass"; seed=1; flower=1; root=1; type=2; s++; break; 
                case 8: source = "Orchid"; fruit=1; seed=1; flower=1; leaf=1; type=2; m++; break; 
                case 9: source = "Flytrap"; fruit=1; root=1; flower=1; type=1; s++; break;
                case 10: source = "Oak"; root=1; nut=1; bark=1; leaf=1; type=3; c++; break;
                case 11: source = "Mushroom"; fruit=1; root=1; type=0; h++; break; 
                case 12: source = "Kelp"; leaf=1; root=1; seed=1; type=1; s++; break;
                case 13: source = "Redwood"; root=1; nut=1; bark=1; leaf=1; type=3; h++; break; 
                case 14: source = "Cactus"; fruit=1; root=1; flower=1; seed=1; thorn=1; type=1; c++; break;
                case 15: source = "Lotus"; fruit=1; root=1; flower=1; type=2; m++; break;
                default: source = "Herb"; root=1; seed=1; fruit=1; leaf=1; flower=1; type=2; m++; break;
            }
            if (r.NextDouble() < .1)
            {
                item = "Sap";
                z++;
                l++;
                t++;
                h++;
                yield = r.Next(3, 8);
            }
            else if (r.NextDouble() < fruit * 1.0 / (fruit+seed+flower+root+leaf+bark+nut+thorn))
            {
                item = "Fruit";
                m+=2;
                k++;
                yield = r.Next(2, 6);
            }
            else if (r.NextDouble() < seed * 1.0 /(seed+flower+root+leaf+bark+nut+thorn))
            {
                c+=2;
                s++;
                item = "Seed";
                yield = r.Next(2,12);
            }
            else if (r.NextDouble() < flower * 1.0 /(flower+root+leaf+bark+nut+thorn))
            {
                m+=2;
                c++;
                l++;
                item = "Flower";
                yield = r.Next(2, 5);
            }
            else if (r.NextDouble() < root * 1.0 /(root+leaf+bark+nut+thorn))
            {
                c+=2;
                s++;
                t++;
                item = "Root";
                yield = r.Next(3, 8);
            }
            else if (r.NextDouble() < leaf * 1.0 /(leaf+bark+nut+thorn))
            {
                s+=2;
                m++;
                z++;
                item = "Leaf";
                yield = r.Next(1, 3);
            }
            else if (r.NextDouble() < bark * 1.0 /(bark+nut+thorn))
            {
                h+=2;
                k++;
                item = "Bark";
                yield = r.Next(4,9);
            }
            else if (r.NextDouble() < nut * 1.0 /(nut+thorn))
            {
                c+=2;
                d++;
                item = "Nut";
                yield = r.Next(2, 5);
            }
            else
            {
                c+=2;
                h++;
                item = "Thorn";
                yield = r.Next(6, 18);
            }
            if (source == "Herb" && r.NextDouble() < .8 && lvl > 18)
            {
                source = "Heart-Shaped Herb";
                c+=2;
                m+=2;
                s+=2;
                h+=2;
            }
            else if (type == 0)
            {
                switch (r.Next(9))
                {
                    case 0: source = "Spotted " + source; h++; break;
                    case 1: source = "Red " + source; c++; break;
                    case 2: source = "Blue " + source; k++; break;
                    case 3: source = "Black " + source; s++; break;
                    case 4: source = "Golden " + source; m++; break;
                    case 5: source = "Spotted " + source; d++; break;
                    case 6: source = "Red " + source; z++; break;
                    case 7: source = "Blue " + source; l++; break;
                    case 8: source = "Black " + source; t++; break;
                }
            }
            else if (type == 1) //lichen, fern, kelp
            {
                switch (r.Next(9))
                {
                    case 0: source = "Crimson-" + source; c++; break;
                    case 1: source = "Gold-" + source; m++; break;
                    case 2: source = "Blue-" + source; s++; break;
                    case 3: source = "Black-" + source; h++; break;
                    case 4: source = "Violet-" + source; k++; break;
                    case 5: source = "Green-" + source; d++; break;
                    case 6: source = "Silver-" + source; t++; break;
                    case 7: source = "White-" + source; z++; break;
                    case 8: source = "Pink-" + source; l++; break;
                }
            }
            else if (type == 2) //angiosperm not woody
            {
                if (coinFlip()) source = "-flowered " + source; else source = "-leafed " + source;
                switch (r.Next(9))
                {
                    case 0: source = "Crimson-" + source; c++; break;
                    case 1: source = "Gold-" + source; m++; break;
                    case 2: source = "Blue-" + source; s++; break;
                    case 3: source = "Black-" + source; h++; break;
                    case 4: source = "Violet-" + source; k++; break;
                    case 5: source = "Green-" + source; d++; break;
                    case 6: source = "Silver-" + source; t++; break;
                    case 7: source = "White-" + source; z++; break;
                    case 8: source = "Pink-" + source; l++; break;
                }
            }
            else//if (type == 3) //tree
            {
                if (coinFlip()) source = "-flowered " + source; else source = "-leafed " + source;
                switch (r.Next(9))
                {
                    case 0: source = "Crimson-" + source; c++; break;
                    case 1: source = "Gold-" + source; m++; break;
                    case 2: source = "Blue-" + source; s++; break;
                    case 3: source = "Black-" + source; h++; break;
                    case 4: source = "Violet-" + source; k++; break;
                    case 5: source = "Green-" + source; d++; break;
                    case 6: source = "Silver-" + source; t++; break;
                    case 7: source = "White-" + source; z++; break;
                    case 8: source = "Pink-" + source; l++; break;
                }
            }
            createStats(c, m, s, h, k, d, z, l, t);
        }
        
        public string birdName1()
        {
            switch (r.Next(5))
            {
                case 0: return "Sparrow";
                case 1: return "Quail";
                case 2: return "Jay";
                case 3: return "Finch";
                default: return "Robin";
            }
        }
        
        public string birdName2()
        {
            switch (r.Next(5))
            {
                case 0: return "Pigeon";
                case 1: return "Raven";
                case 2: return "Crow";
                case 3: return "Kite";
                default: return "Woodpecker";
            }
        }
        public string amphibName()
        {
            switch (r.Next(4))
            {
                case 0: return "Frog";
                case 1: return "Toad";
                case 2: return "Newt";
                default: return "Salamander";
            }
        }
        public string rodentName()
        {
            switch (r.Next(5))
            {
                case 0: return "Mouse";
                case 1: return "Rat";
                case 2: return "Squirrel";
                case 3: return "Mole";
                default: return "Hare";
            }
        }
        
        public String birdPart()
        {
            switch (r.Next(5))
            {
                case 0: return "winged";
                case 1: return "throated";
                case 2: return "clawed";
                case 3: return "crested";
                default: return "beaked";
            }
        }
        public string mammalPart()
        {
            switch (r.Next(5))
            {
                case 0: return "backed";
                case 1: return "tailed";
                case 2: return "eyed";
                case 3: return "chested";
                default: return "furred";
            }
        }
        public void generateSmallAnimal(int lvl)
        {
            int c=0,m=0,s=0,h=0,k=0,d=0,z=0,l=0,t=0;
            int type; //1=herp,2=bird,3=mamm
            int fur=0,scales=0,claw=0,feather=0,tooth=0,wing=0;
            int name = r.Next(8)+lvl/3;
            switch (name)
            {
                case 0: source = rodentName(); fur=1; s++; l++; type=3; break; 
                case 1: if (coinFlip()) source = "Iguana"; else source = "Lizard"; scales=1; c++; type=1; break; 
                case 2: case 3: source = rodentName(); fur=1; h++; type=3; break; 
                case 4: source = "Bat"; fur=1; tooth=1; wing=1; k++; d++; type=3; break; 
                case 5: case 6: source = birdName1(); claw=1; feather=1; wing=1; m++; type=2; break;
                case 7: source = amphibName(); s++; type=1; t++; break; 
                case 8: source = birdName2(); wing=1; feather=1; claw=1; m++; type=2; break;
                case 9: source = "Serpent"; scales=1; tooth=1; l++; h++; type=1; break; 
                case 10: source = "Weasel"; fur=1; tooth=1; c++; z++; type=3; break; 
                default: source = birdName2(); wing=1; feather=1; k++; type=2; break; 
            }
            if (coinFlip() || (fur+scales+claw+feather+tooth+wing < 1))
            {
                yield = 1;
                switch (r.Next(5))
                {
                    case 0: item = "Heart"; c+=3; break;
                    case 1: item = "Liver"; m+=3; break;
                    case 2: item = "Flesh"; s+=3; break;
                    case 3: item = "Eye"; yield = coinFlip() ? 1 : 2; z++; l++; t++; break;
                    default: item = "Blood"; k++; h++; d++; break;
                }
            }
            else
            {
                if (r.NextDouble() < fur * 1.0 / (fur+scales+claw+feather+tooth+wing))
                {
                    item = "Fur";
                    c+=2;
                    z++;
                    yield = r.Next(4, 8);
                }
                else if (r.NextDouble() < scales * 1.0 / (scales+claw+feather+tooth+wing))
                {
                    item = "Scales";
                    m+=2;
                    s++;
                    yield = r.Next(8, 15);
                }
                else if (r.NextDouble() < claw * 1.0 / (claw+feather+tooth+wing))
                {
                    item = "Claw";
                    c+=2;
                    h++;
                    z++;
                    yield = r.Next(2, 7);
                }
                else if (r.NextDouble() < feather * 1.0 / (feather+tooth+wing))
                {
                    item = "Feather";
                    k+=2;
                    m++;
                    yield = r.Next(6, 18);
                }
                else if (r.NextDouble() < tooth * 1.0 / (tooth+wing))
                {
                    item = "Tooth";
                    if (source == "Serpent" && r.NextDouble() < .8)
                        item = "Fang";
                    c+=2;
                    l++;
                    yield = r.Next(2, 15);
                }
                else
                {
                    item = "Wing";
                    t+=2;
                    d++;
                    yield = r.Next(1, 3);
                }
            }
            if (type == 1) //herp
            {
                if (r.NextDouble() < .33) source = "-eyed " + source;
                    else if (coinFlip()) source = "-spotted " + source;
                    else source = "-bellied " + source;
                switch (r.Next(9))
                {
                    case 0: source = "Violet" + source; k++; break;
                    case 1: source = "Iridescent" + source; m++; break;
                    case 2: source = "Crimson" + source; c++; break;
                    case 3: source = "Black" + source; s++; break;
                    case 4: source = "White" + source; h++; break;
                    case 5: source = "Gold" + source; z++; break;
                    case 6: source = "Orange" + source; d++; break;
                    case 7: source = "Teal" + source; l++; break;
                    case 8: source = "Pink" + source; t++; break;
                }
            }
            else if (type == 2) //bird
            {
                source = "-" + birdPart() + " " + source;
                switch (r.Next(9))
                {
                    case 0: source = "Violet" + source; k++; break;
                    case 1: source = "Blue" + source; l++; break;
                    case 2: source = "Crimson" + source; c++; break;
                    case 3: source = "Pink" + source; s++; break;
                    case 4: source = "White" + source; h++; break;
                    case 5: source = "Silver" + source; t++; break;
                    case 6: source = "Gold" + source; z++; break;
                    case 7: source = "Bronze" + source; m++; break;
                    case 8: source = "Green" + source; d++; break;
                }
            }
            else //type==3 //mammal
            {
                source = "-" + mammalPart() + " " + source;
                switch (r.Next(9))
                {
                    case 0: source = "Violet" + source; k++; break;
                    case 1: source = "Blue" + source; m++; break;
                    case 2: source = "Crimson" + source; c++; break;
                    case 3: source = "Pink" + source; s++; break;
                    case 4: source = "Snowy" + source; h++; break;
                    case 5: source = "Silver" + source; l++; break;
                    case 6: source = "Black" + source; t++; break;
                    case 7: source = "Indigo" + source; d++; break;
                    case 8: source = "Brown" + source; z++; break;
                }
            }
            createStats(c, m, s, h, k, d, z, l, t);
        }
        public string bigMammalPart()
        {
            if (r.NextDouble() < .3 &&
                    (item == "Horn" || item == "Tusk" || item == "Antler"
                    || item == "Claw" || item == "Hoof"))
                return item.ToLower() + "ed";
            return mammalPart();
        }
        public void generateInvertebrate(int lvl)
        {
            int c=0,m=0,s=0,h=0,k=0,d=0,z=0,l=0,t=0;
            int shell=0,stinger=0,slime=0,wing=0,noleg=0,larva=0;
            switch (r.Next(10)+lvl/5)
            {
                case 0: source = "Worm"; slime=1; noleg=1; c++; h++; break;
                case 1: source = "Queen Bee"; wing=1; stinger=1; larva=1; m+=2; break;
                case 2: source = "Spider"; stinger=1; s++; t+=2; break;
                case 3: source = "Snail"; slime=1; shell=1; noleg=1; k++; break;
                case 4: source = "Slug"; slime=1; noleg=1; h++; c++; break;
                case 5: source = "Moth"; wing=1; larva=1; s+=2; break;
                case 6: source = "Locust"; wing=1; larva=1; s++; m++; break;
                case 7: source = "Beetle"; shell=1; larva=1; wing=1; m++; k+=2; break;
                case 8: source = "Wasp"; stinger=1; larva=1; wing=1; z++; c++; break;
                case 9: source = "Scorpion"; stinger=1; c++; m++; break;
                case 10: source = "Dragonfly"; wing=1; larva=1; m++; s++; break;
                case 11: source = "Trilobite"; shell=1; c++; h++; l++; break;
                default: source = "Ant Queen"; stinger=1; larva=1; wing=1; m++; break;
            }
            if (r.NextDouble() < (0.2*(wing+shell+slime+stinger+larva))) //currently all options have at least one of the types
            {
                if (larva * 1.0 / (larva + wing + shell + slime + stinger) > r.NextDouble())
                {
                    item = "Larva";
                    if (source == "Queen")
                        yield = 15 + r.Next(15);
                    else
                        yield = r.Next(1, 6);
                    c++;
                    s += 2;
                }
                else if (wing * 1.0 / (wing + shell + slime + stinger) > r.NextDouble())
                {
                    item = "Wing";
                    yield = r.Next(2)+1;
                    t+=2;
                    s+=2;
                }
                else if (shell * 1.0 / (shell + slime + stinger) > r.NextDouble())
                {
                    item = "Shell";
                    yield = 1;
                    k++;
                    m+=2;
                }
                else if (slime * 1.0 / (slime + stinger) > r.NextDouble())
                {
                    item = "Slime";
                    yield = r.Next(4, 8);
                    c++;
                    h += 3;
                    d++;
                }
                else //stinger
                {
                    if (coinFlip())
                        item = "Stinger";
                    else
                        item = "Venom";
                    yield = 1;
                    m++;
                    c+=2;
                }
            }
            else
            {
                switch (r.Next(5)+noleg)
                {
                    case 0: item = "Leg"; c+=2; h+=2; break;
                    case 1: item = "Eye"; m+=2; s++; break;
                    case 2: item = "Flesh"; m++; c++; s++; break;
                    case 3: item = "Blood"; k+=2; m+=2; yield=4; break;
                    case 4: item = "Body"; s+=2; c++; break;
                    default: item = "Mantle"; m++; c+=2; break; //only clams and snails/slugs
                }
                yield = 2 + r.Next(2) + r.Next(2)*r.Next(4);
            }
            switch (r.Next(8))
            {
                case 0: source = "Violet " + source; k++; break;
                case 1: source = "Blue " + source; t++; break;
                case 2: source = "Crimson " + source; c++; break;
                case 3: source = "Green " + source; s++; break;
                case 4: source = "Yellow " + source; h++; break;
                case 5: source = "Silver " + source; s++; break;
                case 6: source = "Black " + source; d++; break;
                case 7: source = "Golden " + source; z++; break;
                case 8: source = "Bronze " + source; l++; break;
            }
            createStats(c, m, s, h, k, d, z, l, t);
        }
        
        public void generateLargeAnimal(int lvl)
        {
            int c=0,m=0,s=0,h=0,k=0,d=0,z=0,l=0,t=0;
            int tusk=0,horn=0,antler=0,claw=0,hoof=0;
            int name = r.Next(11)+lvl/5;
            switch (name) {
                case 0: source = "Deer"; antler=1; hoof=1; s++; l++; break;
                case 1: if (coinFlip()) source = "Goat"; else source = "Sheep";  horn=2; hoof=1; c++; break;
                case 2: if (coinFlip()) source = "Horse"; else source = "Zebra";  hoof=2; m++; l++; break;
                case 3: if (coinFlip()) source = "Gazelle"; else source = "Antelope"; d+=2; horn=2; hoof=1; h++; break;
                case 4: if (coinFlip()) source = "Camel"; else source = "Llama"; z++; hoof=1; c++; break;
                case 5: if (r.NextDouble() < .33) source = "Elk"; else if (coinFlip()) source = "Caribou";
                        else source = "Reindeer"; hoof=1; antler=1; m++; break;
                case 6: if (r.NextDouble() < .33) source = "Impala"; else if (coinFlip()) source = "Kudu";
                        else source = "Wildebeest";  horn=2; hoof=1; c++; d+=3; break;
                case 7: if (coinFlip()) source = "Wallaby"; else source = "Kangaroo";  claw=1; h++; k++; break;
                case 8: if (coinFlip()) source = "Buffalo"; else source = "Bison"; horn=2; hoof=1; c++; break;
                case 9: if (coinFlip()) source = "Giraffe"; else source = "Moose"; ; antler=1; c++; z++; break;
                case 10: source = "Ground Sloth"; claw=1; m++; break;
                case 11: if (coinFlip()) { source = "Hippo"; tusk=1; } else { source = "Rhino"; horn=1; } hoof=1; m++; break;
                case 12: if (coinFlip()) source = "Mastodon"; else source = "Mammoth"; t++; tusk=1; h++; c++; break;
                default: source = "Unicorn"; horn=1; hoof=1; m++; h++; k++; break; 
            }
            if (tusk == 1 && r.NextDouble() < .4)
            {
                item = "Tusk";
                yield = 1 + r.Next(2);
                m++;
                k+=3;
            }
            else if (antler == 1 && coinFlip())
            {
                item = "Antler";
                yield = 1 + r.Next(2);
                if (coinFlip())
                    c += 2;
                else
                    k += 2;
                h++;
            }
            else if (horn > 0 && r.NextDouble() < .4)
            {
                item = "Horn";
                yield = horn;
                s+=2;
                m++;
            }
            else if (claw == 1 && r.NextDouble() < .3)
            {
                item = "Claw";
                yield = 1;
                t+=2;
                m++;
            }
            else if (hoof == 1 && r.NextDouble() < .3)
            {
                item = "Hoof";
                yield = 1 + r.Next(4);
                t++;
                l++;
                z++;
            }
            else
            {
                yield = 1;
                switch (r.Next(7))
                {
                    case 0: item = "Heart"; m+=3; t++; break; 
                    case 1: item = "Liver"; c+=3; break; 
                    case 2: item = "Hide"; d+=2; h++; break; 
                    case 3: item = "Eye"; m+=3; yield++; break; 
                    case 4: item = "Blood"; z+=2; m++; yield += r.Next(3); break; 
                    case 5: item = "Flesh"; l+=2; m++; yield += r.Next(3); break; 
                    case 6: item = "Sinew"; d+=2; h++; yield += r.Next(3); break; 
                }
            }
            switch (r.Next(9))
            {
                case 0: source = "Violet-" + bigMammalPart() + " " + source; k++; break;
                case 1: source = "Blue-" + bigMammalPart() + " " + source; m++; break;
                case 2: source = "Crimson-" + bigMammalPart() + " " + source; c++; break;
                case 3: source = "Green-" + bigMammalPart() + " " + source; l++; break;
                case 4: source = "White-" + bigMammalPart() + " " + source; h++; break;
                case 5: source = "Silver-" + bigMammalPart() + " " + source; t++; break;
                case 6: source = "Black-" + bigMammalPart() + " " + source; s++; break;
                case 7: source = "Golden-" + bigMammalPart() + " " + source; z++; break;
                case 8: source = "Brown-" + bigMammalPart() + " " + source; d++; break;
            }
            createStats(c, m, s, h, k, d, z, l, t);
        }
        //uses built-in Danger
        public string dangerPrint()
        {
            switch (danger) {
                case 0: return "None";
                case 1: case 2: return "Low";
                case 3: case 4: return "Med";
                case 5: case 6: return "High";
                case 7: case 8: return "Danger";
                default: return "DANGER";
            }
        }
        public string dropPrint()
        {
            switch (droprate)
            {
                case 1:	return "RARE";
                case 2: case 3: return "Rare";
                case 4: case 5: return "Unusual";
                case 6: case 7: case 8: return "Uncommon";
                case 9: case 10: case 11: case 12: return "Common";
                default: return "Error";
            }
        }
        public int GetYield()
        {
            return yield + r.Next(yield/2);
        }
    }
}