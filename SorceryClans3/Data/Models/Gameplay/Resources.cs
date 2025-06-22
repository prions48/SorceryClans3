using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Storage;

namespace SorceryClans3.Data.Models
{
    public class Resources
    {
        public int Money { get; set; } = 0;
        public MoneyDisplay CurrentPeriod { get; set; } = new();
        public List<MoneyDisplay> History { get; set; } = [];
        public List<Artifact> Artifacts { get; set; } = [];
        public List<Beast> Beasts { get; set; } = [];
        public List<BeastHarvest> Harvests { get; set; } = [];
        public List<LesserUndead> LesserUndeads { get; set; } = [];

        //add spell fragments later
        public void TransferResources(Resources res, int remain = 0)
        {
            Money += res.Money - remain;
            Artifacts.AddRange(res.Artifacts);
            Beasts.AddRange(res.Beasts);
            Harvests.AddRange(res.Harvests);
            LesserUndeads.AddRange(res.LesserUndeads);
            res.Reset(remain); //is this OOP?
        }
        public void Reset(int remain)
        {
            Money = remain;
            Artifacts = [];
            Beasts = [];
            Harvests = [];
            LesserUndeads = [];
        }
        public int AllCount => Money + OtherCount;
        public int OtherCount
        {
            get
            {
                return Artifacts.Count + Beasts.Count + Harvests.Count + LesserUndeads.Count;
            }
        }
        public void BoostMoney(double factor)
        {
            Money = (int)(Money * factor);
        }
        public void LoseMoney(int money)
        {
            Money -= money;
            CurrentPeriod.Gain(money);
        }
        public void GainMoney(int money)
        {
            Money += money;
            CurrentPeriod.Lose(money);
        }
        public void MonthReport()
        {
            History.Add(CurrentPeriod);
            CurrentPeriod = new();
        }
    }
    public class MoneyDisplay
    {
        public int GainAmt { get; set; } = 0;
        public int LostAmt { get; set; } = 0;
        public int GainTally { get; set; } = 0;
        public int LostTally { get; set; } = 0;
        public void Gain(int m)
        {
            GainAmt += m;
            GainTally++;
        }
        public void Lose(int m)
        {
            LostAmt += m;
            LostTally++;
        }
    }
}