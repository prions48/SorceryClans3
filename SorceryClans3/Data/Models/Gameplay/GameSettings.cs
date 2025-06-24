namespace SorceryClans3.Data.Models
{
    public class GameSettings
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        //need to handle time zones at some point don't forget
        public bool RealTime { get; set; } = false;
        public DateTime GameTime { get; set; }
        public Random r { get; set; } = new();
        public DateTime NextHeal { get; set; }
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
        public bool BypassDay1 { get { if (RealTime) return true; return !(GameTime.Day == 1 && GameTime.Hour == 0); } }
        #region Time Calcs
        public DateTime MissionEndTime(Mission mission)
        {
            if (RealTime)
                return CurrentTime.AddMinutes(mission.MissionDays).Add(TravelTime(mission));
            return CurrentTime.AddDays(mission.MissionDays).Add(TravelTime(mission));
        }
        public DateTime MissionTravelTime(Mission mission)
        {
            return CurrentTime.Add(TravelTime(mission));
        }
        public DateTime TravelCompletion(MapLocation loc1, MapLocation loc2, int dscore)
        {
            return CurrentTime.Add(TravelTime(loc1, loc2, dscore));
        }
        public TimeSpan TravelTime(Mission mission)
        {
            if (mission.AttemptingTeam == null)
                return new(0);
            return TravelTime(mission.Location, mission.AttemptingTeam.Location ?? MapLocation.HomeBase, mission.AttemptingTeam.DScore);
        }
        public TimeSpan TravelTime(MapLocation loc1, MapLocation loc2, int dscore)
        {
            if (dscore == 0)
                return new(0);
            double distance = loc1.GetDistance(loc2);
            if (RealTime)
                return new TimeSpan(0, (int)Math.Ceiling(distance / dscore), 0);
            return new TimeSpan((int)Math.Ceiling(distance / dscore), 0, 0, 0);
        }
        public TimeSpan TravelTime(ClientCity city, int dscore)
        {
            if (dscore == 0)
                return new(0);
            double distance = city.Location.GetDistance(MapLocation.HomeBase);
            if (RealTime)
                return new TimeSpan(0, (int)Math.Ceiling(distance / dscore), 0);
            return new TimeSpan((int)Math.Ceiling(distance / dscore), 0, 0, 0);
        }
        public TimeSpan TravelTime(ClientCity city, Team team)
        {
            if (team.DScore == 0)
                return new(0);
            return TravelTime(city, team.DScore);
        }
        public DateTime TravelCompletion(ClientCity city, Team team)
        {
            return CurrentTime.Add(TravelTime(city, team));
        }
        public DateTime BanditTime()
        {
            if (RealTime)
                return CurrentTime.AddMinutes(r.Next(4) + 2);
            return CurrentTime.AddDays(r.Next(5) + 10);
        }
        public DateTime PayDay(MissionContract contract)
        {
            if (RealTime)
                return CurrentTime.AddDays(1);
            return CurrentTime.AddMonths(1);
        }
        public DateTime BuildTime(DefenseType type)
        {
            switch (type)
            {
                case DefenseType.WatchTower: return RealTime ? CurrentTime.AddHours(4) : CurrentTime.AddDays(15);
                case DefenseType.SafeHouse: return RealTime ? CurrentTime.AddHours(4) : CurrentTime.AddDays(15);
                default: return CurrentTime;
            }
        }
        public DateTime DefenseDate(DefenseType type)
        {
            switch (type)
            {
                case DefenseType.WatchTower: return RealTime ? CurrentTime.AddHours(r.Next(6) + 12) : CurrentTime.AddDays(20 + r.Next(20));
                case DefenseType.SafeHouse: return RealTime ? CurrentTime.AddHours(r.Next(6) + 12) : CurrentTime.AddDays(20 + r.Next(20));
                default: return RealTime ? CurrentTime.AddHours(r.Next(6) + 12) : CurrentTime.AddDays(20 + r.Next(20));
            }
        }
        public DateTime TrainingDate()
        {
            if (RealTime)
                return CurrentTime.AddHours(15 + r.Next(6));
            return CurrentTime.AddDays(15 + r.Next(10)); //I'm just making these up at random right now
        }
        #region Healing
        public void SetNextHeal()
        {
            if (RealTime)
                NextHeal = CurrentTime.AddHours(1);//no clue if this is right
            NextHeal = CurrentTime.AddHours(6);
        }
        public bool HealTime => CurrentTime >= NextHeal;
        #endregion
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

        #region Config - limits
        public int MaxMissions(int lvl)
        {
            return 15 + lvl * 3;
        }
        public int MaxContracts(int lvl)
        {
            return 2 + lvl / 2;
        }
        #endregion

        #region Contracts
        public string ContractDisplay()
        {
            if (RealTime)
                return "Daily";
            return "Monthly";
        }
        #endregion
    }
}