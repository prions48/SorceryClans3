namespace SorceryClans3.Data.Models
{
    public enum ResearchDiscovery
    {
        Power,
        Artifact,
        LesserUndead,
        GreaterUndead,
        NecroArtifact,
        LesserDemon,
        GreaterDemon,
        DemonicCurse,
        SpiritSoldier,
        SpiritArtifact,
        SpiritWeather,
        BeastPet,
        BeastTame,
        BeastHarvest,
        Angel,
        Nephilim,
        AngelStatue,
        SummonFaerie,
        FaerieBargain//and a third?
    }
    //do extension spells get their own system?
    /*
    ImprovePet (done by beast tamer),
    ImproveIcon (done by Nephilim),
    ImproveNecroArtifact (done by greater undead),
    ImproveWeather (done by spirit attuned),
    ImproveFaeriesomethingorother (done by ???)
    ImproveDemonicCurse (done by greater demon)
    */
    public static class ResearchUtils
    {
        public static int PointsToScore(this int i, ResearchDiscovery disco)
        {
            switch (disco)
            {
                case ResearchDiscovery.BeastHarvest:
                case ResearchDiscovery.BeastTame:
                case ResearchDiscovery.SpiritSoldier:
                    return (i - 1000000) / 200000;
                case ResearchDiscovery.LesserUndead:
                case ResearchDiscovery.GreaterUndead:
                    return (i - 1000000) / 450000;
                case ResearchDiscovery.LesserDemon:
                case ResearchDiscovery.GreaterDemon:
                    return (i - 1000000) / 200000;
                case ResearchDiscovery.Angel:
                    int ret = (i - 1000000) / 600000;
                    if (ret > 10)
                        ret = 10;
                    return ret;
                case ResearchDiscovery.Artifact:
                    return 2 + (i - 1000000) / 1000000;
                case ResearchDiscovery.NecroArtifact:
                    return 1 + (i - 1000000) / 1500000;
                case ResearchDiscovery.Power:
                default:
                    return 1 + (i - 1000000) / 300000;
            }
        }
        public static bool NeedsCaster(this ResearchDiscovery disco)
        {
            switch (disco)
            {
                case ResearchDiscovery.GreaterUndead:
                case ResearchDiscovery.GreaterDemon:
                case ResearchDiscovery.SpiritArtifact:
                case ResearchDiscovery.BeastHarvest:
                case ResearchDiscovery.AngelStatue:
                case ResearchDiscovery.FaerieBargain:
                    return true;
                default: return false;
            }
        }
    }
}