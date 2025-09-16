using SorceryClans3.Data.Models;

namespace SorceryClans3.Data.Abstractions
{
    public interface IMap
    {
        public MapLocation Location { get; set; }
        public double X => Location.X;
        public double Y => Location.Y;
        public MudBlazor.Color Color { get; }
        public string TooltipText { get; }
    }
}