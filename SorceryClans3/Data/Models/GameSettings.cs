namespace SorceryClans3.Data.Models
{
    public class GameSettings
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        //need to handle time zones at some point don't forget
        public bool RealTime { get; set; } = false;
        public DateTime GameTime { get; set; }
        public Random r { get; set; } = new();
        public DateTime CurrentTime
        {
            get
            {
                if (RealTime)
                    return DateTime.Now;
                return GameTime;
            }
        }
        public TimeSpan ElapsedGameTime
        {
            get
            {
                return CurrentTime - StartDate;
            }
        }
        #region Time Calcs
        public DateTime MissionEndTime(Mission mission)
        {
            if (RealTime)
                return CurrentTime.AddMinutes((mission.TravelDistance * 1.0 / mission.AttemptingTeam!.DScore) + mission.MissionDays);
            return CurrentTime.AddDays((mission.TravelDistance * 1.0 / mission.AttemptingTeam!.DScore) + mission.MissionDays);
        }
        public DateTime BanditTime()
        {
            if (RealTime)
                return CurrentTime.AddMinutes(r.Next(4) + 2);
            return CurrentTime.AddDays(r.Next(5) + 10);
        }
        #endregion

        #region Probabilities
        public bool BanditAttackOdds()
        {
            //in the future, use the ElapsedTime to affect odds
            if (RealTime)
                return r.NextDouble() < 0.005; //1 per 100 seconds (obv for testing purposes for now) //probably come up with something more clever for this
            return r.NextDouble() < .01; //1 per month~ (given 4 events per day)
        }
        #endregion
    }
}