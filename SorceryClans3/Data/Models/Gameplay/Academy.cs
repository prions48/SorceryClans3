using SorceryClans3.Data.Abstractions;

namespace SorceryClans3.Data.Models
{
    public class Academy
    {
        public Guid AcademyAssignmentID { get; init; } = Guid.NewGuid();
        public Assignment AcademyAssignment()
        {
            return new(AcademyAssignmentID, "Academy Instruction");
        }
        public Dictionary<AcademyRole, Soldier?> HeadInstructors { get; set; } = [];
        public List<Team> Teams { get; set; } = [];
        public double CombatScore
        {
            get
            {
                double score = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (HeadInstructors.TryGetValue((AcademyRole)i, out Soldier? s))
                    {
                        score += ComAcadScore(s, (AcademyRole)i);
                    }
                }
                return score + CoachScores(Teams.SelectMany(e => e.GetAllSoldiers).ToList(), HeadInstructors.Values.Where(e => e is not null).Select(e => e!.ID).ToList());
            }
        }
        public double MagicScore
        {
            get
            {
                double score = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (HeadInstructors.TryGetValue((AcademyRole)i, out Soldier? s))
                    {
                        score += MagAcadScore(s, (AcademyRole)i);
                    }
                }
                return score + CoachScores(Teams.SelectMany(e => e.GetAllSoldiers).ToList(), HeadInstructors.Values.Where(e => e is not null).Select(e => e!.ID).ToList());
            }
        }
        public double SubtletyScore
        {
            get
            {
                double score = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (HeadInstructors.TryGetValue((AcademyRole)i, out Soldier? s))
                    {
                        score += SubAcadScore(s, (AcademyRole)i);
                    }
                }
                return score + CoachScores(Teams.SelectMany(e => e.GetAllSoldiers).ToList(), HeadInstructors.Values.Where(e => e is not null).Select(e => e!.ID).ToList());
            }
        }
        public void SetInstructor(AcademyRole arole, Soldier sold)
        {
            if (HeadInstructors.ContainsKey(arole))
            {
                HeadInstructors[arole] = sold;
            }
            else
            {
                HeadInstructors.TryAdd(arole, sold);
            }
        }
        public void AddTeam(Team team)
        {
            Teams.Add(team);
            team.AssignMission(AcademyAssignment());
        }
        public void RemoveTeam(Team team)
        {
            List<AcademyRole> roles = [];
            foreach (Soldier sold in team.GetAllSoldiers)
            {
                if (HeadInstructors.Values.Where(e => e != null).Select(e => e!.ID).Contains(sold.ID))
                {
                    foreach (KeyValuePair<AcademyRole, Soldier?> kvp in HeadInstructors)
                    {
                        if (kvp.Value != null && kvp.Value.ID == sold.ID)
                        {
                            roles.Add(kvp.Key);
                        }
                    }
                }
            }
            foreach (AcademyRole role in roles)
            {
                HeadInstructors[role] = null;
            }
            Teams.Remove(team);
            team.ClearMission();
        }
        public static double ComAcadScore(Soldier? s, AcademyRole arole)
        {
            if (s == null || s.PowerLevel <= 1)
                return 0;
            int mainscore = s.Combat;
            int minusscore = 0;
            switch (arole)
            {
                case AcademyRole.Headmaster: return (mainscore + s.Magic + s.Subtlety) * Math.Log10(s.PowerLevel);
                case AcademyRole.Battlemage: mainscore += s.Magic; minusscore = s.Subtlety; break;
                case AcademyRole.HeadAssassin: mainscore += s.Subtlety; minusscore = s.Magic; break;
                case AcademyRole.Blademaster: minusscore = s.Magic + s.Subtlety; break;
                default: return 0;
            }
            if (minusscore > mainscore)
                return 0;
            return s.TeachSkill * (s.Charisma + mainscore - minusscore) * Math.Log10(s.PowerLevel) * (arole == AcademyRole.Blademaster ? 2 : 1);
        }
        public static double MagAcadScore(Soldier? s, AcademyRole arole)
        {
            if (s == null || s.PowerLevel <= 1)
                return 0;
            int mainscore = s.Magic;
            int minusscore = 0;
            switch (arole)
            {
                case AcademyRole.Headmaster: return (mainscore + s.Combat + s.Subtlety) * Math.Log10(s.PowerLevel);
                case AcademyRole.Battlemage: mainscore += s.Combat; minusscore = s.Subtlety; break;
                case AcademyRole.Shadowmage: mainscore += s.Subtlety; minusscore = s.Combat; break;
                case AcademyRole.Spellmaster: minusscore = s.ComBase + s.Subtlety; break;
                default: return 0;
            }
            if (minusscore > mainscore)
                return 0;
            return s.TeachSkill * (s.Charisma + mainscore - minusscore) * Math.Log10(s.PowerLevel) * (arole == AcademyRole.Spellmaster ? 2 : 1);
        }
        public static double SubAcadScore(Soldier? s, AcademyRole arole)
        {
            if (s == null || s.PowerLevel <= 1)
                return 0;
            int mainscore = s.Subtlety;
            int minusscore = 0;
            switch (arole)
            {
                case AcademyRole.Headmaster: return (mainscore + s.Magic + s.Combat) * Math.Log10(s.PowerLevel);
                case AcademyRole.Shadowmage: mainscore += s.Magic; minusscore = s.Combat; break;
                case AcademyRole.HeadAssassin: mainscore += s.Subtlety; minusscore = s.Magic; break;
                case AcademyRole.Spymaster: minusscore = s.Magic + s.Combat; break;
                default: return 0;
            }
            if (minusscore > mainscore)
                return 0;
            return s.TeachSkill * (s.Charisma + mainscore - minusscore) * Math.Log10(s.PowerLevel) * (arole == AcademyRole.Spymaster ? 2 : 1);
        }
        public static double CoachScores(List<Soldier> solds, List<Guid> excludes)
        {
            double score = 0;
            foreach (Soldier sold in solds)
            {
                if (excludes.Contains(sold.ID) || sold.PowerLevel <= 1)
                    continue;
                score += Math.Log10(sold.PowerLevel) * sold.TeachSkill * (sold.Combat + sold.Magic + sold.Subtlety + sold.Charisma) / 5;
            }
            return score;
        }
        
    }
    public enum AcademyRole
    {
        Headmaster = 0,
        Battlemage = 1,
        HeadAssassin = 2,
        Shadowmage = 3,
        Blademaster = 4,
        Spellmaster = 5,
        Spymaster = 6
    }
}