using System.ComponentModel.DataAnnotations;
using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class Faerie
    {
        [Key] public Guid ID { get; set; }
        public string FaerieType { get; set; }
        public string FaerieTitle { get; set; }
        public string FaerieName { get; set; }
        private string FaerieTrueName { get; set; }
        public FaerieCourt? Court { get; set; }
        public FaerieSeason Season
        {
            get
            {
                if (Court == null)
                    return FaerieSeason.None;
                return this.Court.Season;
            }
        }
        public Faerie(FaerieCourt? court, int rank)
        {
            Court = court;
            FaerieTrueName = Names.TrueName();
            FaerieTitle = Names.FaerieTitle(rank);
            FaerieType = Names.FaerieType(Season, rank);
        }

    }
}