using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace SorceryClans3.Data.Models
{
    public class GameEventDisplay
    {
        [Key] public Guid ID { get; set; } = Guid.NewGuid();
        public string Message { get; set; } = "";
        public DateTime EventDate { get; set; }
        public Mission? DisplayMission { get; set; }
        public Team? DisplayTeam { get; set; }
        public Team? DisplayTeam2 { get; set; }
        public TeamResult? DisplayResult { get; set; }
        public Soldier? NewSoldier { get; set; }
        public List<Soldier> Deads { get; set; } = [];
        public bool OpenHealDialog { get; set; } = false;
        public bool OpenRescueDialog { get; set; } = false;
        //add more things
        //e.g. results of spells, bandit attacks, etc etc
        //add rewards object?

        public GameEventDisplay(string msg, DateTime date)
        {
            Message = msg;
            EventDate = date;
        }
        public bool AnyDeaths => Deads.Count > 0;
        public MarkupString DisplayDeaths
        {
            get
            {
                string result = "";
                foreach (Soldier soldier in Deads)
                {
                    if (result != "")
                        result += "<br />";
                    result += soldier.SoldierName + " has died from their battle wounds!";
                }
                return (MarkupString)result;
            }
        }
        public MarkupString DisplayText
        {
            get
            {
                string add = "";
                if (DisplayResult != null && DisplayTeam != null)
                {
                    foreach (Soldier sold in DisplayResult.HPDamage.Keys)//have to use this because dead soldiers are removed from team
                    {
                        if (sold.HPCurrent >= 0 && sold.Health < HealthLevel.Dead)
                        {
                            if (DisplayResult.HPDamage.TryGetValue(sold, out int dmg))
                            {
                                (Guid, int, bool)? gain = DisplayResult.Gains.FirstOrDefault(e => e.Item1 == sold.ID);
                                if (gain != null)
                                {
                                    add += "<br />" + sold.SoldierName + " has gained " + gain.Value.Item2 + " power and taken " + dmg + " damage, and is now " + sold.Health.ToString();
                                    if (gain.Value.Item3)
                                        add += "<br />" + sold.SoldierName + " has become tougher from this experience!";
                                }
                                else
                                {
                                    add += "<br />" + sold.SoldierName + " participated.";//testing
                                }
                            }
                            else
                            {
                                (Guid, int, bool)? gain = DisplayResult.Gains.FirstOrDefault(e => e.Item1 == sold.ID);
                                if (gain != null)
                                {
                                    add += "<br />" + sold.SoldierName + " has gained " + gain.Value.Item2 + " power" + (gain.Value.Item3 ? " and has become tougher from this experience!" : ".");
                                }
                                else
                                    add += "<br />" + sold.SoldierName + " participated.";//testing
                            }
                        }
                        else
                        {
                            add += "<br />" + sold.SoldierName + " has been killed in battle!";
                        }
                    }
                }
                return (MarkupString)(Message + add);
            }
        }
    }
}