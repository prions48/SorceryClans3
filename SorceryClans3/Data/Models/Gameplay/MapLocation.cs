using MathNet.Numerics.Distributions;
namespace SorceryClans3.Data.Models
{
    public enum Direction
    {
        Xpos,
        Xneg,
        Ypos,
        Yneg
    }
    public class MapLocation
    {
        public string? LocationName { get; set; } = null;
        public double X { get; set; }
        public double Y { get; set; }
        public static MapLocation HomeBase
        {
            get
            {
                return new(0.0, 0.0) { LocationName = "home base"};
            }
        }
        public MapLocation()
        {
            Random r = new Random();
            X = r.NextDouble() * 200 - 100;
            Y = r.NextDouble() * 200 - 100;
        }
        public MapLocation(int range, int min)
        {
            Random r = new Random();
            X = ((r.NextDouble() * (range - min)) + min) * (1 - (r.Next(2) * 2));
            Y = ((r.NextDouble() * (range - min)) + min) * (1 - (r.Next(2) * 2));
        }
        public MapLocation(double x, double y)
        {
            X = x;
            Y = y;
        }
        public MapLocation(ClientCity client)
        {
            LocationName = client.CityName;
            X = Normal.Sample(client.Location.X, 10.0 + client.CityLevel);
            Y = Normal.Sample(client.Location.Y, 10.0 + client.CityLevel);
        }
        public void MoveUp(double step = 1.0)
        {
            Y += step;
            if (Y > 100)
                Y = 100;
        }
        public void MoveDown(double step = 1.0)
        {
            Y -= step;
            if (Y < -100)
                Y = -100;
        }
        public void MoveLeft(double step = 1.0)
        {
            X -= step;
            if (X < -100)
                X = -100;
        }
        public void MoveRight(double step = 1.0)
        {
            X += step;
            if (X > 100)
                X = 100;
        }
    }
    public static class MapUtils
    {
        private static Random random = new Random();
        public static double GetDistance(this MapLocation location1)
        {
            return Math.Sqrt(SQ(location1.X) + SQ(location1.Y));
        }
        public static double GetDistance(this MapLocation location1, MapLocation location2)
        {
            return Math.Sqrt(SQ(location1.X - location2.X) + SQ(location1.Y - location2.Y));
        }
        /// <summary>
        /// Midpoint function will eventually be replaced by some fancy trigonometry to identify the interception point
        /// </summary>
        /// <param name="location1"></param>
        /// <param name="location2"></param>
        /// <returns></returns>
        public static MapLocation GetMidpoint(this MapLocation location1, MapLocation location2)
        {
            return new MapLocation((location1.X + location2.X) / 2, (location1.Y + location2.Y) / 2);
        }
        private static double SQ(double x)
        {
            return x * x;
        }
        public static Direction GetDirection(this MapLocation location1, MapLocation location2)
        {
            double xdir = location2.X - location1.X;
            double ydir = location2.Y - location1.Y;
            if (Math.Abs(xdir) / (Math.Abs(xdir) + Math.Abs(ydir)) > random.NextDouble())
            {
                if (xdir >= 0)
                    return Direction.Xpos;
                else
                    return Direction.Xneg;
            }
            else
            {
                if (ydir >= 0)
                    return Direction.Ypos;
                else
                    return Direction.Yneg;
            }
        }
    }
}