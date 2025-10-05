using SorceryClans3.Data.Abstractions;

namespace SorceryClans3.Data.Models
{
    public enum RivalTeamAssign
    {
        Explore,
        Perimeter,
        Infiltrate,
        Mercenary,
        Other
    }
    public class RivalTeamMission : IMission
    {
        public Guid MissionID => Guid.NewGuid();
        public RivalTeamAssign Assign { get; set; }
        public string MissionName => Assign.ToString();
        public RivalTeamMission(RivalTeamAssign assign)
        {
            Assign = assign;
        }
    }
    public static class RivalUtils
    {
        private static Random r = new();
        public static void SetAssign(this RivalTeam team)
        {
            switch (r.Next(10))
            {
                case 0: case 1: team.Mission = new RivalTeamMission(RivalTeamAssign.Explore); break;
                case 2: team.Mission = new RivalTeamMission(RivalTeamAssign.Perimeter); break;
                case 3: case 4: case 5: case 6: team.Mission = new RivalTeamMission(RivalTeamAssign.Mercenary); break;
                case 7: case 8: break; //idle
                default: team.Mission = new RivalTeamMission(RivalTeamAssign.Other); break;
            }
        }
    }
}