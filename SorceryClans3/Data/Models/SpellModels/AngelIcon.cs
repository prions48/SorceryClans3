namespace SorceryClans3.Data.Models
{
    public class AngelIcon
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Angel Angel { get; set; }
        public int Level { get; set; }
        public int HPCost { get; set; }
        public int? Com { get; set; }
        public int? Mag { get; set; }
        public int? Sub { get; set; }
        public int? Tra { get; set; }
        public int? GroupTra { get; set; }
        public int? Heal { get; set; }
        public int? Lead { get; set; } //pct pts
        public int? Cha { get; set; }
        public int? Tac { get; set; }
        public int? Log { get; set; }
        public int? Teach { get; set; } //pct pts
        public int? Counter { get; set; } //pct pts
        public int? Research { get; set; } //pct pts
        public AngelIcon(Angel angel)
        {
            Angel = angel;
            Level = 1;
            Random r = new();
            HPCost = -2 - (angel.Rank < 3 ? 1 : angel.Rank / 2);
            switch (angel.FirstScope)
            {
                case AngelScope.Combat: Com = angel.Rank; break;
                case AngelScope.Magic: Mag = angel.Rank; break;
                case AngelScope.Subtlety: Sub = angel.Rank; break;
                case AngelScope.Heal: Heal = angel.Rank; break;
                case AngelScope.Travel: Tra = angel.Rank; break;
                case AngelScope.Leadership: Lead = angel.Rank; break;
                case AngelScope.Charisma: Cha = angel.Rank; break;
                case AngelScope.Logistics: Log = angel.Rank; break;
                case AngelScope.Tactics: Tac = angel.Rank; break;
                case AngelScope.Teaching: Teach = angel.Rank; break;
                case AngelScope.CounterIntel: Counter = angel.Rank; break;
            }
            switch (angel.SecondScope)
            {
                case AngelScope.Combat: Com = (Com ?? 0) + (angel.Rank / 2); break;
                case AngelScope.Magic: Mag = (Mag ?? 0) + (angel.Rank / 2); break;
                case AngelScope.Subtlety: Sub = (Sub ?? 0) + (angel.Rank / 2); break;
                case AngelScope.Heal: Heal = (Heal ?? 0) + (angel.Rank / 2); break;
                case AngelScope.Travel: Tra = (Tra ?? 0) + (angel.Rank / 2); break;
            }
        }
        public string ArtifactName()
        {
            return "Icon of " + Angel.AngelName;
        }
        public Artifact GenerateIcon()
        {
            return new Artifact(this);
        }
    }
}