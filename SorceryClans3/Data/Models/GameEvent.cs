using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Models
{
    public class GameEvent
    {
        [Key] public Guid ID { get; set; } = Guid.NewGuid();
        public MissionType Type { get; set; }
        public Mission? MissionToComplete { get; set; }
        public Team? TeamInTransit { get; set; }
        public MapLocation? Destination { get; set; }
        public DateTime EventCompleted { get; set; }
        public bool Visible { get; set; }
        public bool FixedDate { get; set; }
        public GameEvent(Mission mission, DateTime duedate)
        {
            Type = MissionType.Mercenary;
            MissionToComplete = mission;
            EventCompleted = duedate;
            Visible = true;
            FixedDate = true;
        }
        public GameEvent(Team team, DateTime duedate, MissionType type, MapLocation destination)
        {
            Type = type;
            TeamInTransit = team;
            EventCompleted = duedate;
            Visible = true;
            FixedDate = true;
            Destination = destination;
        }
        public GameEvent(MissionType type, DateTime duedate)
        {
            //should only be used for bandit/clan/etc
            Type = type;
            Visible = true;//for testing
            FixedDate = true;//for testing
            EventCompleted = duedate;
        }
        public GameEventDisplay ResolveMercenary()
        {
            if (MissionToComplete?.AttemptingTeam == null)
                throw new Exception("Failure to resolve missing mission or team");
            var results = MissionToComplete.CompleteMission();
            //use diff to create game results
            return new($"Mission {(results.Item1 ? "succeeded" : "failed")}!", EventCompleted)
            {
                DisplayMission = MissionToComplete,
                DisplayTeam = MissionToComplete?.AttemptingTeam ?? TeamInTransit,
                DisplayResult = new TeamResult(MissionToComplete!.AttemptingTeam, results.Item2)
            };
        }
        public GameEventDisplay ResolveReturn(bool liaison = false)
        {
            if (TeamInTransit == null)
                throw new Exception("Failure to resolve missing team");
            TeamInTransit.MissionID = null;
            TeamInTransit.Location = Destination;
            return new($"Team {TeamInTransit.TeamName} has {(liaison ? "arrived to coordinate missions" : "returned to " + TeamInTransit.Location?.LocationName ?? "home base.")}.", EventCompleted)
            {
                DisplayTeam = TeamInTransit
            };
        }
    }
}