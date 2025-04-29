using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public enum ArtIcon
    {
        Sword,
        Axe,
        Hammer,
        Spear,
        Sickle,
        Shield,
        MagicShield,
        Helmet,
        Wand,
        Knife,
        Rope,
        Jewel,
        Clothing,
        Necklace,
        BowArrow,
        Bell,
        Candle
    }
    public class Artifact
    {
        public Guid ID { get; set; }
        public string ArtifactName { get; set; }
        public int ComBoost { get; set; }
        public int MagBoost { get; set; }
        public int SubBoost { get; set; }
        public int HPBoost { get; set; }
        public int HealBoost { get; set; }
        public Artifact()
        {
            ID = Guid.NewGuid();
            SetStats();
            ArtifactName = SetArtifactName();
        }
        public Artifact(int lvl)
        {
            SetStats(lvl);
            ArtifactName = SetArtifactName();
        }
        public Artifact(int lvl, string spirit)
        {
            SetStats(lvl/2);
            ArtifactName = SetSpiritName(spirit);
        }
        private string SetArtifactName()
        {
            Artifact art = this;
            if ((art.ComBoost == art.MagBoost && art.ComBoost > 0) || (art.MagBoost == art.SubBoost && art.MagBoost > 0)
                || (art.ComBoost == art.SubBoost && art.ComBoost > 0))
            {
                ArtIcon = ArtIcon.Hammer;
                return "The Mace of Balance";
            }
            if (art.HealBoost > 0)
            {
                if (art.ComBoost > 0)
                {
                    ArtIcon = ArtIcon.Sword;
                    return "The Blade of the Surgeon";
                }
                if (art.MagBoost > 0)
                {
                    ArtIcon = ArtIcon.Wand;
                    return "The Rod of Vitality";
                }
                if (art.SubBoost > 0)
                {
                    ArtIcon = ArtIcon.Rope;
                    return "The Rope of the Nightingale";
                }
                ArtIcon = ArtIcon.Wand;
                return "The Staff of Healing";
            }
            if (art.ComBoost > art.MagBoost && art.MagBoost > art.SubBoost)
            {
                if (art.MagBoost > 0)
                {
                    ArtIcon = ArtIcon.Axe;
                    return "The Axe of the Moon";
                }
                if (art.SubBoost > 0)
                {
                    ArtIcon = ArtIcon.Knife;
                    return "The Dagger of Slaying";
                }
                ArtIcon = ArtIcon.Spear;
                return "The Glaive of Pain";
            }
            if (art.MagBoost > art.ComBoost && art.MagBoost > art.SubBoost)
            {
                if (art.ComBoost > 0)
                {
                    ArtIcon = ArtIcon.Sword;
                    return "The Mageblade of the Vortex";
                }
                if (art.SubBoost > 0)
                {
                    ArtIcon = ArtIcon.Clothing;
                    return "The Cloak of Storms";
                }
                ArtIcon = ArtIcon.Wand;
                return "The Staff of the Archmage";
            }
            if (art.ComBoost > 0)
            {
                ArtIcon = ArtIcon.Sickle;
                return "The Rapier of Silence";
            }
            if (art.MagBoost > 0)
            {
                ArtIcon = ArtIcon.Candle;
                return "The Candle of Power";
            }
            ArtIcon = ArtIcon.Clothing;
            return "The Cloak of Invisibility";
        }
        private void SetStats(int lvl = 1)
        {
            Random r = new Random();
            int prim = lvl * 2;
            int sec = lvl;
            int bonus = r.Next(6)+2;
            for (int i = 0; i < bonus; i++)
            {
                if (r.NextDouble() < .7)
                    prim++;
                else
                    sec++;
            }
            int type = r.Next(4);
            switch (type)
            {
                case 0: ComBoost = prim; break;
                case 1: MagBoost = prim; break;
                case 2: SubBoost = prim; break;
                case 3: HealBoost = prim; break;
            }
            if (type == 3)
                type = r.Next(4);
            else
                type = r.Next(3);
            switch (type)
            {
                case 0: ComBoost += sec; break;
                case 1: MagBoost += sec; break;
                case 2: SubBoost += sec; break;
                case 3: HealBoost += sec; break;
            }
        }
        private string SetSpiritName(string spirit)
        {
            Artifact art = this;
            if ((art.ComBoost == art.MagBoost && art.ComBoost > 0) || (art.MagBoost == art.SubBoost && art.MagBoost > 0)
                || (art.ComBoost == art.SubBoost && art.ComBoost > 0))
            {
                ArtIcon = ArtIcon.Hammer;
                return "The Mace of the " + spirit;
            }
            if (art.HealBoost > 0)
            {
                if (art.ComBoost > 0)
                {
                    ArtIcon = ArtIcon.Sword;
                    return "The Scalpel of the " + spirit;
                }
                if (art.MagBoost > 0)
                {
                    ArtIcon = ArtIcon.Wand;
                    return "The Rod of the " + spirit;
                }
                if (art.SubBoost > 0)
                {
                    ArtIcon = ArtIcon.Rope;
                    return "The Rope of the " + spirit;
                }
                ArtIcon = ArtIcon.Wand;
                return "The Staff of the " + spirit;
            }
            if (art.ComBoost > art.MagBoost && art.MagBoost > art.SubBoost)
            {
                if (art.MagBoost > 0)
                {
                    ArtIcon = ArtIcon.Axe;
                    return "The Axe of the " + spirit;
                }
                if (art.SubBoost > 0)
                {
                    ArtIcon = ArtIcon.Knife;
                    return "The Dagger of the " + spirit;
                }
                ArtIcon = ArtIcon.Spear;
                return "The Glaive of the " + spirit;
            }
            if (art.MagBoost > art.ComBoost && art.MagBoost > art.SubBoost)
            {
                if (art.ComBoost > 0)
                {
                    ArtIcon = ArtIcon.Sword;
                    return "The Mageblade of the " + spirit;
                }
                if (art.SubBoost > 0)
                {
                    ArtIcon = ArtIcon.Clothing;
                    return "The Cloak of the " + spirit;
                }
                ArtIcon = ArtIcon.Wand;
                return "The Staff of the " + spirit;
            }
            if (art.ComBoost > 0)
            {
                ArtIcon = ArtIcon.Sickle;
                return "The Rapier of the " + spirit;
            }
            if (art.MagBoost > 0)
            {
                ArtIcon = ArtIcon.Candle;
                return "The Candle of the " + spirit;
            }
            ArtIcon = ArtIcon.Clothing;
            return "The Cloak of the " + spirit;
        }
        private ArtIcon ArtIcon { get; set; } = ArtIcon.Sword;
        public string Icon
        {
            get
            {
                switch (ArtIcon)
                {
                    case ArtIcon.Sword: return "<svg viewBox=\"0 0 24 24\"><path d=\"M6.92,5H5L14,14L15,13.06M19.96,19.12L19.12,19.96C18.73,20.35 18.1,20.35 17.71,19.96L14.59,16.84L11.91,19.5L10.5,18.09L11.92,16.67L3,7.75V3H7.75L16.67,11.92L18.09,10.5L19.5,11.91L16.83,14.58L19.95,17.7C20.35,18.1 20.35,18.73 19.96,19.12Z\" /></svg>";
                    case ArtIcon.Axe: return "<svg viewBox=\"0 0 24 24\"><title>axe-battle</title><path d=\"M21.47 12.43C19.35 14.55 15.82 13.84 15.82 13.84V9.6L3.41 22L2 20.59L14.4 8.18H10.16C10.16 8.18 9.45 4.65 11.57 2.53C13.69 .406 17.23 1.11 17.23 1.11V5.36L17.94 4.65L19.35 6.06L18.64 6.77H22.89C22.89 6.77 23.59 10.31 21.47 12.43Z\" /></svg>";
                    case ArtIcon.Hammer: return "<svg viewBox=\"0 0 24 24\"><title>hammer</title><path d=\"M2 19.63L13.43 8.2L12.72 7.5L14.14 6.07L12 3.89C13.2 2.7 15.09 2.7 16.27 3.89L19.87 7.5L18.45 8.91H21.29L22 9.62L18.45 13.21L17.74 12.5V9.62L16.27 11.04L15.56 10.33L4.13 21.76L2 19.63Z\" /></svg>";
                    case ArtIcon.Spear: return "<svg viewBox=\"0 0 24 24\"><title>spear</title><path d=\"M16 9H16.41L3.41 22L2 20.59L15 7.59V9H16M16 4V8H20L22 2L16 4Z\" /></svg>";
                    case ArtIcon.Sickle: return "<svg viewBox=\"0 0 24 24\"><title>sickle</title><path d=\"M19.3 7.2C17.5 4.7 14.9 3 12 2C26.2 10.5 15.4 22.9 8.5 15.5L5.9 16L2.5 19.4C1.9 20 1.9 21 2.5 21.5C3.1 22.1 4.1 22.1 4.6 21.5L7.8 18.3C15.3 24.3 25 15 19.3 7.2Z\" /></svg>";
                    case ArtIcon.Shield: return "<svg viewBox=\"0 0 24 24\"><title>shield</title><path d=\"M12,1L3,5V11C3,16.55 6.84,21.74 12,23C17.16,21.74 21,16.55 21,11V5L12,1Z\" /></svg>";
                    case ArtIcon.MagicShield: return "<svg viewBox=\"0 0 24 24\"><title>shield-cross</title><path d=\"M12,1L3,5V11C3,16.5 6.8,21.7 12,23C17.2,21.7 21,16.5 21,11V5L12,1M16,10H13V18H11V10H8V8H11V5H13V8H16V10Z\" /></svg>";
                    case ArtIcon.Helmet: return "<svg viewBox=\"0 0 24 24\"><title>racing-helmet</title><path d=\"M2.2,11.2C2,13.6 2.7,15.6 4.2,17.4C5.7,19.2 7.7,20 10.1,20H20.1C20.6,20 21.1,19.8 21.5,19.4C21.9,19 22.1,18.5 22.1,18V17.2C22.1,16.6 22,15.9 21.9,15H13.7C12.7,15 11.9,14.6 11.2,13.9C10.5,13.2 10.1,12.3 10.1,11.4C10.1,9.8 10.8,8.7 12.3,8.1L17.1,6C15.4,4.8 13.4,4.1 11.1,4C8.9,3.8 6.9,4.5 5.1,5.9C3.3,7.3 2.4,9 2.2,11.2M12.1,11.4C12.1,11.8 12.3,12.2 12.6,12.5C12.9,12.8 13.3,13 13.7,13H21.5C20.9,10.8 20,9 18.7,7.6L13.1,9.9C12.4,10.1 12.1,10.6 12.1,11.4Z\" /></svg>";
                    case ArtIcon.Wand: return "<svg viewBox=\"0 0 24 24\"><title>magic-staff</title><path d=\"M17.5 9C16.12 9 15 7.88 15 6.5S16.12 4 17.5 4 20 5.12 20 6.5 18.88 9 17.5 9M14.43 8.15L2 20.59L3.41 22L15.85 9.57C15.25 9.24 14.76 8.75 14.43 8.15M13 5L13.63 3.63L15 3L13.63 2.37L13 1L12.38 2.37L11 3L12.38 3.63L13 5M21 5L21.63 3.63L23 3L21.63 2.37L21 1L20.38 2.37L19 3L20.38 3.63L21 5M21 9L20.38 10.37L19 11L20.38 11.63L21 13L21.63 11.63L23 11L21.63 10.37L21 9Z\" /></svg>";
                    case ArtIcon.Knife: return "<svg viewBox=\"0 0 24 24\"><title>knife-military</title><path d=\"M22,2L17.39,3.75L10.46,10.68L14,14.22L20.92,7.29C22.43,5.78 22,2 22,2M8.33,10L6.92,11.39L8.33,12.8L2.68,18.46L6.21,22L11.87,16.34L13.28,17.76L14.7,16.34L8.33,10Z\" /></svg>";
                    case ArtIcon.Rope: return "<svg viewBox=\"0 0 24 24\"><title>jump-rope</title><path d=\"M21 4.5V10.5C21 11.2 20.5 11.9 19.8 12V17.3C19.8 18.6 19 21.1 16 21.1H14.5C14.9 20.7 15.3 20.2 15.5 19.6H16C18.1 19.6 18.2 17.7 18.2 17.4V12C17.5 11.9 17 11.3 17 10.5V4.5C17 3.7 17.7 3 18.5 3H19.5C20.3 3 21 3.7 21 4.5M14.8 18.2C14.8 19.7 13.6 21 12 21H8C5 21 4.2 18.5 4.2 17.2V12C3.5 11.9 3 11.2 3 10.5V4.5C3 3.7 3.7 3 4.5 3H5.5C6.3 3 7 3.7 7 4.5V10.5C7 11.2 6.5 11.9 5.8 12V17.3C5.8 17.7 5.9 19.5 8 19.5H9.6C9.4 19.1 9.3 18.7 9.3 18.3V8.3C9.3 6.8 10.5 5.5 12.1 5.5S14.8 6.7 14.8 8.3M13.2 8.2C13.2 7.6 12.7 7 12 7S10.8 7.6 10.8 8.2V18.2C10.8 18.9 11.4 19.4 12 19.4S13.2 18.8 13.2 18.2V8.2Z\" /></svg>";
                    case ArtIcon.Jewel: return "<svg viewBox=\"0 0 24 24\"><title>diamond-stone</title><path d=\"M16,9H19L14,16M10,9H14L12,17M5,9H8L10,16M15,4H17L19,7H16M11,4H13L14,7H10M7,4H9L8,7H5M6,2L2,8L12,22L22,8L18,2H6Z\" /></svg>";
                    case ArtIcon.Clothing: return "<svg viewBox=\"0 0 24 24\"><title>tshirt-v</title><path d=\"M16,21H8A1,1 0 0,1 7,20V12.07L5.7,13.07C5.31,13.46 4.68,13.46 4.29,13.07L1.46,10.29C1.07,9.9 1.07,9.27 1.46,8.88L7.34,3H9C9.29,4.8 10.4,6.37 12,7.25C13.6,6.37 14.71,4.8 15,3H16.66L22.54,8.88C22.93,9.27 22.93,9.9 22.54,10.29L19.71,13.12C19.32,13.5 18.69,13.5 18.3,13.12L17,12.12V20A1,1 0 0,1 16,21\" /></svg>";
                    case ArtIcon.Necklace: return "<svg viewBox=\"0 0 24 24\"><title>necklace</title><path d=\"M21.5 5H19.5C19.5 9.14 16.14 12.5 12 12.5C7.86 12.5 4.5 9.14 4.5 5H2.5C2.55 10.11 6.59 14.29 11.7 14.5C11.1 15.4 10 17.2 10 18C10 20.67 14 20.67 14 18C14 17.2 12.9 15.4 12.3 14.5C17.41 14.29 21.45 10.11 21.5 5Z\" /></svg>";
                    case ArtIcon.BowArrow: return "<svg viewBox=\"0 0 24 24\"><title>bow-arrow</title><path d=\"M19.03 6.03L20 7L22 2L17 4L17.97 4.97L16.15 6.79C10.87 2.16 3.3 3.94 2.97 4L2 4.26L2.5 6.2L3.29 6L10.12 12.82L6.94 16H5L2 19L4 20L5 22L8 19V17.06L11.18 13.88L18 20.71L17.81 21.5L19.74 22L20 21.03C20.06 20.7 21.84 13.13 17.21 7.85L19.03 6.03M4.5 5.78C6.55 5.5 11.28 5.28 14.73 8.21L10.82 12.12L4.5 5.78M18.22 19.5L11.88 13.18L15.79 9.27C18.72 12.72 18.5 17.45 18.22 19.5Z\" /></svg>";
                    case ArtIcon.Bell: return "<svg viewBox=\"0 0 24 24\"><title>bell</title><path d=\"M21,19V20H3V19L5,17V11C5,7.9 7.03,5.17 10,4.29C10,4.19 10,4.1 10,4A2,2 0 0,1 12,2A2,2 0 0,1 14,4C14,4.1 14,4.19 14,4.29C16.97,5.17 19,7.9 19,11V17L21,19M14,21A2,2 0 0,1 12,23A2,2 0 0,1 10,21\" /></svg>";
                    case ArtIcon.Candle: return "<svg viewBox=\"0 0 24 24\"><path d=\"M12.5,2C10.84,2 9.5,5.34 9.5,7A3,3 0 0,0 12.5,10A3,3 0 0,0 15.5,7C15.5,5.34 14.16,2 12.5,2M12.5,6.5A1,1 0 0,1 13.5,7.5A1,1 0 0,1 12.5,8.5A1,1 0 0,1 11.5,7.5A1,1 0 0,1 12.5,6.5M10,11A1,1 0 0,0 9,12V20H7A1,1 0 0,1 6,19V18A1,1 0 0,0 5,17A1,1 0 0,0 4,18V19A3,3 0 0,0 7,22H19A1,1 0 0,0 20,21A1,1 0 0,0 19,20H16V12A1,1 0 0,0 15,11H10Z\" /></svg>";
                    default: return "";
                }
            }
        }
    }
}