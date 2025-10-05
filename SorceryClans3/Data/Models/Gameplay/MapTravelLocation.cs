using Azure.Storage.Blobs.Models;

namespace SorceryClans3.Data.Models
{
    public class MapTravelLocation
    {
        public MapLocation Location { get; set; }
        public MapLocation Target { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Angle { get; set; }
        public MapTravelLocation(MapLocation startpoint, MapLocation endpoint, DateTime starttime)
        {
            Location = startpoint;
            TimeStamp = starttime;
            Target = endpoint;
            Angle = startpoint.Angle(endpoint);
            //double cos = Math.Cos(Angle);
            //double sin = Math.Sin(Angle);
        }
        //hey math professors, I'm doing "calculus" lite for fun, are you proud of me now?!?! 
        public bool Iterate(int dscore, GameSettings settings)
        {
            bool reached = false;
            TimeSpan elapsedtime = settings.CurrentTime - TimeStamp;
            double distanceTraveled = settings.RealTime ? dscore * elapsedtime.Minutes : dscore * (elapsedtime.Hours / 24.0);
            double distanceToGo = Location.GetDistance(Target);
            if (distanceToGo < distanceTraveled)
            {
                reached = true;
                distanceTraveled = distanceToGo;
            }
            if (settings.RealTime)
                Location = new MapLocation(Location.X + (Math.Cos(Angle) * distanceTraveled), Location.Y + (Math.Sin(Angle) * distanceTraveled));
            else
                Location = new MapLocation(Location.X + (Math.Cos(Angle) * distanceTraveled), Location.Y + (Math.Sin(Angle) * distanceTraveled));
            TimeStamp = settings.CurrentTime;
            return reached;
        }
    }
}