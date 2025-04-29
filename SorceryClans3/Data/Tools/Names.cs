using SorceryClans3.Data.Models;

namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        private static Random r = new Random();
        public static string ClanName()
        {
            int syllables = 2 + r.Next(2) + (int)(r.NextDouble()*r.NextDouble()*3);
            string ret = "";
            for (int i = 0; i < syllables; i++)
                ret += Syllable(i==0);
            return ret.NameCase();
        }
        public static string SoldierName()
        {
            int ntemp = 1 + (1-(int)(r.NextDouble()*r.NextDouble()*2)) + (int)(r.NextDouble()*r.NextDouble()*2);
            string temp = "";
            for (int i = 0; i < ntemp; i++)
                temp += Syllable(i==0);
			if (r.NextDouble()>.6)
				switch ((int)(r.NextDouble()*12))
				{
					case 0: case 4: temp = temp + "maru"; break;
					case 1: case 5: temp = temp + "suke"; break;
					case 2: temp = temp + "ro"; break;
					case 3: temp = temp + "shi"; break;
					case 6: temp = temp + "mi"; break;
					case 7: temp = temp + "ta"; break;
					case 8: temp = temp + "na"; break;
					default: temp = temp + "ko"; break;
				}
			else if (r.NextDouble() > .3)
				temp = temp + Syllable();
            return temp.NameCase();
        }
        public static string PowerName(MagicColor color, int lvl, BoostStat prim, BoostStat sec)
        {
            switch (color)
            {
                case MagicColor.None: if (lvl >= 5) return SuperName(prim, sec); return NoneName(prim,sec);
                case MagicColor.Black: return BlackName(lvl, prim, sec);
                case MagicColor.Red: return RedName(prim, sec); 
                case MagicColor.Blue: return BlueName(lvl, prim, sec);
                case MagicColor.White: return WhiteName(prim, sec);
                case MagicColor.Green: return GreenName(prim, sec);
                case MagicColor.Purple: return PurpleName(lvl, prim, sec);
            }
            return "Sharingan";
        }
        
        private static string StyleWay()
        {
            switch (r.Next(4))
            {
                case 0: return "The Way of the ";
                case 1: return "The Path of the ";
                case 2: return "The School of the ";
                default: return "The Style of the ";
            }
        }
        public static string StylePower()
        {
            switch (r.Next(4))
            {
                case 0: return "Being";
                case 1: return "Soul";
                case 2: return "Body";
                default: return "Existence";
            }
        }
        private static string Adjective(SkillStat stat, int boost)
        {
            if (stat == SkillStat.Combat)
            {
                switch (r.Next(7+boost))
                {
                    case 0: return "Blocking";
                    case 1: return "Striking";
                    case 2: return "Dodging";
                    case 3: return "Slashing";
                    case 4: return "Smashing";
                    case 5: return "Crushing";
                    case 6: return "Flowing";
                    case 7: return "Mangling";
                    case 8: return "Devastating";
                    default: return "Invincible";
                }
            }
            else if (stat == SkillStat.Magic)
            {
                switch (r.Next(7+boost))
                {
                    case 0: return"Mystical";
                    case 1: return"Flaming";
                    case 2: return"Arcane";
                    case 3: return"Stormy";
                    case 4: return"Chaotic";
                    case 5: return"Prismatic";
                    case 6: return"Demonic";
                    case 7: return"Necrotic";
                    case 8: return"Cosmic";
                    default: return "Invincible";
                }
            }
            else if (stat == SkillStat.Subtlety)
            {
                switch (r.Next(7+boost))
                {
                    case 0: return "Subtle";
                    case 1: return "Shadowy";
                    case 2: return "Hidden";
                    case 3: return "Deceptive";
                    case 4: return "Elusive";
                    case 5: return "Empty";
                    case 6: return "Illusionary";
                    case 7: return "Many-Faced";
                    case 8: return "Psychic";
                    default: return "Invincible";
                }
            }
            else //healing
            {
                switch (r.Next(5+boost))
                {
                    case 0: return "Healing";
                    case 1: return "Blessed";
                    case 2: return "Caring";
                    case 3: return "Nurturing";
                    case 4: return "Strengthening";
                    default: return "Compassionate";
                }
            }
        }
        private static string Noun(SkillStat stat, int boost)
        {
            if (stat == SkillStat.Combat)
            {
                switch (r.Next(0,7+boost))
                {
                    case 0: return "Fist"; 
                    case 1: return "Sword"; 
                    case 2: return "Axe"; 
                    case 3: return "Shield"; 
                    case 4: return "Crane"; 
                    case 5: return "Mantis"; 
                    case 6: return "Tiger"; 
                    default: return "Dragon";
                }
            }
            else if (stat == SkillStat.Magic)
            {
                switch (r.Next(0,7+boost))
                {
                    case 0: return "Staff";
                    case 1: return "Flame";
                    case 2: return "Wizardry";
                    case 3: return "Witchcraft";
                    case 4: return "Wand";
                    case 5: return "Sky";
                    case 6: return "Sorcery";
                    default: return "Ring";
                }
            }
            else if (stat == SkillStat.Subtlety)
            {
                switch (r.Next(0,7+boost))
                {
                    case 0: return "Knife";
                    case 1: return "Snake";
                    case 2: return "Thief";
                    case 3: return "Monkey";
                    case 4: return "Chameleon";
                    case 5: return "Spider";
                    case 6: return "Skinchanging"; 
                    default: return "Nothingness";
                }
            }
            else //heal
            {
                switch (r.Next(0,4+boost))
                {
                    case 0: return "Scalpel";
                    case 1: return "Scroll";
                    case 2: return "Bandage";
                    case 3: return "Touch";
                    case 4: return "Lantern";
                    default: return "Breath";
                }
            }
        }
        public static string StyleName(SkillStat p, SkillStat s, int lvl)
        {
            return StyleWay() + Adjective(s,lvl) + " " + Noun(p,lvl);
        }
        public static string RankName(int i)
        {
            switch (i)
            {
                case 0: return "Beginner";
                case 1: return "Initiate";
                case 2: return "Novice";
                case 3: return "Pupil";
                case 4: return "Junior";
                case 5: return "Adept";
                case 6: return "Associate";
                case 7: return "Peer";
                case 8: return "Honored";
                case 9: return "Defender";
                case 10: return "Senior";
                case 11: return "Master";
                case 12: return "Champion";
                case 13: return "Headmaster";
                default: return "Grandmaster";
            }
        }
        private static string NameCase(this string s)
        {
            return s.Substring(0,1).ToUpper() + s.Substring(1);
        }
        private static string Syllable(bool first = false)
        {
            
            string tempsyl = "";
			int vowel = r.Next(9);
			switch (vowel) {
				case 0: case 1: case 2: tempsyl = "a"; break;
				case 3: case 4: tempsyl = "o"; break;
				case 5: case 6: tempsyl = "u"; break;
				case 7: tempsyl = "i"; break;
				default: tempsyl = "e"; break; //case==7
			}
            int cons = r.Next(60); //2020: added some
			if ((first || r.NextDouble() > .5) && (vowel <= 6) && (cons > 46))
				cons -= 10;
			if (tempsyl == "a" || tempsyl == "o")
				switch (cons) {
					case 0: case 1: case 2: case 55: case 56: case 57: case 58: break; //no consonant 
					case 3: case 4: case 5: case 36: case 59: tempsyl = "k" + tempsyl; break;
					case 6: case 7: case 8: case 51: tempsyl = "s" + tempsyl; break;
					case 9: case 10: case 11: case 52: tempsyl = "t" + tempsyl; break;
					case 12: case 13: case 14: case 46: case 53: tempsyl = "n" + tempsyl; break;
					case 15: case 16: tempsyl = "h" + tempsyl; break;
					case 17: case 18: tempsyl = "m" + tempsyl; break;
					case 19: case 20: tempsyl = "y" + tempsyl; break;
					case 21: case 22: case 23: tempsyl = "r" + tempsyl; break;
					case 24: case 25: tempsyl = "w" + tempsyl; break;
					case 26: case 27: tempsyl = "g" + tempsyl; break;
					case 28: case 29: case 30: tempsyl = "z" + tempsyl; break;
					case 31: case 32: tempsyl = "d" + tempsyl; break;
					case 33: case 34: tempsyl = "b" + tempsyl; break;
					case 35: tempsyl = "p" + tempsyl; break;
					case 37: tempsyl = "ky" + tempsyl; break;
					case 38: case 39: case 40: tempsyl = "sh" + tempsyl; break;
					case 41: case 42: case 43: case 54: tempsyl = "j" + tempsyl; break;
					case 44: case 45:  tempsyl = "ch" + tempsyl; break;
					//OK I gotta cut down on these damn "y" names
					case 47: if (r.NextDouble()>.5) tempsyl = "my" + tempsyl; else tempsyl = "ry" + tempsyl; break;
					case 48: if (r.NextDouble()>.5) tempsyl = "gy" + tempsyl; else tempsyl = "hy" + tempsyl; break;
					case 49: if (r.NextDouble()>.5) tempsyl = "by" + tempsyl; else tempsyl = "dy" + tempsyl; break;
					case 50: if (r.NextDouble()>.5) tempsyl = "ny" + tempsyl; else tempsyl = "py" + tempsyl; break;
					default: tempsyl = "" + tempsyl; break;
				}
			else if (tempsyl == "u")
				switch (cons) {
					case 0: case 1: case 2: case 55: case 56: case 57: break; //no consonant
					case 3: case 4: case 5: case 54: case 58: tempsyl = "k" + tempsyl; break;
					case 6: case 7: case 8: case 53: case 24: case 59: tempsyl = "s" + tempsyl; break;
					case 9: case 10: case 11: case 18: case 36: case 52: tempsyl = "ts" + tempsyl; break;
					case 12: case 13: case 14: tempsyl = "n" + tempsyl; break;
					case 15: case 16: tempsyl = "f" + tempsyl; break; //no "hu" sound
					case 17: tempsyl = "m" + tempsyl; break;
					case 19: case 20: tempsyl = "y" + tempsyl; break;
					case 21: case 22: case 23: tempsyl = "r" + tempsyl; break;
					case 26: case 27: tempsyl = "g" + tempsyl; break;
					case 28: case 29: case 30: case 51: tempsyl = "z" + tempsyl; break;
					case 31: case 32: tempsyl = "d" + tempsyl; break;
					case 33: case 34: tempsyl = "b" + tempsyl; break;
					case 35: tempsyl = "p" + tempsyl; break;
					case 37: tempsyl = "ky" + tempsyl; break;
					case 38: case 39: case 40: tempsyl = "sh" + tempsyl; break;
					case 41: case 42: case 43: tempsyl = "j" + tempsyl; break;
					case 44: case 45: case 46: tempsyl = "ch" + tempsyl; break;
					case 47: if (r.NextDouble()>.5) tempsyl = "my" + tempsyl; else tempsyl = "ry" + tempsyl; break;
					case 48: if (r.NextDouble()>.5) tempsyl = "gy" + tempsyl; else tempsyl = "hy" + tempsyl; break;
					case 49: if (r.NextDouble()>.5) tempsyl = "by" + tempsyl; else tempsyl = "dy" + tempsyl; break;
					case 50: if (r.NextDouble()>.5) tempsyl = "ny" + tempsyl; else tempsyl = "py" + tempsyl; break;
					default: tempsyl = "" + tempsyl; break;
				}
			else if (tempsyl == "i")
				switch (cons) {
					case 0: case 1: case 2: case 3: case 54: case 55: break; //no consonant
					case 4: case 5: case 6: case 7: case 56: case 57: tempsyl = "k" + tempsyl; break;
					case 8: case 9: case 10: case 11: case 58: tempsyl = "n" + tempsyl; break;
					case 12: case 13: case 14: case 15: tempsyl = "h" + tempsyl; break;
					case 16: case 17: case 18: case 19: tempsyl = "m" + tempsyl; break;
					case 20: case 21: case 22: case 23: tempsyl = "r" + tempsyl; break;
					case 24: case 25: case 26: case 27: tempsyl = "g" + tempsyl; break;
					case 28: case 29: case 30: case 31: tempsyl = "d" + tempsyl; break;
					case 32: case 33: case 34: case 35: tempsyl = "b" + tempsyl; break;
					case 36: case 37: tempsyl = "p" + tempsyl; break;
					case 38: case 39: case 40: case 41: case 42: case 43: tempsyl = "sh" + tempsyl; break;
					case 44: case 45: case 46: case 47: case 48: tempsyl = "j" + tempsyl; break;
					case 49: case 50: case 51: case 52: case 53: case 59: tempsyl = "ch" + tempsyl; break;
					default: tempsyl = "" + tempsyl; break;
				}
			else// if (tempsyl.equals("e"))
				switch (cons) {
					case 0: case 1: case 2: case 3: case 4: case 5: break; //no consonant
					case 6: case 7: case 8: case 9: case 10: case 11: case 26: tempsyl = "k" + tempsyl; break;
					case 12: case 13: case 14: case 15: case 16: tempsyl = "n" + tempsyl; break;
					case 17: case 18: case 19: case 20: case 21: tempsyl = "h" + tempsyl; break;
					case 22: case 23: case 24: case 25: case 51: tempsyl = "m" + tempsyl; break;
					case 27: case 28: case 29: case 30: case 31: case 52: tempsyl = "r" + tempsyl; break;
					case 32: case 33: case 34: case 35: case 36: case 53: tempsyl = "g" + tempsyl; break;
					case 37: case 38: case 39: case 40: case 41: case 54: tempsyl = "d" + tempsyl; break;
					case 42: case 43: case 44: case 45: case 46: tempsyl = "b" + tempsyl; break;
					case 47: case 48: case 49: case 50: tempsyl = "p" + tempsyl; break;
					case 55: case 56: case 57: case 58: tempsyl = "t" + tempsyl; break;
					default: break; //no consonant
				}
			if (r.NextDouble() > .9)
				tempsyl = tempsyl + "n";
			return tempsyl;
        }
        public static string DemonName()
        {
            string dname = "";
            int c1 = r.Next(18);
            int v1 = r.Next(10);
            switch (c1) {
            case 0: dname = "B"; break;
            case 1: dname = "D"; break;
            case 2: dname = "X"; break;
            case 3: dname = "K"; break;
            case 4: dname = "M"; break;
            case 5: dname = "R"; break;
            case 6: dname = "T"; break;
            case 7: dname = "J"; break;
            case 8: dname = "Z"; break;
            case 9: dname = "G"; break;
            case 10: dname = "V"; break;
            case 11: dname = "Th"; break;
            case 12: dname = "P"; break;
            case 13: dname = "Y"; break;
            case 14: dname = "H"; break;
            case 15: case 16: case 17: break; //no first consonant
            }
            switch (v1) {
            case 0: case 1: case 2: case 3: case 4: dname = dname + "a"; break;
            case 5: dname = dname + "e"; break;
            case 6: dname = dname + "i"; break;
            case 7: dname = dname + "o"; break;
            case 8: dname = dname + "u"; break;
            case 9: if (r.NextDouble() > .3) dname = dname + "o";
                    else if (r.NextDouble() > .5) dname = dname + "e";
                    else dname = dname + "i"; break;
            }
            if (c1 >= 15) //no first consonant
                dname = dname.Substring(0,1).ToUpper() + dname.Substring(1);
            int e1 = (int)(r.NextDouble()*15);
            switch (e1) {
            case 0: dname = dname + "b"; break;
            case 1: case 8: case 9: dname = dname + "g"; break;
            case 2: case 10: case 11: dname = dname + "l"; break;
            case 3: case 12: dname = dname + "n"; break;
            case 4: dname = dname + "m"; break;
            case 5: dname = dname + "s"; break;
            case 6: case 13: dname = dname + "r"; break;
            case 7: case 14: dname = dname + "z"; break;
            }
            if (r.NextDouble() > .17) //1 in 6 chance of no middle syllable
            {
                int c2 = (int)(r.NextDouble()*12);
                int v2 = (int)(r.NextDouble()*13);
                switch (c2) {
                case 0: case 1: case 2: case 3: dname = dname + "r"; break;
                case 4: case 5: dname = dname + "m"; break;
                case 6: dname = dname + "z"; break;
                case 7: case 8: case 9: case 10: dname = dname + "g"; break;
                case 11: dname = dname + "th"; break;
                }
                if (c2 >= 7 && r.NextDouble() > .5)
                    dname = dname + "r";
                switch (v2) {
                case 0: case 4: case 5: dname = dname + "a"; break;
                case 1: case 6: case 7: dname = dname + "i"; break;
                case 2: case 8: case 9: dname = dname + "o"; break;
                case 3: case 10: case 11: dname = dname + "u"; break;
                case 12: dname = dname + "e"; break;
                }
            }
            int c3 = (int)(r.NextDouble()*16);
            int v3 = (int)(r.NextDouble()*10);
            int e3 = (int)(r.NextDouble()*12);
            switch (c3) {
            case 0: dname = dname + "m"; break;
            case 1: dname = dname + "x"; break;
            case 2: case 11: dname = dname + "n"; break;
            case 3: case 12: dname = dname + "r"; break;
            case 4: case 13: dname = dname + "g"; break;
            case 5: dname = dname + "tr"; break;
            case 6: dname = dname + "ll"; break;
            case 7: dname = dname + "d"; break;
            case 8: case 14: dname = dname + "n"; break;
            case 9: dname = dname + "v"; break;
            case 10: case 15: dname = dname + "nn"; break;
            }
            switch (v3) {
            case 0: case 5: case 6: dname = dname + "a"; break;
            case 1: dname = dname + "e"; break;
            case 2: dname = dname + "i"; break;
            case 3: case 7: case 8: dname = dname + "o"; break;
            case 4: case 9: dname = dname + "u"; break;
            }
            if (r.NextDouble() < .1 && v3 != 2) //doubled, no ii
            {
                switch (v3) { //same seed
                case 0: case 5: case 6: dname = dname + "a"; break;
                case 1: dname = dname + "e"; break;
                case 2: dname = dname + "i"; break;
                case 3: case 7: case 8: dname = dname + "o"; break;
                case 4: case 9: dname = dname + "u"; break;
                }
            }
            switch (e3) {
            case 0: dname = dname + "l"; break;
            case 1: dname = dname + "d"; break;
            case 2: dname = dname + "n"; break;
            case 3: dname = dname + "s"; break;
            case 4: dname = dname + "r"; break;
            case 5: case 9: dname = dname + "th"; break;
            case 6: dname = dname + "k"; break;
            case 7: case 10: dname = dname + "g"; break;
            case 8: case 11: dname = dname + "z"; break;
            }
            return dname;
        }
    }
}