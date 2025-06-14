namespace SorceryClans3.Data.Models
{
    public class HunterMission : Mission
    {
        public Guid HunterID { get; set; }
        public Guid TargetID { get; set; }
        public HunterMission(int seed, bool forceall = false, bool nocolor = false) : base(seed, new(0), forceall, nocolor)
        {
            Type = MissionType.TameBeast;
        }
    }
}