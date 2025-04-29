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
        public string Adjective { get; set; }
        public string BeastName { get { return Adjective + " " + Animal; } }
        public BeastPet(int lvl, PetType type)
        {
            Type = type;
            Animal = Names.Domesticated(type, lvl);
        }
    }
}