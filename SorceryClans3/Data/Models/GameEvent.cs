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
            Type = mission.Type;
            MissionToComplete = mission;
            EventCompleted = duedate;
            Visible = true;
            FixedDate = mission.Type == MissionType.Mercenary;
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
        public GameEvent(MissionType type, DateTime duedate, bool visible)
        {
            //should only be used for bandit/clan/etc
            Type = type;
            Visible = visible;//for testing
            FixedDate = visible;//for testing
            EventCompleted = duedate;
        }
        public GameEvent(MissionContract contract, DateTime duedate, bool payday)
        {
            //should only be used for bandit/clan/etc
            Type = payday ? MissionType.PayDay : MissionType.ContractMisson;
            MissionToComplete = contract;
            Visible = !payday;//don't need to clog up dashboard
            FixedDate = false;
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
                DisplayResult = new TeamResult(MissionToComplete!.AttemptingTeam, results.Item1, results.Item2)
            };
        }
        public GameEventDisplay ResolveReturn(bool liaison = false)
        {
            if (TeamInTransit == null)
                throw new Exception("Failure to resolve missing team");
            TeamInTransit.MissionID = null;
            TeamInTransit.Location = Destination;
            return new($"Team {TeamInTransit.TeamName} has {(liaison ? "arrived to coordinate missions at" : "arrived at ")} {TeamInTransit.Location?.LocationName ?? "home base."}.", EventCompleted)
            {
                DisplayTeam = TeamInTransit
            };
        }
        public GameEventDisplay? ResolvePayday()
        {
            if (MissionToComplete == null)
                return null;
            if (MissionToComplete.AttemptingTeam == null || MissionToComplete.AttemptingTeam.MissionID != MissionToComplete.ID)
                return null;
            MissionContract? contract = MissionToComplete as MissionContract;
            if (contract == null)
                return null;
            int money = contract.PayContract();
            return new($"{contract.Client.CityName} has paid {contract.MoneyReward} for the services of Team {contract.AttemptingTeam}.", EventCompleted);
        }
    }
}