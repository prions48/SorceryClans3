namespace SorceryClans3.Data.Models
{
    public class GreaterSpirit
    {
        public Guid ID { get; set; }
        public Artifact Artifact { get; set; }
        public Soldier Spirit { get; set; }
        public GreaterSpirit(int rank)
        {
            Random r = new Random();
            ID = Guid.NewGuid();
            int seed = rank / 2;
            string ntype, etype; //control, nature, element
            int cb, mb, sb, kb;
            int tough, newhp;
            int topart, topsol;
            int prim = 0, sec = 0, tert = 0;
            do
            {
                cb = 0; mb = 0; sb = 0; kb = 0; tough = 0; newhp = 0; prim = 0; sec = 0; tert = 0;
                for (int i = 0; i <= (int)(rank * 1.4 + 10); i++)
                {
                    if (r.NextDouble() < .5)
                        prim++;
                    else if (r.NextDouble() < .6)
                        sec++;
                    else
                        tert++;
                }
                if (r.NextDouble() < .15) //heal
                {
                    switch (r.Next(3) + rank / 7)
                    {
                        case 0: ntype = "Cat"; kb += prim; sb += sec; mb += tert; break;
                        case 1: ntype = "Lynx"; kb += prim; cb += sec; mb += tert; break;
                        case 2: ntype = "Dryad"; sb += prim; mb += sec; cb += tert; break;
                        case 3: ntype = "Kappa"; kb += prim; cb += sec; mb += tert; break;
                        case 4: ntype = "Equinal"; mb += prim; cb += sec; sb += tert; break;
                        case 5: ntype = "Ursinal"; cb += prim; mb += sec; sb += tert; tough++; break;
                        default: ntype = "Naraka"; kb += prim; mb += sec; sb += tert; tough++; break;
                    }
                    switch (r.Next(3))
                    {
                        case 0: etype = "Water"; sb += seed; break;
                        case 1: etype = "Life"; mb += seed; break;
                        default: etype = "Wood"; cb += seed; tough++; break;
                    }
                }
                else
                {
                    switch (r.Next(10) + rank / 5) //rank should only be like up to 20
                    {
                        case 0: case 1: ntype = "Condor"; sb += prim; cb += sec; mb += tert; break;
                        case 2: ntype = "Harpy"; sb += prim; cb += sec; mb += tert; break;
                        case 3: ntype = "Rakshasa"; mb += prim; sb += sec; cb += tert; break;
                        case 4: ntype = "Cerberus"; cb += prim; mb += sec; sb += tert; break;
                        case 5: ntype = "Asura"; mb += prim; cb += sec; sb += tert; break;
                        case 6: ntype = "Titan"; cb += prim; mb += sec; sb += tert; tough++; break;
                        case 7: ntype = "Hydra"; mb += prim; cb += sec; sb += tert; tough++; break;
                        case 8: ntype = "Kraken"; cb += prim; sb += sec; mb += tert; tough++; break;
                        case 9: ntype = "Beholder"; sb += prim; mb += sec; cb += tert; break;
                        case 10: ntype = "Reaper"; sb += prim; mb += sec; cb += tert; break;
                        case 11: ntype = "Garuda"; mb += prim; cb += sec; sb += tert; break;
                        case 12: ntype = "Kirin"; cb += prim; mb += sec; sb += tert; break;
                        default: ntype = "Dragon"; cb += prim; mb += sec; sb += tert; tough++; break;
                    }
                    switch (r.Next(15))
                    {
                        case 0: etype = "Fire"; cb += seed; break;
                        case 1: etype = "Shadow"; sb += seed; tough++; break;
                        case 2: etype = "Water"; mb += seed; break;
                        case 3: etype = "Air"; sb += seed; break;
                        case 4: etype = "Stone"; cb += seed; tough++; break;
                        case 5: etype = "Blood"; mb += seed; tough++; break;
                        case 6: etype = "Lightning"; mb += seed; break;
                        case 7: etype = "Poison"; cb += seed; break;
                        case 8: etype = "Crystal"; mb += seed; break;
                        case 9: etype = "Steel"; cb += seed; tough++; break;
                        case 10: etype = "Sand"; sb += seed; tough++; break;
                        case 11: etype = "Wood"; sb += seed; break;
                        case 12: etype = "Iron"; cb += seed; break;
                        case 13: etype = "Lava"; mb += seed; tough++; break;
                        default: etype = "Dust"; sb += seed; break;
                    }
                }

                Artifact = new Artifact(rank);
                newhp = 4 + r.Next(3) + rank / 5 * (tough + 1) + tough * 3;
                //return true on pwr to signal greater spirit
                if (kb > 0)
                    topsol = 0;
                else if (cb >= mb && cb >= sb)
                    topsol = 1;
                else if (mb >= cb && mb >= sb)
                    topsol = 2;
                else //if (sb >= cb && sb >= mb)
                    topsol = 3;
                if (Artifact.HealBoost > 0)
                    topart = 0;
                else if (Artifact.ComBoost >= Artifact.MagBoost && Artifact.ComBoost >= Artifact.SubBoost)
                    topart = 1;
                else if (Artifact.MagBoost >= Artifact.ComBoost && Artifact.MagBoost >= Artifact.SubBoost)
                    topart = 2;
                else //if (sb >= cb && sb >= mb)
                    topart = 3;
            } while (topsol != topart);
            Spirit = new Soldier()
            {
                Type = SoldierType.GreaterSpirit,
                ClanName = etype,
                GivenName = ntype,
                PowerLevel = 1,
                ComBase = cb,
                MagBase = mb,
                SubBase = sb,
                HPBase = newhp,
                HPCurrent = newhp,
                TravelBase = null,
                Medical = new Medical()
                {
                    HealBase = kb,
                    MedicalPowerBase = kb > 10 ? 100 : 10 * kb,
                    Assessed = true,
                    Trained = kb > 0
                }
            };
            Artifact.SpiritSoldier = Spirit;
        }
    }
}