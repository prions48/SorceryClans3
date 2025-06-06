using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Models
{
    public class GameEvent
    {
        [Key] public Guid ID { get; set; }
        public MissionType Type { get; set; }
        public Mission? MissionToComplete { get; set; }
        public Team? TeamInTransit { get; set; }
        public DateTime EventCompleted { get; set; }
        public bool Visible { get; set; }
        public GameEvent(Mission mission, DateTime duedate)
        {
            Type = MissionType.Mercenary;
            MissionToComplete = mission;
            EventCompleted = duedate;
            Visible = true;
        }
        public GameEvent(Team team, DateTime duedate)
        {
            Type = MissionType.TravelHome;
            TeamInTransit = team;
            EventCompleted = duedate;
            Visible = true;
        }
        public GameEventDisplay ResolveMercenary()
        {
            if (MissionToComplete?.AttemptingTeam == null)
                throw new Exception("Failure to resolve missing mission or team");
            var results = MissionToComplete.CompleteMission();
            //use diff to create game results
            return new($"Mission {(results.Item1 ? "succeeded" : "failed")}!")
            {
                DisplayMission = MissionToComplete,
                DisplayTeam = MissionToComplete?.AttemptingTeam ?? TeamInTransit,
                DisplayResult = new TeamResult(MissionToComplete!.AttemptingTeam, results.Item2)
            };
        }
        public GameEventDisplay ResolveReturn()
        {
            if (TeamInTransit == null)
                throw new Exception("Failure to resolve missing team");
            TeamInTransit.MissionID = null;
            return new($"Team {TeamInTransit.TeamName} has returned.")
            {
                DisplayTeam = TeamInTransit
            };
        }
    }
}