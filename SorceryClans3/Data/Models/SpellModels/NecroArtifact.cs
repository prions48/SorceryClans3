using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class NecroArtifact : Artifact
    {
        public Guid? UndeadID { get; set; }
        public string UndeadName { get; set; }
        public NecroTarget? Target { get; set; } = null;
        public Guid? TargetID { get; set; } = null;
        public SkillStat Secondary { get; set; }
        public int Duration { get; set; }
        public int SubtletyBreach { get; set; }
        public NecroArtifact(int lvl) : base(lvl)
        {
            UndeadName = Names.UndeadArtName(lvl);
            var result = Names.UndeadArt();
            ArtifactName = result.Item1 + " of the " + UndeadName;
            ArtifactIcon = result.Item2;
            Random r = new();
            Duration = lvl * 10 + r.Next(10);
            SubtletyBreach = lvl * lvl * 1000 + r.Next(lvl * 1000);
            for (int i = 0; i < lvl; i++)
            {
                if (r.Next(2) == 0)
                    Duration += 10;
                else
                    SubtletyBreach += 5000;
            }
            switch (r.Next(3))
            {
                case 0: Secondary = SkillStat.Combat; break;
                case 1: Secondary = SkillStat.Magic; break;
                case 2: Secondary = SkillStat.Subtlety; break;
            }
        }
        public int ScoreToGenerate()
        {
            Random r = new();
            return Level * Level * 5000 + r.Next(Level * Level * 2000);
        }
        public Soldier GenerateSoldier(Guid targetid)
        {
            TargetID = targetid;
            Random r = new();
            string[] names = UndeadName.Split(" ");
            Soldier newsoldier = new Soldier()
            {
                ClanName = names[0],
                GivenName = names[1],
                Type = SoldierType.LesserUndead,
                PowerLevel = Level * 500 + r.Next(500),
                ComBase = r.Next(2) + (Secondary == SkillStat.Combat ? r.Next(3) + Level / 2 : 0),
                MagBase = r.Next(2) + (Secondary == SkillStat.Magic ? r.Next(3) + Level / 2 : 0),
                SubBase = 5 + Level / 3 + (Secondary == SkillStat.Subtlety ? r.Next(3) + Level / 3 : 0),
                TravelBase = null,
                HPBase = 5 + Level / 3 + r.Next(3),
                TypeID = this.ID,
                RemainingActive = Duration + r.Next(20)
            };
            newsoldier.CalcLimit();
            newsoldier.HPCurrent = newsoldier.HPMax;
            UndeadID = newsoldier.ID;
            return newsoldier;
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