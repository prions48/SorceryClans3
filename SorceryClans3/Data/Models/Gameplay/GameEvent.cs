using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;

namespace SorceryClans3.Data.Models
{
    public class GameEvent
    {
        [Key] public Guid ID { get; set; } = Guid.NewGuid();
        public MissionType Type { get; set; }
        public Mission? MissionToComplete { get; set; }
        public Team? TeamInTransit { get; set; }
        public Team? TargetTeam { get; set; }
        public Soldier? FocusSoldier { get; set; }
        public MapLocation? Destination { get; set; }
        public DateTime EventCompleted { get; set; }
        public DefenseType? DefenseType { get; set; }
        public ClientCity? City { get; set; }
        public DefenseStructure? DefenseStructure { get; set; }
        public List<Soldier> Deads { get; set; } = [];
        public Spell? HuntSpell { get; set; }
        public StyleTemplate? TrainStyle { get; set; }
        public bool? RoundTrip { get; set; }
        public bool Visible { get; set; }
        public bool FixedDate { get; set; }
        public bool Cancelable { get; set; } = false;
        public GameEvent(List<Soldier> deads, DateTime duedate)
        {
            Deads = deads;
            EventCompleted = duedate;
            Visible = true;
            FixedDate = false;
            Type = MissionType.Announcement;
        }
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
        public GameEvent(Team team, DateTime duedate, ClientCity city, bool roundtrip)
        {
            Type = MissionType.ResourceTransit;
            TeamInTransit = team;
            EventCompleted = duedate;
            Visible = true;
            FixedDate = true;
            Destination = city.Location;
            City = city;
            RoundTrip = roundtrip;
        }
        public GameEvent(Team team, MissionType type, DateTime duedate, Soldier soldier, StyleTemplate? style = null)
        {
            //training missions
            TeamInTransit = team;
            FocusSoldier = soldier;
            Type = type;
            Visible = true;
            FixedDate = false;
            Cancelable = true;
            EventCompleted = duedate;
            TrainStyle = style;
        }
        public GameEvent(Team team, MissionType type, DateTime duedate, DefenseStructure structure)
        {
            //using for defense cycle, prob also for other invisible assignment cycles
            TeamInTransit = team;
            Type = type;
            Visible = false;
            FixedDate = false;
            EventCompleted = duedate;
            DefenseStructure = structure;
        }
        public GameEvent(MissionType type, DateTime duedate, bool visible)
        {
            //should only be used for bandit/clan/etc //also using for defense cycle, prob also for other invisible assignment cycles
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
        public GameEvent(DefenseType type, DateTime duedate)
        {
            DefenseType = type;
            Type = MissionType.Building;
            Visible = true;
            FixedDate = true;
            EventCompleted = duedate;
        }
        public GameEvent(Team team1, Team team2, MapLocation location, DateTime duedate)
        {
            Type = MissionType.MedicalRescue;
            TeamInTransit = team1;
            TargetTeam = team2;
            EventCompleted = duedate;
            Visible = true;
            FixedDate = true;
            Destination = location;
        }
        public GameEvent(Team team, Spell spell, Soldier? target, DateTime duedate)
        {
            TeamInTransit = team;
            HuntSpell = spell;
            FocusSoldier = target;
            Type = MissionType.TameBeast;
            Visible = true;
            FixedDate = false;
            EventCompleted = duedate;
        }
        public GameEventDisplay ResolveMercenary()
        {
            if (MissionToComplete?.AttemptingTeam == null)
                throw new Exception("Failure to resolve missing mission or team");
            var results = MissionToComplete.CompleteMission();
            List<(Guid, int, bool)> gains = [];
            List<Soldier> newsolds = [];
            foreach (Soldier soldier in MissionToComplete.AttemptingTeam.GetAllSoldiers)
            {
                gains.Add(soldier.GainPower(MissionToComplete.PowerGain()));
                newsolds.AddRange(NecromancyScan(soldier, MissionToComplete));
            }
            //use diff to create game results
            return new($"Mission {(results.Item1 ? "succeeded" : "failed")}!", EventCompleted)
            {
                DisplayMission = MissionToComplete,
                DisplayTeam = MissionToComplete?.AttemptingTeam ?? TeamInTransit,
                DisplayResult = new TeamResult(MissionToComplete!.AttemptingTeam, gains, results.Item1, results.Item2),
                AddedSoldiers = newsolds
            };
        }
        private List<Soldier> NecromancyScan(Soldier soldier, Mission mission)
        {
            List<Soldier> newsolds = [];
            int i = 0;
            while (i < soldier.Consumables.Count)
            {
                Spell spell = soldier.Consumables[i];
                if (spell.LesserUndead != null && mission.CScore >= spell.LesserUndead.ScoreToGenerate())
                {
                    Soldier newsold = spell.LesserUndead.GenerateSoldier();
                    soldier.SubSoldiers.Add(newsold);
                    newsold.SubTo = soldier;
                    soldier.Consumables.RemoveAt(i);
                    newsolds.Add(newsold);
                }
                else
                    i++;
            }
            if (soldier.Artifact is NecroArtifact necro && mission.CScore >= necro.ScoreToGenerate())//tmp
            {
                Soldier newsold = necro.GenerateSoldier(mission.Client.ID);
                soldier.SubSoldiers.Add(newsold);
                newsold.SubTo = soldier;
                newsolds.Add(newsold);
            }
            return newsolds;
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
        public GameEventDisplay ResolveBuilding()
        {
            return new($"{DefenseType} construction completed!", EventCompleted);
        }
        public GameEventDisplay ResolveResourceTransit()
        {
            if (RoundTrip == null || TeamInTransit == null || City == null)
                throw new Exception("Failure to configure travel");
            if (RoundTrip == false)
            {
                TeamInTransit.MissionID = null;
                TeamInTransit.Location = null;
                return new($"Team {TeamInTransit.TeamName} has returned to home base, bringing resources from {City.CityName}.", EventCompleted);
            }
            else
            {
                return new($"Team {TeamInTransit.TeamName} has delivered resources from {City.CityName} and is now returning.", EventCompleted);
            }
        }
        public GameEventDisplay ResolveDefensePatrol()
        {
            Random r = new();
            foreach (Soldier s in TeamInTransit!.GetAllSoldiers)
            {
                s.GainPower(r.Next(50) + 20);
            }
            return new($"Team {TeamInTransit!.TeamName} completed defense cycle.", EventCompleted);//for testing?
        }
        public GameEventDisplay ResolveLeadershipTraining()
        {
            bool success = TeamInTransit!.LeadershipTraining(FocusSoldier!);
            TeamInTransit.MissionID = null;
            return new($"Team {TeamInTransit!.TeamName} has returned from leadership training under {FocusSoldier!.SoldierName}, {(success ? "triumphant" : "exhausted")}.", EventCompleted)
            {
                DisplayTeam = TeamInTransit
            };
        }
        public GameEventDisplay ResolveMedicalTraining()
        {
            bool success = TeamInTransit!.MedicalTraining(FocusSoldier!);
            TeamInTransit.MissionID = null;
            return new($"Team {TeamInTransit.TeamName} has returned from medical training under {FocusSoldier!.SoldierName}, {(success ? "triumphant" : "exhausted")}.", EventCompleted);
        }
        public GameEventDisplay ResolveStyleTraining()
        {
            if (FocusSoldier == null || TrainStyle == null)
                throw new Exception("Style training not configured.");
            bool success = TeamInTransit!.StyleTraining(FocusSoldier, TrainStyle);
            TeamInTransit.MissionID = null;
            return new($"Team {TeamInTransit.TeamName} has returned from style training under {FocusSoldier!.SoldierName}, {(success ? "triumphant" : "exhausted")}.", EventCompleted);
        }
        public GameEventDisplay ResolveRescue()
        {
            return new("Team " + TeamInTransit!.TeamName + " has arrived to help Team " + TargetTeam!.TeamName + ".", EventCompleted)
            {
                OpenHealDialog = true,
                DisplayTeam = TeamInTransit,
                DisplayTeam2 = TargetTeam
            };
        }
        public GameEventDisplay ResolveHunt()
        {
            Soldier? newsoldier = null;
            if (TeamInTransit == null)
                throw new Exception("Team missing from hunting effort!");
            if (HuntSpell?.Beast != null && FocusSoldier != null)
            {
                //do odds here
                newsoldier = HuntSpell.Beast.GenerateBeast(FocusSoldier.GivenName);
                newsoldier.Team = TeamInTransit;
                FocusSoldier.AddSubSoldier(newsoldier);
            }
            else if (HuntSpell?.BeastPet != null)
            {
                //do odds here
                newsoldier = HuntSpell.BeastPet.GenerateSoldier();
                TeamInTransit.AddSoldier(newsoldier);
            }
            TeamInTransit.MissionID = null;
            if (newsoldier != null)
            {
                return new("Team " + TeamInTransit.TeamName + " has tamed " + newsoldier.SoldierName, EventCompleted)
                {
                    DisplayResult = new TeamResult(TeamInTransit, TeamInTransit.BoostSoldiers(100), true, 1000),
                    NewSoldier = newsoldier
                };
            }
            else
            {
                return new("Team " + (TeamInTransit?.TeamName ?? "Unknown") + " has failed to tame a " + (HuntSpell?.BeastName ?? "beast") + "!", EventCompleted)
                {
                    DisplayResult = new TeamResult(TeamInTransit!, TeamInTransit!.BoostSoldiers(100), false, 1000)//tmp on diff //also don't forget this part does the damage
                };
            }
        }
        public GameEventDisplay Announcement()
        {
            return new("", EventCompleted)
            {
                Deads = Deads
            };
        }
    }
}