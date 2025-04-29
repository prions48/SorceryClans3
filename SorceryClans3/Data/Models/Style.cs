namespace SorceryClans3.Data.Models
{
    public class Style
    {
        private IList<StyleRank> Ranks { get; set; }
        public int StyleXP { get; set; }
        public Style(StyleTemplate template)
        {
            Ranks = template.Ranks;
            StyleXP = 0;
        }
        public int CBonus { get { return Ranks.Where(e => e.StyleXP <= this.StyleXP).Sum(e => e.CBonus); } }
        public int MBonus { get { return Ranks.Where(e => e.StyleXP <= this.StyleXP).Sum(e => e.MBonus); } }
        public int SBonus { get { return Ranks.Where(e => e.StyleXP <= this.StyleXP).Sum(e => e.SBonus); } }
        public int KBonus { get { return Ranks.Where(e => e.StyleXP <= this.StyleXP).Sum(e => e.KBonus); } }
    }
}