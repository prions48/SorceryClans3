using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public enum AngelScope
    {
        Combat = 0,
        Magic = 1,
        Subtlety = 2,
        Heal = 3,
        Travel = 4,
        Leadership = 5,
        Charisma = 6,
        Tactics = 7,
        Logistics = 8,
        Teaching = 9,
        CounterIntel = 10,
        Research = 11
    }
    public class Angel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Scope { get; set; }
        public int Rank { get; set; }
        public AngelScope FirstScope { get; init; }
        public AngelScope SecondScope { get; init; }
        public List<AngelIcon> Icons { get; set; } = [];
        public Angel(int lvl)
        {
            if (lvl < 1)
                lvl = 1;
            else if (lvl > 9)
                lvl = 9;
            Rank = lvl;
            Name = Names.AngelName();
            Random r = new();
            FirstScope = RandomScope(r, 3); //play around later with whether to adjust odds of 
            SecondScope = RandomScope(r, 1);
            Scope = Names.GetScope(FirstScope, SecondScope);
            Icons.Add(new(this));
        }
        private AngelScope RandomScope(Random r, int mainodds)
        {
            if (mainodds > 1 && r.Next(mainodds) == 0)
            {
                return (AngelScope)r.Next(12);
            }
            else
            {
                return (AngelScope)r.Next(5);
            }
        }
        public string AngelName => $"{AngelRank} {Name}";
        public string AngelRank
        {
            get
            {
                switch (Rank)
                {
                    case 1: return "Seraphim";
                    case 2: return "Cherubim";
                    case 3: return "Ophanim";
                    case 4: return "Grigorim";//watcher
                    case 5: return "Hashmallim";//dominion
                    case 6: return "Tarshishim";//virtue
                    case 7: return "Erelim";//power
                    case 8: return "Shinanim";//principality
                    case 9: return "Archangel";
                    default: return "";
                }
            }
        }
    }
}