using SorceryClans3.Data.Models;

namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string AngelName()
        {
            string aname = "";
            Random r = new();
            bool thicc = false; //heavy start to the name, guarantees no first consonant on second syllable
            int s1 = r.Next(32); //first consonant
            int s2 = r.Next(10); //first vowel
            switch (s1)
            {
                case 0: aname = aname + "B"; break;
                case 1:
                    if (r.NextDouble() > .5) aname = aname + "D";
                    else aname = aname + "F"; break;
                case 2: aname = aname + "Ch"; break;
                case 3: aname = aname + "G"; break;
                case 4: aname = aname + "H"; break;
                case 5: aname = aname + "J"; break;
                case 6:
                    if (r.NextDouble() > .5) aname = aname + "K";
                    else aname = aname + "W"; break;
                case 7: aname = aname + "L"; break;
                case 8: aname = aname + "M"; break;
                case 9: aname = aname + "N"; break;
                case 10: aname = aname + "P"; break;
                case 11: aname = aname + "Qu"; break;
                case 12: aname = aname + "R"; break;
                case 13: aname = aname + "S"; break;
                case 14: aname = aname + "Sh"; break;
                case 15: aname = aname + "T"; break;
                case 16:
                    aname = aname + "T"; if (r.NextDouble() < .5) aname = aname + "r"; //Tr x2 over Ts and Tw
                    else if (r.NextDouble() < .5) aname = aname + "s";
                    else aname = aname + "w"; thicc = true; break;
                case 17: aname = aname + "V"; break;
                case 19: aname = aname + "X"; break;
                case 20: aname = aname + "Y"; break;
                case 21: aname = aname + "Z"; break;
                case 22: aname = aname + "Th"; break;
                case 23: aname = aname + "Ph"; thicc = true; break;
                case 24:
                    aname = aname + "G"; if (r.NextDouble() > .8) aname = aname + "w";
                    else if (r.NextDouble() > .5) aname = aname + "r";
                    else aname = aname + "l"; thicc = true; break;
                case 25:
                    aname = aname + "K"; if (r.NextDouble() > .5) aname = aname + "r";
                    else aname = aname + "l"; break;
                case 26: aname = aname + "Fr"; thicc = true; break;
                case 27: aname = aname + "Br"; thicc = true; break;
                case 28: aname = aname + "Dr"; break;
                case 29: aname = aname + "Sw"; thicc = true; break;
                case 18:
                case 30:
                case 31:
                    if (r.NextDouble() > .3) aname = aname + "A";
                    else if (r.NextDouble() > .5) aname = aname + "E";
                    else if (r.NextDouble() > .6) aname = aname + "I";
                    else if (r.NextDouble() > .5) aname = aname + "U";
                    else aname = aname + "O"; s2 = 11; break;
            }
            switch (s2)
            {
                case 0: case 1: case 2: case 3: case 4: aname = aname + "a"; break;
                case 5: case 6: aname = aname + "e"; break;
                case 7: aname = aname + "i"; break;
                case 8: aname = aname + "o"; break;
                case 9:
                    if (r.NextDouble() > .4) aname = aname + "o";
                    else aname = aname + "u"; break;
                default: break;
            }
            int s3 = (int)(r.NextDouble() * 15); //end first consonant
            switch (s3)
            {
                case 0: aname = aname + "m"; break;
                case 1: if (s1 != 23) aname = aname + "ph"; else aname = aname + "r"; break;
                case 2: if (r.NextDouble() > .5) aname = aname + "a"; else aname = aname + "i"; break;
                case 4: if (s1 != 14) aname = aname + "sh"; else aname = aname + "l"; break;
                case 5: if (s1 != 11) aname = aname + "qu"; else aname = aname + "r"; break;
                case 6: if (s1 != 8) aname = aname + "m"; else aname = aname + "z"; break;
                case 7: if (s1 != 21) aname = aname + "z"; else aname = aname + "m"; break;
                case 8: if (s1 != 22) aname = aname + "th"; else aname = aname + "l"; break;
                case 9: if (s1 != 0) aname = aname + "b"; else aname = aname + "l"; break;
                case 10: if (s1 != 6) aname = aname + "k"; else aname = aname + "z"; break;
                case 11: if (s1 != 15) aname = aname + "t"; else aname = aname + "r"; break;
                case 12: if (s1 != 12) aname = aname + "r"; else aname = aname + "l"; break;
                case 13: if (s1 != 26) aname = aname + "f"; else aname = aname + "l"; break;
                case 14: aname = aname + "n"; break;
                default: break;
            }
            int s4 = r.Next(20); //second consonant
            if (s3 == 5 || thicc)
                s4 = 10;
            switch (s4)
            {
                case 0: aname = aname + "l"; break;
                case 1: aname = aname + "c"; break;
                case 2: if (s3 != 4) aname = aname + "sh"; break;
                case 3: aname = aname + "m"; break;
                case 4: case 5: aname = aname + "r"; break;
                case 6: aname = aname + "k"; break;
                case 7: aname = aname + "p"; break;
                case 8: aname = aname + "n"; break;
                case 9: aname = aname + "v"; break;
                default: break; //50% chance of no second consonant
            }
            int s5 = r.Next(4);
            switch (s5)
            {
                case 0: case 1: aname = aname + "a"; break;
                case 2: aname = aname + "e"; break;
                case 3: aname = aname + "i"; break;
            }
            int s6 = r.Next(10);
            switch (s6)
            {
                case 0: case 1: aname = aname + "l"; break;
                case 2: case 3: aname = aname + "n"; break;
                case 4: aname = aname + "ch"; break;
                case 5: aname = aname + "v"; break;
                case 6: if (s3 != 8) aname = aname + "th"; else aname = aname + "n"; break;
                case 7: if (s3 != 13) aname = aname + "f"; else aname = aname + "l"; break;
                case 8: aname = aname + "z"; break;
                case 9: aname = aname + "r"; break;
            }
            if (r.NextDouble() > .9 && s6 != 6 && s6 != 4) //removes --thtael and chtiel
                aname = aname + "t";
            if (r.NextDouble() > .6)
                aname = aname + "ael";
            else
                aname = aname + "iel";
            return aname;
        }
        //later, turn all these into random gnerators
        //also later, add level numbers so scopes can sound more powerful at higher levels
        public static string GetScope(AngelScope first, AngelScope second)
        {
            switch (first)
            {
                case AngelScope.Combat:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Blades";
                        case AngelScope.Magic: return "Lightning";
                        case AngelScope.Subtlety: return "Blinding";
                        case AngelScope.Heal: return "Sinews";
                        case AngelScope.Travel: return "Chariots";
                        default: return "";
                    }
                case AngelScope.Magic:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Flames";
                        case AngelScope.Magic: return "Crystals";
                        case AngelScope.Subtlety: return "Shadows";
                        case AngelScope.Heal: return "Vitality";
                        case AngelScope.Travel: return "Windwalking";
                        default: return "";
                    }
                case AngelScope.Subtlety:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Heartbeats";
                        case AngelScope.Magic: return "Windspeaking";
                        case AngelScope.Subtlety: return "Shadows";
                        case AngelScope.Heal: return "Breath";
                        case AngelScope.Travel: return "Silence";
                        default: return "";
                    }
                case AngelScope.Heal:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Bones";
                        case AngelScope.Magic: return "Blood";
                        case AngelScope.Subtlety: return "Skin";
                        case AngelScope.Heal: return "Life";
                        case AngelScope.Travel: return "Endurance";
                        default: return "";
                    }
                case AngelScope.Travel:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Marching";
                        case AngelScope.Magic: return "Flight";
                        case AngelScope.Subtlety: return "Feet";
                        case AngelScope.Heal: return "Vigor";
                        case AngelScope.Travel: return "Roadways";
                        default: return "";
                    }
                case AngelScope.Leadership:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Courage";
                        case AngelScope.Magic: return "Harmony";
                        case AngelScope.Subtlety: return "Timing";
                        case AngelScope.Heal: return "Compassion";
                        case AngelScope.Travel: return "Journeys";
                        default: return "";
                    }
                case AngelScope.Charisma:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Banners";
                        case AngelScope.Magic: return "Summoning";
                        case AngelScope.Subtlety: return "Deception";
                        case AngelScope.Heal: return "Faith";
                        case AngelScope.Travel: return "Encouragement";
                        default: return "";
                    }
                case AngelScope.Logistics:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Iron";
                        case AngelScope.Magic: return "Gathering";
                        case AngelScope.Subtlety: return "Patience";
                        case AngelScope.Heal: return "Medicine";
                        case AngelScope.Travel: return "Maps";
                        default: return "";
                    }
                case AngelScope.Tactics:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Formations";
                        case AngelScope.Magic: return "Elements";
                        case AngelScope.Subtlety: return "Doorways";
                        case AngelScope.Heal: return "Closing";
                        case AngelScope.Travel: return "Ambushes";
                        default: return "";
                    }
                case AngelScope.Teaching:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Swordsmanship";
                        case AngelScope.Magic: return "Knowledge";
                        case AngelScope.Subtlety: return "Deception";
                        case AngelScope.Heal: return "Knitting";
                        case AngelScope.Travel: return "Singing";
                        default: return "";
                    }
                case AngelScope.CounterIntel:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Analysis";
                        case AngelScope.Magic: return "Mindreading";
                        case AngelScope.Subtlety: return "Liespotting";
                        case AngelScope.Heal: return "Scars";
                        case AngelScope.Travel: return "Pursuit";
                        default: return "";
                    }
                case AngelScope.Research:
                    switch (second)
                    {
                        case AngelScope.Combat: return "Technique";
                        case AngelScope.Magic: return "Study";
                        case AngelScope.Subtlety: return "Secrets";
                        case AngelScope.Heal: return "Surgery";
                        case AngelScope.Travel: return "Measurement";
                        default: return "";
                    }
                default: return "";
            }
        }
    }
}