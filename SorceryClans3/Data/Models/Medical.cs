namespace SorceryClans3.Data.Models
{
    public class Medical
    {
        public int MedicalPowerBase { get; set; } = 0;
        public int MedicalPower { get { return (int)(MedicalPowerBase * FatigueFactor); } }
        public int MedicFatigue { get; set; }
        private double FatigueFactor
        {
            get
            {
                if (MedicFatigue < 3)
                    return 1.0;
                if (MedicFatigue < 5)
                    return .95;
                if (MedicFatigue < 7)
                    return .8;
                return 5.0 / MedicFatigue;
            }
        }
        public int HealBase { get; set; }
        public bool Assessed { get; set; } = false;
        public bool Trained { get; set; } = false;
        public Medical()
        {
            Random r = new Random();
            HealBase = r.Next(11);
        }
        public Medical(int elite)
        {
            Random r = new Random();
            HealBase = r.Next(11) + elite;
        }
        public string MedicalStatus
        {
            get
            {
                if (!Assessed)
                    return "Unknown";
                if (HealBase <= 2)
                    return "Unskilled";
                if (HealBase <= 4)
                    return "Mediocre";
                if (HealBase <= 7)
                    return "Capable";
                if (HealBase <= 9)
                    return "Talented";
                return "Exceptional";
            }
        }
        public string MedicRankText
        {
            get
            {
                if (MedicalPower <= 20)
                    return "Intern";
                if (MedicalPower <= 50)
                    return "Medic";
                if (MedicalPower <= 80)
                    return "Doctor";
                return "Surgeon";
            }
        }
        public string FatigueText
        {
            get
            {
                if (MedicFatigue <= 0)
                    return "None";
                if (MedicFatigue < 3)
                    return "Low";
                if (MedicFatigue < 5)
                    return "Medium";
                if (MedicFatigue < 7)
                    return "High";
                return "Extreme";
            }
        }
        public string FatigueIcon
        {
            get
            {
                if (MedicFatigue <= 0)
                    return MudBlazor.Icons.Material.TwoTone.SentimentVerySatisfied;
                if (MedicFatigue < 3)
                    return MudBlazor.Icons.Material.TwoTone.SentimentSatisfiedAlt;
                if (MedicFatigue < 5)
                    return MudBlazor.Icons.Material.TwoTone.SentimentNeutral;
                if (MedicFatigue < 7)
                    return MudBlazor.Icons.Material.TwoTone.SentimentDissatisfied;
                return MudBlazor.Icons.Material.TwoTone.SentimentVeryDissatisfied;
            }
        }
        public MudBlazor.Color FatigueColor
        {
            get
            {
                if (MedicFatigue <= 0)
                    return MudBlazor.Color.Success;
                if (MedicFatigue < 3)
                    return MudBlazor.Color.Primary;
                if (MedicFatigue < 5)
                    return MudBlazor.Color.Info;
                if (MedicFatigue < 7)
                    return MudBlazor.Color.Warning;
                return MudBlazor.Color.Error;
            }
        }
        public int GainMedicPower(int reps = 1)
        {
            Random r = new Random();
            int z = 0;
            for (int i = 0; i < reps; i++)
            {
                if (CanTrain && r.NextSingle() < HealBase * 0.1)
                {
                    MedicalPowerBase++;
                    z++;
                }
            }
            return z;
        }
        public bool CanTrain => MedicalPowerBase < HealBase * 10 && MedicalPowerBase < 100;
    }
}