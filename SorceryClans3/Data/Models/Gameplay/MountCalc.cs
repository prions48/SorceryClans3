namespace SorceryClans3.Data.Models
{
    public class MountCalc
    {
        public Guid SoldierID { get; set; }
        public int TravelScore { get; set; }
        public int TravelBoost { get; set; }
        public bool CanMount { get; set; }
        public bool CanBeMounted { get; set; }
        public MountCalc? MountedSoldier { get; set; }
        public MountCalc(Soldier soldier)
        {
            SoldierID = soldier.ID;
            TravelScore = soldier.Travel!.Value;
            TravelBoost = soldier.TravelGroupBoost;
            CanMount = soldier.IsIndependent;
            CanBeMounted = soldier.MountCount > 0;
        }
        public static List<MountCalc> GenerateMountCalcs(List<Soldier> soldiers)
        {
            List<MountCalc> solds = [];
            List<MountCalc> mounts = [];
            foreach (Soldier soldier in soldiers)
            {
                if (soldier.Travel == null)
                    continue;
                if (soldier.IsIndependent)
                {
                    solds.Add(new(soldier));
                }
                else if (soldier.MountCount > 0)
                {
                    for (int i = 0; i < soldier.MountCount; i++)
                    {
                        mounts.Add(new(soldier));
                    }
                }
            }
            mounts = mounts.OrderByDescending(e => e.TravelScore).ToList();
            solds = solds.OrderBy(e => e.TravelScore).ToList();
            for (int i = 0; i < mounts.Count && solds.Count > 0; i++)
            {
                mounts[i].MountedSoldier = solds[0];
                solds.RemoveAt(0);
            }
            solds.AddRange(mounts);
            return solds;
        }
    }
}