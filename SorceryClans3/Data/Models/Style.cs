using Microsoft.Identity.Client.Extensibility;

namespace SorceryClans3.Data.Models
{
    public class Style
    {
        public string StyleName { get { return Template.StyleName; } }
        public Soldier Soldier { get; set; }
        public Guid StyleID { get { return Template.ID; } }
        private StyleTemplate Template { get; set; }
        private IList<StyleRank> Ranks { get { return Template.Ranks; } }
        public StyleRank CurrentRank { get { return Ranks.Where(e => e.StyleXP <= this.StyleXP).MaxBy(e => e.StyleXP)!; } }
        public int StyleXP { get; set; }
        private double Aptitude { get; set; }
        private Random r = new();
        public Style(StyleTemplate template, Soldier soldier)
        {
            Template = template;
            StyleXP = 0;
            Soldier = soldier;
            Aptitude = soldier.StyleAptitude;
        }
        public int CBonus { get { return Ranks.Where(e => e.StyleXP <= this.StyleXP).Sum(e => e.CBonus); } }
        public int MBonus { get { return Ranks.Where(e => e.StyleXP <= this.StyleXP).Sum(e => e.MBonus); } }
        public int SBonus { get { return Ranks.Where(e => e.StyleXP <= this.StyleXP).Sum(e => e.SBonus); } }
        public int KBonus { get { return Ranks.Where(e => e.StyleXP <= this.StyleXP).Sum(e => e.KBonus); } }
        public RankTeach Teach
        {
            get
            {
                return CurrentRank.Teach;
            }
        }
        public void Level(bool force = false)
        {
            if (StyleXP < Aptitude * 100 && (r.NextDouble() < Aptitude || force))
            {
                StyleXP++;
                if (CurrentRank.GivePower && Template.Power != null && Soldier.Power == null)
                {
                    Soldier.Power = Template.Power.GeneratePower(true);
                }
            }
        }
        public double LevelGap()
        {
            return Aptitude - (StyleXP / 100.0);
        }
        public string Icon
        {
            get
            {
                return MudBlazor.Icons.Material.Filled.FrontHand;//for now...
            }
        }
        public MudBlazor.Color IconColor
        {
            get
            {
                if (CurrentRank.GivePower)
                    return Template.Power?.Color.Color() ?? MudBlazor.Color.Secondary;
                return MudBlazor.Color.Secondary;
            }
        }
    }
}