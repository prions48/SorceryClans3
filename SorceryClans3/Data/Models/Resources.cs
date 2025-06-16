namespace SorceryClans3.Data.Models
{
    public class Resources
    {
        public int Money { get; set; } = 0;
        public List<Artifact> Artifacts { get; set; } = [];
        public List<Beast> Beasts { get; set; } = [];
        public List<BeastHarvest> Harvests { get; set; } = [];
        public List<LesserUndead> LesserUndeads { get; set; } = [];
        //add spell fragments later
        public void TransferResources(Resources res)
        {
            Money += res.Money;
            Artifacts.AddRange(res.Artifacts);
            Beasts.AddRange(res.Beasts);
            Harvests.AddRange(res.Harvests);
            LesserUndeads.AddRange(res.LesserUndeads);
            res.Reset(); //is this OOP?
        }
        public void Reset()
        {
            Money = 0;
            Artifacts = [];
            Beasts = [];
            Harvests = [];
            LesserUndeads = [];
        }
    }
}