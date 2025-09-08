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
    }
}