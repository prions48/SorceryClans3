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
            HPCost = -3;
        }
        public AngelIcon(AngelIcon icon)
        {
            
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