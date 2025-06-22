using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SorceryClans3.Data.Tools;

namespace SorceryClans3.Data.Models
{
    public class MissionContract : Mission
    {
        public string ContractName { get; set; }
        public int NumEvents { get; set; } = 0;
        public DateTime NextEvent { get; set; }
        public MissionContract(GameSettings settings, int seed, ClientCity client, bool important = false) : base(settings, seed, client, important)
        {
            ContractName = Names.ContractName() + client.CityName;
            Type = MissionType.ContractMisson;
            Client = client;
            Location = new(client);
            ID = Guid.NewGuid();
            Seed = seed;
            SetScore(true);
            SetColor(true); //maybe revisit allowing color for contracts later...
            SetDisp();
            SetTime(settings);
            Importance = GenerateImportance(important);
            if (Importance == ClientImportance.Important)
                Resources.BoostMoney(1.5);
            else if (Importance == ClientImportance.Critical)
                Resources.BoostMoney(3.0);
        }
        protected override void SetTime(GameSettings settings)
        {
            MissionDays = r.Next(30) + 5; //settings can do more later
            if (settings.RealTime)
            {
                ExpirationDate = settings.CurrentTime.AddDays(r.Next(8) + 3);
            }
            else
            {
                ExpirationDate = settings.CurrentTime.AddMonths(1 + r.Next(4)).AddDays(r.Next(20));
            }
        }
        public override (bool, int) CompleteMission()
        {
            var result = base.CompleteMission();
            MissionDays = r.Next(30) + 5;
            SetScore(r.Next(3) > 0);//but not disp!
            NumEvents++;
            return result;
        }
        public int PayContract()
        {
            Client.Resources.GainMoney(MoneyReward);
            return MoneyReward;
        }
        public override int ReputationBoost()
        {
            int ret = Seed;
            if (Seed > 100000)
                ret = 100000 + ((Seed - 100000) / 1000);
            return (int)((ret / 1000 + r.Next(100)) * ImportanceFactor * (10.0 + NumEvents) / 10.0);
        }
        public override int ReputationPenalty()
        {
            int ret = Seed;
            if (Seed > 100000)
                ret  = 100000 + ((Seed - 100000) / 1000);
            return (int)((ret / 100 + r.Next(1000)) * ImportanceFactor * 10.0 / (10.0 + NumEvents));
        }
    }
}