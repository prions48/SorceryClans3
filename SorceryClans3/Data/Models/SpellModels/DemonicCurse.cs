namespace SorceryClans3.Data.Models
{
    public enum CurseBases
    {
        Combat = 0,
        Magic = 1,
        Subtlety = 2,
        Leadership = 3,
        Healing = 4
    }
    public enum CurseTargets
    {
        Population = 0,
        Defenses = 1,
        Sabotage = 2,
        Soldiers = 3,
        Learning = 4
    }
    public class DemonicCurse
    {

        public Guid ID { get; set; }
        public int powerlevel;
        public int attackpower; //the power that the curse deploys after penetration
        public CurseBases cursetype; //type of curse, based on the type of demon  //1==combat, 2==magic, 3==subtlety
        public CurseTargets subtype; //within the curse types, three possible effects for each
                            //combat: 1==defense damage, 2==harms a team, 3==perimeter scout damage
                            //magic: 1==magic damage, 2==sabotage effect, 3==blocks power gain
                            //subtlety: 1==internal damage, 2==poison water supply effect, 3==team disappears
                            //fun twist(?): it's not explicit which effect each curse has? meh....
        public int casttime; //once the team is in position they must cast the curse
        public string cursename;
        public bool detected;
        public string implement; //the name

        //attackpower scaling:
        //1 is a very minimal curse, 40 is mighty, 75 is maximum annihilation

        public DemonicCurse(int pl)
        {
            Random r = new();
            powerlevel = pl;
            cursetype = (CurseBases)r.Next(5);
            subtype = (CurseTargets)r.Next(5);
            cursename = GenerateName(cursetype, powerlevel);
            implement = GenerateImplement(cursetype);
            attackpower = 1 + pl * 2 + r.Next(5);
            int r1 = r.Next(2);
            casttime = 1 + r.Next(10) + r1 * r.Next(10);
            if (casttime > 1)
                attackpower += r.Next(2) + 2;
            if (casttime > 4)
                attackpower += r.Next(3) + 4;
            if (casttime > 7)
                attackpower += r.Next(4) + 6;
            if (casttime > 12)
                attackpower += r.Next(5) + 8;
        }
        public Artifact GenerateArtifact()
        {
            Random r = new();
            int prim = 0, sec = 0;
            for (int i = 0; i < powerlevel; i++)
            {
                if (r.Next(3) > 0)
                    prim++;
                else
                    sec++;
            }
            Artifact artifact = new()
            {
                ArtifactName = $"{ImplementName} of {cursename}",
                Curse = this
            };
            switch (cursetype)
            {
                case CurseBases.Combat:
                    artifact.ComBoost = prim;
                    switch (subtype)
                    {
                        case CurseTargets.Population: artifact.ChaBoost = sec; break;
                        case CurseTargets.Defenses: artifact.SubBoost = sec; break;
                        case CurseTargets.Sabotage: artifact.MagBoost = sec; break;
                        case CurseTargets.Soldiers: artifact.HealBoost = sec; break;
                        case CurseTargets.Learning: artifact.LeadBoost = sec; break;
                    }
                    break;
                case CurseBases.Magic:
                    artifact.MagBoost = prim;
                    switch (subtype)
                    {
                        case CurseTargets.Population: artifact.ChaBoost = sec; break;
                        case CurseTargets.Defenses: artifact.SubBoost = sec; break;
                        case CurseTargets.Sabotage: artifact.MagBoost = sec; break;
                        case CurseTargets.Soldiers: artifact.HealBoost = sec; break;
                        case CurseTargets.Learning: artifact.LeadBoost = sec; break;
                    }
                    break;
                case CurseBases.Subtlety:
                    artifact.SubBoost = prim;
                    switch (subtype)
                    {
                        case CurseTargets.Population: artifact.ChaBoost = sec; break;
                        case CurseTargets.Defenses: artifact.MagBoost = sec; break;
                        case CurseTargets.Sabotage: artifact.TacBoost = sec; break;
                        case CurseTargets.Soldiers: artifact.HealBoost = sec; break;
                        case CurseTargets.Learning: artifact.LeadBoost = sec; break;
                    }
                    break;
                case CurseBases.Leadership:
                    artifact.LeadBoost = prim;
                    switch (subtype)
                    {
                        case CurseTargets.Population: artifact.ChaBoost = sec; break;
                        case CurseTargets.Defenses: artifact.GroupTravelBoost = sec; break;
                        case CurseTargets.Sabotage: artifact.TacBoost = sec; break;
                        case CurseTargets.Soldiers: artifact.HealBoost = sec; break;
                        case CurseTargets.Learning: artifact.LogBoost = sec; break;
                    }
                    break;
                case CurseBases.Healing:
                    artifact.HealBoost = prim;
                    switch (subtype)
                    {
                        case CurseTargets.Population: artifact.ChaBoost = sec; break;
                        case CurseTargets.Defenses: artifact.SubBoost = sec; break;
                        case CurseTargets.Sabotage: artifact.MagBoost = sec; break;
                        case CurseTargets.Soldiers: artifact.ComBoost = sec; break;
                        case CurseTargets.Learning: artifact.LogBoost = sec; break;
                    }
                    break;
            }
            return artifact;
        }
        public string EffectName()
        {
            switch (cursetype)
            {
                case CurseBases.Combat:
                    switch (subtype)
                    {
                        case CurseTargets.Population: return "citizens";
                        case CurseTargets.Defenses: return "walls";
                        case CurseTargets.Sabotage: return "armories";
                        case CurseTargets.Soldiers: return "troops";
                        case CurseTargets.Learning: return "trainers";
                    }
                    break;
                case CurseBases.Magic:
                    switch (subtype)
                    {
                        case CurseTargets.Population: return "children";
                        case CurseTargets.Defenses: return "barriers";
                        case CurseTargets.Sabotage: return "wealth";
                        case CurseTargets.Soldiers: return "sorcerers";
                        case CurseTargets.Learning: return "archmages";
                    }
                    break;
                case CurseBases.Subtlety:
                    switch (subtype)
                    {
                        case CurseTargets.Population: return "secrets";
                        case CurseTargets.Defenses: return "sentinels";
                        case CurseTargets.Sabotage: return "food";
                        case CurseTargets.Soldiers: return "supplies";
                        case CurseTargets.Learning: return "spies";
                    }
                    break;
                case CurseBases.Leadership:
                    switch (subtype)
                    {
                        case CurseTargets.Population: return "trust";
                        case CurseTargets.Defenses: return "guards";
                        case CurseTargets.Sabotage: return "codes";
                        case CurseTargets.Soldiers: return "captains";
                        case CurseTargets.Learning: return "scholars";
                    }
                    break;
                case CurseBases.Healing:
                    switch (subtype)
                    {
                        case CurseTargets.Population: return "health";
                        case CurseTargets.Defenses: return "hospitals";
                        case CurseTargets.Sabotage: return "medicines";
                        case CurseTargets.Soldiers: return "medics";
                        case CurseTargets.Learning: return "surgeons";
                    }
                    break;
            }
            return "";
        }

        public string GetName()
        {
            string ctn;
            if (casttime < 2)
                ctn = "Word";
            else if (casttime < 5)
                ctn = "Hex";
            else if (casttime < 8)
                ctn = "Evocation";
            else if (casttime < 12)
                ctn = "Incantation";
            else
                ctn = "Ritual";
            return ctn + " of " + cursename;
        }

        public string GenerateName(CurseBases ct, int lvl)
        {
            Random r = new();
            if (ct == CurseBases.Combat)
            {
                switch ((int)(r.NextDouble() * 4) + lvl / 3)
                {
                    case 0: return "Teeth";
                    case 1: return "Claws";
                    case 2: return "Stone";
                    case 3: return "Fangs";
                    case 4: return "Spikes";
                    case 5: return "Spears";
                    case 6: return "Chains";
                    case 7: return "Dogs";
                    case 8: return "Swords";
                    case 9: return "Wings";
                    default: return "Blades";
                }
            }
            else if (ct == CurseBases.Magic)
            {
                switch ((int)(r.NextDouble() * 4) + lvl / 3)
                {
                    case 0: return "Hunger";
                    case 1: return "Blood";
                    case 2: return "Poison";
                    case 3: return "Vines";
                    case 4: return "Disease";
                    case 5: return "Melting";
                    case 6: return "Lightning";
                    case 7: return "Flames";
                    case 8: return "Shadows";
                    case 9: return "Storms";
                    default: return "Wasting";
                }
            }
            else if (ct == CurseBases.Subtlety)
            {
                switch ((int)(r.NextDouble() * 4) + lvl / 3)
                {
                    case 0: return "Shadows";
                    case 1: return "Secrets";
                    case 2: return "Darkness";
                    case 3: return "Eyes";
                    case 4: return "Ears";
                    case 5: return "Touch";
                    case 6: return "Silence";
                    case 7: return "Messages";
                    case 8: return "Watchfulness";
                    case 9: return "Blindness";
                    default: return "Betrayal";
                }
            }
            else if (ct == CurseBases.Leadership)
            {
                switch ((int)(r.NextDouble() * 4) + lvl / 3)
                {
                    case 0: return "Lies";
                    case 1: return "Courage";
                    case 2: return "Strife";
                    case 3: return "Greed";
                    case 4: return "Deception";
                    case 5: return "Vanity";
                    case 6: return "Manipulation";
                    case 7: return "Treason";
                    case 8: return "Lust";
                    case 9: return "Conspiracy";
                    default: return "Despair";
                }
            }
            else
            {
                switch ((int)(r.NextDouble() * 4) + lvl / 3)
                {
                    case 0: return "Breath";
                    case 1: return "Blood";
                    case 2: return "Bone";
                    case 3: return "Sleep";
                    case 4: return "Fear";
                    case 5: return "Exhaustion";
                    case 6: return "Hunger";
                    case 7: return "Decay";
                    case 8: return "Infection";
                    case 9: return "Pestilence";
                    default: return "Sanity";
                }
            }
        }

        public string GenerateImplement(CurseBases curse)
        {
            Random r = new();
            if (curse == CurseBases.Combat)
            {
                switch (r.Next(5))
                {
                    case 0: return "dagger";
                    case 1: return "sword";
                    case 2: return "spear";
                    case 3: return "blade";
                    default: return "shield";
                }
            }
            else if (curse == CurseBases.Magic)
            {
                switch (r.Next(5))
                {
                    case 0: return "candle";
                    case 1: return "bell";
                    case 2: return "wand";
                    case 3: return "crystal";
                    default: return "staff";
                }
            }
            else if (curse == CurseBases.Subtlety)
            {
                switch (r.Next(5))
                {
                    case 0: return "mask";
                    case 1: return "box";
                    case 2: return "scarf";
                    case 3: return "vial";
                    default: return "coin";
                }
            }
            else if (curse == CurseBases.Leadership)
            {
                switch (r.Next(5))
                {
                    case 0: return "banner";
                    case 1: return "flag";
                    case 2: return "staff";
                    case 3: return "mask";
                    default: return "gauntlet";
                }
            }
            else //healing
            {
                switch (r.Next(5))
                {
                    case 0: return "bandage";
                    case 1: return "scalpel";
                    case 2: return "mask";
                    case 3: return "vial";
                    default: return "amulet";
                }
            }
        }
        private string ImplementName
        {
            get
            {
                return implement.Substring(0, 1).ToUpper() + implement.Substring(1);
            }
        }
        public DemonicCurse()
        {
            ;//file init
        }
    }
}