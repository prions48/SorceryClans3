namespace SorceryClans3.Data.Models
{
    public interface ILocated
    {
        public MapLocation Location { get; set; }
    }
    public static class GameUtils
    {
        public static void NormalizeLocations(this List<ILocated> items, int numreps = 3)
        {
            //map normalizing algorithm lol
            for (int i = 0; i < numreps; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    double closest = 500;
                    int closestindex = -1;
                    for (int k = 0; k < items.Count; k++)
                    {
                        if (j == k)
                            continue;
                        double distance = items[j].Location.GetDistance(items[k].Location);
                        if (distance < closest)
                        {
                            closest = distance;
                            closestindex = k;
                        }
                    }
                    if (closestindex == -1)
                        continue;
                    Direction dir = items[j].Location.GetDirection(items[closestindex].Location);
                    switch (dir)
                    {
                        case Direction.Ypos: items[j].Location.MoveDown(); break;
                        case Direction.Yneg: items[j].Location.MoveUp(); break;
                        case Direction.Xpos: items[j].Location.MoveLeft(); break;
                        case Direction.Xneg: items[j].Location.MoveRight(); break;
                    }
                }
            }
        }
    }
}