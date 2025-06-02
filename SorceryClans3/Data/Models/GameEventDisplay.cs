using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Models
{
    public class GameEventDisplay
    {
        [Key] public Guid ID { get; set; }
        public string Message { get; set; } = "";
        public Mission? DisplayMission { get; set; }
        public Team? DisplayTeam { get; set; }
        public TeamResult? DisplayResult { get; set; }
        //add more things
        //e.g. results of spells, bandit attacks, etc etc
        //add rewards object?

        public GameEventDisplay(string msg)
        {
            Message = msg;
        }
        public string DisplayText
        {
            get
            {
                string add = "";
                if (DisplayResult != null)
                {
                    foreach (KeyValuePair<Soldier, int> hp in DisplayResult.HPDamage)
                    {
                        if (hp.Value > 0)
                        {
                            add += "\n" + hp.Key.SoldierName + " took " + hp.Value + " dmg and is currently " + hp.Key.Health.ToString();
                        }
                    }
                }
                return Message + add;
            }
        }
    }
}