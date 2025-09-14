using SorceryClans3.Data.Abstractions;

namespace SorceryClans3.Data.Models
{
    public class RescueMission : IMission
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid MissionID => ID;
        public string MissionName => $"Rescuing Team {Rescuees.TeamName}";
        public Team Rescuers { get; set; }
        public Team Rescuees { get; set; }
        public RescueMission(Team rescuers, Team rescuees)
        {
            Rescuers = rescuers;
            Rescuees = rescuees;
        }
    }
}