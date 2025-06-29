using System.Reflection.Metadata.Ecma335;

namespace SorceryClans3.Data.Models
{
    public class Defenses
    {
        //buildings/walls etc here
        //not teams (currently) though
        public List<DefenseStructure> Structures { get; set; } = [];
        public List<Team> GetTeams(DefenseType? type = null)
        {
            return Structures.Where(e => e.Team != null && (type == null || e.Type == type)).Select(e => e.Team!).ToList();
        }
        public List<DefenseStructure> StructuresByType(DefenseType type, OccupiedStatus status = OccupiedStatus.All)
        {
            return Structures.Where(e => e.Type == type && (status == OccupiedStatus.All || ((status == OccupiedStatus.Occupied) == (e.Team != null)))).ToList();
        }
        public void BuildStructure(DefenseType type)
        {
            Structures.Add(new(type));
        }
    }
    public class DefenseStructure
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Team? Team { get; set; } = null;
        public DefenseType Type { get; set; }
        public DefenseStructure(DefenseType type)
        {
            Type = type;
        }
    }
    public enum DefenseType
    {
        WatchTower = 0,
        SafeHouse = 1,
        Hospital = 2
    }
    public static class DefenseUtils
    {
        public static int Cost(this DefenseType type)
        {
            switch (type)
            {
                case DefenseType.WatchTower: return 10000;
                case DefenseType.SafeHouse: return 15000;
                case DefenseType.Hospital: return 7500;
                default: return 0;
            }
        }
        public static int Max(this DefenseType type)
        {
            switch (type)
            {
                case DefenseType.WatchTower: return 8;
                case DefenseType.SafeHouse: return 8;
                case DefenseType.Hospital: return 3;
                default: return 0;
            }
        }
    }
}