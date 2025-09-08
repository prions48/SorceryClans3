using SorceryClans3.Data.Models;

namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
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
        private static string Adjective(SkillStat stat, MagicColor color)
        {
            switch (stat)
            {
                case SkillStat.Combat:
                    switch (color)
                    {
                        case MagicColor.Black: return "Deathly";
                        case MagicColor.Blue: return "Flowing";
                        case MagicColor.Red: return "Wicked";
                        case MagicColor.Green: return "Feral";
                        case MagicColor.White: return "Shining";
                        case MagicColor.Purple: return "Terrible";
                        default: return "Mysterious";
                    }
                case SkillStat.Magic:
                    switch (color)
                    {
                        case MagicColor.Black: return "Occult";
                        case MagicColor.Blue: return "Swirling";
                        case MagicColor.Red: return "Burning";
                        case MagicColor.Green: return "Growing";
                        case MagicColor.White: return "Sparkling";
                        case MagicColor.Purple: return "Magnificent";
                        default: return "Mysterious";
                    }
                case SkillStat.Subtlety:
                    switch (color)
                    {
                        case MagicColor.Black: return "Ghostly";
                        case MagicColor.Blue: return "Ethereal";
                        case MagicColor.Red: return "Shadowy";
                        case MagicColor.Green: return "Prowling";
                        case MagicColor.White: return "Silver";
                        case MagicColor.Purple: return "Illusionary";
                        default: return "Mysterious";
                    }
                case SkillStat.Heal:
                    switch (color)
                    {
                        case MagicColor.Black: return "Vampiric";
                        case MagicColor.Blue: return "Energetic";
                        case MagicColor.Red: return "Cursed";
                        case MagicColor.Green: return "Nourishing";
                        case MagicColor.White: return "Holy";
                        case MagicColor.Purple: return "Glamoured";
                        default: return "Mysterious";
                    }
                default: return "Mysterious";
            }
        }
        private static string Adjective(SkillStat stat, int boost)
        {
            if (stat == SkillStat.Combat)
            {
                switch (r.Next(7 + boost))
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
                switch (r.Next(7 + boost))
                {
                    case 0: return "Mystical";
                    case 1: return "Flaming";
                    case 2: return "Arcane";
                    case 3: return "Stormy";
                    case 4: return "Chaotic";
                    case 5: return "Prismatic";
                    case 6: return "Demonic";
                    case 7: return "Necrotic";
                    case 8: return "Cosmic";
                    default: return "Invincible";
                }
            }
            else if (stat == SkillStat.Subtlety)
            {
                switch (r.Next(7 + boost))
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
                switch (r.Next(5 + boost))
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
                    case 1: return "Book";
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
        public static string StyleName(SkillStat p, SkillStat s, int lvl, MagicColor? color)
        {
            if (color != null)
                return StyleWay() + Adjective(s, color.Value) + " " + Noun(p, lvl);
            return StyleWay() + Adjective(s, lvl) + " " + Noun(p, lvl);
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
    }
}