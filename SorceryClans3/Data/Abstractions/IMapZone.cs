using SorceryClans3.Data.Models;
public interface IMapZone
{
    public MapLocation Location { get; set; }
    public double X => Location.X;
    public double Y => Location.Y;
    public double SmallRadius { get; }
    public double LargeRadius { get; }
    public MudBlazor.Color Color { get; }
    public string TooltipName { get; }
}