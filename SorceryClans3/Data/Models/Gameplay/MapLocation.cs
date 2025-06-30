using System.Runtime.CompilerServices;
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
        public static MapLocation GetMidpoint(this MapLocation location1, MapLocation location2)
        {
            return new MapLocation((location1.X + location2.X) / 2, (location1.Y + location2.Y) / 2);
        }
        //most important use case currently: determine whether potential rescuers will catch up with team before it gets to its target
        public static double TravelTimeUnits(this MapLocation location1, Team team)
        {
            if (team.DScore == 0)
                return 0;
            double distance = location1.GetDistance(team.Location ?? MapLocation.HomeBase);
            return distance / team.DScore;
        }
        public static MapLocation? InterceptionPoint(this MapLocation startpt, Team rescuee, Team rescuer)
        {
            if (rescuer.DScore == 0 || rescuee.DScore == 0)
                return rescuee.BaseLocation; //?? //this is your reminder to delete empty teams

            //let's give this a try
            //and by "this" I mean https://officialtwelve.blogspot.com/2015/08/projectile-interception.html
            double travel = startpt.GetDistance(rescuee.BaseLocation);
            double origtravel = startpt.GetDistance(rescuer.BaseLocation);
            double traveltime = travel / rescuee.DScore; //cutoff time

            double? rescuetime = Quadratic(SQ(rescuee.DScore) - SQ(rescuer.DScore),
                                           rescuee.DScore * (-2 * origtravel * Math.Cos(startpt.Angle(rescuee.BaseLocation, rescuer.BaseLocation))),
                                           SQ(origtravel));
            if (rescuetime == null)
                return null;
            if (rescuetime > traveltime)
                return null;

            //classic Alejandro, I focused on getting the time and didn't think about how
            //calcualting the position at said time would also involve math.  Let me see if
            //I can cook this part up without any help.  Just me and my geometry skills.
            double ratio = rescuetime.Value / traveltime;
            return new MapLocation(startpt.X + (rescuee.BaseLocation.X - startpt.X) * ratio, startpt.Y + (rescuee.BaseLocation.Y - startpt.Y) * ratio);
            //ok that wasn't even hard now that I'm looking at it.  BUT I MADE IT IN MY HEAD FROM SCRATCH
            //ARE YOU PROUD OF ME NOW DAD???
        }
        //returns null for imaginary results, returns larger of two results
        private static double? Quadratic(double a, double b, double c)
        {
            double disc = b * b - 4 * a * c;
            if (disc < 0)
                return null;
            double v1 = (-1 * b + Math.Sqrt(disc)) / (2 * a);
            double v2 = (-1 * b - Math.Sqrt(disc)) / (2 * a);
            return Math.Max(v1, v2);
        }
        private static double Angle(this MapLocation center, MapLocation location1, MapLocation location2)
        {
            return Math.Atan2(location2.Y - center.Y, location2.X - center.X) - Math.Atan2(location1.Y - center.Y, location1.X - center.X);
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