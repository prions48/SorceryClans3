using SorceryClans3.Data.Models;

namespace SorceryClans3.Data.Abstractions
{
    public interface IMission
    {
        public Guid MissionID { get; }
        public string MissionName { get; }
    }
    public class Assignment : IMission
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid MissionID => ID;
        public string MissionName { get { return _missionName; } }
        private string _missionName;
        public Assignment(Guid id, string name)
        {
            ID = id;
            _missionName = name;
        }
        public static Assignment Travel
        {
            get
            {
                return new(Guid.Empty, "Traveling");
            }
        }
        private static Guid leadership = Guid.NewGuid();
        private static Guid medical = Guid.NewGuid();
        public Assignment(StyleTemplate style)
        {
            ID = style.ID;
            _missionName = $"Training in the {style.StyleName}";
        }
        public Assignment(MissionType type)
        {
            switch (type)
            {
                case MissionType.LeadershipTraining: _missionName = "Leadership Training"; ID = leadership; break;
                case MissionType.MedicalTraining: _missionName = "Medical Training"; ID = medical; break;
                default: _missionName = "Unknown"; break;
            }
        }

    }
}