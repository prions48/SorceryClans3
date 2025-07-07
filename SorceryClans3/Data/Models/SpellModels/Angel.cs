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
        CounterIntel = 10
    }
    public class Angel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string AngelScope { get; set; }
        public int Rank { get; set; }
        public AngelScope FirstScope { get; init; }
        public AngelScope SecondScope { get; init; }
        public List<AngelIcon> Icons { get; set; } = [];
        public Angel(int lvl)
        {
            Name = Names.AngelName();
            Random r = new();
            FirstScope = RandomScope(r, 3); //play around later with whether to adjust odds of 
            SecondScope = RandomScope(r, 1);
            AngelScope = Names.GetScope(FirstScope, SecondScope);
            Icons.Add(new(this));
        }
        private AngelScope RandomScope(Random r, int mainodds)
        {
            if (mainodds > 1 && r.Next(mainodds) == 0)
            {
                return (AngelScope)r.Next(11);
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
                    case 4: return "Watcher";
                    case 5: return "Dominion";
                    case 6: return "Virtue";
                    case 7: return "Power";
                    case 8: return "Principality";
                    case 9: return "Archangel";
                    default: return "";
                }
            }
        }
    }
}