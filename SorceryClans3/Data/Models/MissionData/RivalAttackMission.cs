using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using SorceryClans3.Data.Abstractions;

namespace SorceryClans3.Data.Models
{
    public class RivalAttackMission : IMission
    {
        [Key] public Guid ID { get; set; } = Guid.NewGuid();
        public Guid MissionID => ID;
        public string MissionName => $"Attacking {Rival.RivalName}";
        public Team AttackingTeam { get; set; }
        public Rival Rival { get; set; }
        public DateTime MissionStart { get; set; }
        private GameSettings Settings { get; set; }
        public RivalAttackMission(Rival rival, Team team, GameSettings settings)
        {
            Settings = settings;
            Rival = rival;
            AttackingTeam = team;
            AttackingTeam.TravelLocation = new(team.BaseLocation, rival.Location, settings.CurrentTime);
        }
        public (List<GameEventDisplay>,GameEventDisplay?) Iterate()
        {
            if (AttackingTeam?.TravelLocation == null)
                throw new Exception("Travel attack configuration error");
            bool reached = AttackingTeam.TravelLocation.Iterate(AttackingTeam.DScore, Settings);
            if (AttackingTeam.TravelLocation.Location.GetDistance(Rival.Location) <= Rival.PerimeterRange)
                return (Rival.AttemptDetect(AttackingTeam).Select(e => new GameEventDisplay($"Team {AttackingTeam.TeamName} has been detected by a perimeter team while en route to attack {Rival.RivalName}", Settings.CurrentTime) { DisplayResult = e, DisplayTeam = AttackingTeam }).ToList(),reached ? Rival.AttackRival(AttackingTeam, Settings.CurrentTime) : null);
            return ([],null);
        }
    }
}