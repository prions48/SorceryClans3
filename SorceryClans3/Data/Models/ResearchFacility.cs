using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Models
{
    public class ResearchFacility
    {
        [Key] public Guid ID { get; set; }
        public int NumTeams { get; set; } = 1;
        public string DisplayInfo
        {
            get
            {
                string size = NumTeams + " Slot" + (NumTeams==1?"":"s");
                if (Project == null)
                    return size + ", open";
                return size + ", " + Project.GetColor.ToString() + " research with " + Project.TeamIDs.Count + " Team" + (Project.TeamIDs.Count==1?"":"s");
            }
        }
        public ResearchProject? Project { get; set; }
        public ResearchFacility()
        {
            ID = Guid.NewGuid();
        }
    }
}