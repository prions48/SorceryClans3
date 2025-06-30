using SorceryClans3.Data.Abstractions;
namespace SorceryClans3.Data.Models
{
    public class Patient : IHP
    {
        public Guid PatientID { get; set; }
        public string SoldierName { get; set; } = "";
        public int PowerLevel { get; set; }
        public int HealNeeded { get; set; }
        public int HPMax { get { return _hpMax; } }
        public int _hpMax { get; set; }
        public int HPCurrent { get; set; }
        public HealthLevel Health { get; set; }
        public IList<string> MedicIDs { get; set; } = new List<string> { Guid.Empty.ToString() };
        public HealStatus Status { get; set; } = HealStatus.NotYet;
    }
}