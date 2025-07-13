using SorceryClans3.Data.Tools;
namespace SorceryClans3.Data.Models
{
    public enum PetType
    {
        Spy = 0, //sub only
        Scout = 1, //com+sub
        Sentinel = 2, //mag+sub
        Mount = 3, //travel
        Brute = 4, //com only
        Magic = 5, //mag only
        Breaker = 6, //com+mag
        Hunter = 7 // all 3
    }
    public class BeastPet
    {
        public Guid ID { get; set; }
        public PetType Type { get; set; }
        public string Animal { get; set; }
        public string Adjective { get; set; } = "Green";
        public string ToolName { get; set; } = "Saddle";
        public string BeastName { get { return Adjective + " " + Animal; } }
        public BeastPet(int lvl)
        {
            Type = PetType.Mount;//for now, maybe forever
            Animal = Names.Domesticated(Type, lvl);
        }
        public Soldier GenerateSoldier()
        {
            return new()
            {
                ClanName = Adjective,
                GivenName = Animal,
                TravelBase = 12,
                ComBase = 0,
                MagBase = 0,
                SubBase = 5,
                Type = SoldierType.Beast,
                PowerLevel = 1000,
                TypeID = this.ID,
                MountCountBase = 1
            };
        }
    }
}