using SorceryClans3.Data.Abstractions;
using SorceryClans3.Data.Models;
namespace SorceryClans3.Components.Pages.Dialogs
{
    public class MedicPatients
    {
        public string MedicID { get; set; } = "";
        public string MedicName { get; set; } = "";
        public int MPBase { get; set; }
        public int Fatigue { get; set; }
        public string FatigueIcon
            {
                get
                {
                    if (Fatigue <= 0)
                        return MudBlazor.Icons.Material.TwoTone.SentimentVerySatisfied;
                    if (Fatigue < 3)
                        return MudBlazor.Icons.Material.TwoTone.SentimentSatisfiedAlt;
                    if (Fatigue < 5)
                        return MudBlazor.Icons.Material.TwoTone.SentimentNeutral;
                    if (Fatigue < 7)
                        return MudBlazor.Icons.Material.TwoTone.SentimentDissatisfied;
                    return MudBlazor.Icons.Material.TwoTone.SentimentVeryDissatisfied;
                }
            }
            public MudBlazor.Color FatigueColor
            {
                get
                {
                    if (Fatigue <= 0)
                        return MudBlazor.Color.Success;
                    if (Fatigue < 3)
                        return MudBlazor.Color.Primary;
                    if (Fatigue < 5)
                        return MudBlazor.Color.Info;
                    if (Fatigue < 7)
                        return MudBlazor.Color.Warning;
                    return MudBlazor.Color.Error;
                }
            }
        //public string MPDisplay { get { if (PatientIDs.Count == 0) return MPBase.ToString(); return (MPBase / (PatientIDs.Count * PatientIDs.Count)).ToString(); } }
    }
}