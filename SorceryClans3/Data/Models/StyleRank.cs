namespace SorceryClans3.Data.Models
{
    public class StyleRank
    {
        public Guid StyleID { get; set; }
        public string Name { get; set; } = "";
        public int CBonus { get; set; }
        public int MBonus { get; set; }
        public int SBonus { get; set; }
        public int KBonus { get; set; }
        public int StyleXP { get; set; }
        public bool GivePower { get; set; }
        public RankTeach Teach { get; set; } = RankTeach.NoTeach;
        public StyleRank(Guid styleid)
        {
            StyleID = styleid;
        }
    }
}