namespace SorceryClans3.Data.Models
{
    public class GameSettings
    {
        public bool RealTime { get; set; } = false;
        public DateTime GameTime { get; set; }
        public DateTime CurrentTime
        {
            get
            {
                if (RealTime)
                    return DateTime.Now;
                return GameTime;
            }
        }
    }
}