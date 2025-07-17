namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
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