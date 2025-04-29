using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Abstractions
{
    public interface IHP
    {
        public string SoldierName { get; }
        public int HPMax { get; }
        public int HPCurrent { get; set; }
        public HealthLevel Health { get; set; }
    }
}