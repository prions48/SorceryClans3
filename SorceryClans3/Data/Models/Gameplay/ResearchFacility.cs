using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using SorceryClans3.Data.Abstractions;

namespace SorceryClans3.Data.Models
{
    public class ResearchFacility : IMission
    {
        [Key] public Guid ID { get; set; }
        public Guid MissionID => ID;
        public string MissionName => $"Ready at {FacilityName}";
        public int NumTeams { get; set; } = 1;
        public List<Team> Teams { get; set; } = [];
        public List<SpellCastMission> SpellCastMissions { get; set; } = [];
        public Action AddTeamKnowledge { get; set; }
        public ResearchProject? Project { get; set; }
        public string DisplayInfo
        {
            get
            {
                string size = NumTeams + " Slot" + (NumTeams == 1 ? "" : "s");
                if (Project == null)
                    return size + ", open";
                return size + ", " + Project.GetColor.ToString() + " research with " + Project.TeamIDs.Count + " Team" + (Project.TeamIDs.Count == 1 ? "" : "s");
            }
        }
        private string FacilityName
        {
            get
            {
                MagicColor? color = Project?.GetColor;
                switch (color)
                {
                    case null: return "Empty Facility";
                    case MagicColor.None: return "Open Research Facility";
                    default: return $"{color} Magic Research Facility";
                }
            }
        }
        public ResearchFacility(Action callback)
        {
            ID = Guid.NewGuid();
            AddTeamKnowledge = callback;
        }
        public void AddTeam(Team team)
        {
            Teams.Add(team);
            team.AssignMission(this);
            AddTeamKnowledge.Invoke();
        }
        public void RemoveTeam(Team team)
        {
            if (Teams.Remove(team))
                team.ClearMission();
        }
        public void StartCastSpell(Team team, Spell spell, Soldier? caster = null, Soldier? target = null, int? quantity = null)
        {
            if (Teams.Contains(team))
            {
                SpellCastMission mission = new(spell, team, caster, target, quantity);
                SpellCastMissions.Add(mission);
                team.AssignMission(mission);
            }
        }
        public List<SpellCastMission> IncrementDay()
        {
            List<SpellCastMission> spellmissions = [];
            int ctr = 0;
            while (ctr < SpellCastMissions.Count)
            {
                SpellCastMission? result = SpellCastMissions[ctr].IncrementDay();
                if (result != null)
                {
                    spellmissions.Add(result);
                    SpellCastMissions[ctr].CastingTeam.AssignMission(this);
                    SpellCastMissions.RemoveAt(ctr);
                }
                else
                    ctr++;
            }
            return spellmissions;
        }
    }
}