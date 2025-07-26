using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class NecroArtifact : Artifact
    {
        public string UndeadName { get; set; }
        public NecroTarget? Target { get; set; } = null;
        public Guid? TargetID { get; set; } = null;
        public NecroArtifact(int lvl) : base(lvl)
        {
            UndeadName = Names.UndeadArtName(lvl);
            var result = Names.UndeadArt();
            ArtifactName = result.Item1 + " of the " + UndeadName;
            ArtifactIcon = result.Item2;
        }
    }
    public enum NecroTarget
    {
        Client,
        Rival,
        Clan
        //probably more later
    }
}