using SorceryClans3.Data.Abstractions;
using SorceryClans3.Data.Models;
using SorceryClans3.Data.Tools;

public class RivalTeam
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public IMission? Mission { get; set; }
    private List<RivalSoldier> Soldiers { get; set; } = [];
    public int SoldierCount => Soldiers.Count;
    private double Teamwork { get { return _teamwork; } set { _teamwork = value; if (_teamwork < 0.1) _teamwork = 0.1; if (_teamwork > 2.5) _teamwork = 2.5; } }
    private double _teamwork = 1.5;
    public int Combat => (int)(Teamwork * Soldiers.Sum(e => e.Combat));
    public int Magic => (int)(Teamwork * Soldiers.Sum(e => e.Combat));
    public int Subtlety => (int)(Teamwork * Teamwork * Soldiers.Sum(e => e.Combat) / SoldierCount);
    public RivalTeam(int rlvl, Random r)
    {
        int sct = r.Next(2, 8);
        for (int i = sct; i >= 0; i--)
        {
            Soldiers.Add(new RivalSoldier(rlvl, i, r));
        }
    }
    public void XPGain(Random r)
    {
        Teamwork += r.NextDouble() * 0.3;
        foreach (RivalSoldier soldier in Soldiers)
            soldier.XPGain(r);
    }
    public void Damage(Random r, int hurt = 10)
    {
        for (int i = 0; i < hurt; i++)
            Soldiers[r.Next(Soldiers.Count)].Hurt();
        int x = 0;
        while (x < Soldiers.Count)
        {
            if (Soldiers[x].HP <= 0)
            {
                Soldiers.RemoveAt(x);
                Teamwork -= r.NextDouble() * 0.5;
            }
            else
                x++;
        }
    }
    public void PassTime(Random r)
    {
        foreach (RivalSoldier soldier in Soldiers)
            if (r.Next(3) == 0)
                soldier.HP++;
    }
}
public class RivalSoldier
{
    public Guid ID { get; set; } = Guid.NewGuid();
    private int PowerMax { get; set; }
    public int PowerLevel { get; set; }
    public int HP { get { return _hp; } set { _hp = value; if (_hp > HPMax) _hp = HPMax; if (_hp < 0) _hp = 0; } }
    private int _hp;
    private int HPMax { get; set; }
    private int ComBase { get; set; }
    private int MagBase { get; set; }
    private int SubBase { get; set; }
    public int Combat => (int)(PowerLevel * ComBase * HurtFactor);
    public int Magic => (int)(PowerLevel * MagBase * HurtFactor);
    public int Subtlety => (int)(PowerLevel * SubBase * HurtFactor);
    private double HurtFactor
    {
        get
        {
            if (HP <= 0)
                return 0.0;
            if (HP + 4 >= HPMax)
                return 1.0;
            if (HP * 2.0 >= HPMax)
                return 0.8;
            if (HP * 3.0 >= HPMax)
                return 0.3;
            return 0.1;
        }
    }
    public RivalSoldier(int rlvl, int tct, Random r)
    {
        PowerLevel = r.Next(2000) + 1000 + (rlvl * 500) + (tct * 2500);
        PowerMax = 1 + r.Next(10 + (rlvl / 5) + (tct / 10));
        ComBase = r.Next(3, 10 + rlvl);
        MagBase = r.Next(3, 10 + rlvl);
        SubBase = r.Next(3, 10 + rlvl);
        HPMax = 8 + r.Next(10 + (rlvl / 2)) + (tct / 3);
        HP = HPMax;
    }
    public void XPGain(Random r)
    {
        if (PowerLevel > PowerMax * 1000)
            PowerLevel += r.Next(50);
        else
            PowerLevel += r.Next(1000);
    }
    public void Hurt()
    {
        HP--;
    }
}
public class Rival : IMap, ILocated
{
    public int rank; //1 to 10
    public int numteams;
    public List<RivalTeam> Teams { get; set; } = [];
    public MapLocation Location { get; set; }
    public MudBlazor.Color Color => MudBlazor.Color.Tertiary;
    public string TooltipText => $"{RivalName} ({rank})";
    public int PerimeterRange => 2 + (Teams.Count(e => e.Mission?.MissionName == "Perimeter") / 3);
    public int avgpower;
    public int hp;
    public int NumCivilians { get; set; }
    public double Defenses { get; set; }
    public int Learning { get; set; }
    public int dispteams;
    public int disppower;
    public double known = 0.0;

    public string RivalName { get; set; }
    public string ClanName { get; set; }

    public bool Discovered { get; set; }
    public bool AtWar { get; set; }

    public List<Team> Spies { get; set; } = [];

    private Random r = new();
    public Rival(int seed, List<Rival> rl)
    {
        if (seed < 1)
            seed = 1;
        Location = new(100, seed > 5 ? 60 : 30);
        AtWar = false;
        rank = seed;
        hp = seed * 20 + (int)(r.NextDouble() * (20 + 2 * seed));
        NumCivilians = 50 + (seed * r.Next(10, 20)) + r.Next(100);
        numteams = seed * 2 + seed * seed / 2 + 10 + (int)(r.NextDouble() * 10);
        for (int q = 0; q < numteams; q++)
        {
            Teams.Add(new RivalTeam(seed, r));
        }
        Defenses = 1.0 + (r.NextDouble() * (seed / 5.0));
        Learning = 1000 + r.Next(200 * seed);
        RivalName = RandomName(rl);
        ClanName = Names.ClanName();
        known = 0.0;
        AssignTeams();
    }
    public List<TeamResult> AttemptDetect(Team team)
    {
        List<TeamResult> results = [];
        int i = 0;
        while (i < Teams.Count)
        {
            RivalTeam rivalteam = Teams[i];
            bool destroyed = false;
            if (rivalteam.Mission?.MissionName == "Perimeter")
            {
                if (r.Next(4) == 0) //?????
                {
                    if (rivalteam.Subtlety > team.SScore)
                    {
                        var result = AttackTeam(team, rivalteam);
                        results.Add(result.Item1);
                        destroyed = result.Item2;
                    }
                }
            }
            if (!destroyed)
                i++;
        }
        return results;
    }
    public GameEventDisplay AttackRival(Team team, DateTime currentTime)
    {
        List<RivalTeam> defenders = Teams.Where(e => e.Mission?.MissionName == "Perimeter" && r.Next(5) == 0).ToList();
        RivalTeam? rteam = defenders.Count == 0 ? null : defenders[r.Next(defenders.Count)];
        if (rteam == null)
        {
            return new GameEventDisplay($"Team {team.TeamName} has stormed the gates of {RivalName}!", currentTime) { DisplayTeam = team, DisplayResult = new TeamResult(team), AdditionalMessages = DamageRival(2)};
        }
        else
        {
            (TeamResult,bool) result = AttackTeam(team, rteam, Defenses);
            List<string> msgs = [];
            if (result.Item1.Success)
            {
                //random damage for now
                 msgs = DamageRival(1);
            }
            return new GameEventDisplay($"Team {team.TeamName} has battled a rival team at the gates of {RivalName} {(result.Item1.Success ? $"{(result.Item2 ? "and destroyed the team utterly" : "and triumphed")}!" : "and been defeated!")}", currentTime)
            {
                DisplayResult = result.Item1,
                DisplayTeam = team,
                AdditionalMessages = msgs
            };
        }
    }
    public (TeamResult,bool) AttackTeam(Team team, RivalTeam rteam, double boost = 1.0)
    {
        bool destroyed = false;
        bool breakout = false;
        List<Soldier> solds = team.GetAllSoldiers.ToList();
        Dictionary<Soldier, int> damage = team.GetAllSoldiers.ToDictionary(x => x, x => 0);
        bool? winning = null;
        int rds = 0;
        while (!breakout)
        {
            rds++;
            int rdmg = 0, tdmg = 0;
            double pct = rteam.Combat * boost / team.CScore;
            if (pct > 0.95 && pct < 1.05)
            {
                int tac = team.TacticsScore;
                int balance = tac > r.Next(20) ? 3 : -3;
                rdmg += r.Next(3) + balance;
                tdmg += r.Next(3) - balance;
            }
            else if (rteam.Combat * boost >= team.CScore)
            {
                int diff = rteam.Combat - team.CScore;
                int factor = 1;
                while (diff > 0)
                {
                    if (r.Next(5) == 0)
                        rdmg++;
                    tdmg++;
                    diff -= factor * 100;
                    factor *= 2;
                }
                breakout = team.TacticsScore > r.Next(100) || r.Next(10) == 0;
                if (breakout)
                    winning = false;
            }
            else //duh, winning
            {
                int diff = team.CScore - (int)(rteam.Combat * boost);
                int factor = 1;
                while (diff > 0)
                {
                    if (r.Next(5) == 0)
                        tdmg++;
                    rdmg++;
                    diff -= factor * 100;
                    factor *= 2;
                }
                breakout = r.Next(team.Leaders.Sum(e => e.Tactics)) > 10 || r.Next(10) == 0;
                if (breakout)
                    winning = true;
            }
            rteam.Damage(r, rdmg);
            destroyed = destroyed || Clean();
            for (int i = 0; i < tdmg; i++)
                damage[solds[r.Next(solds.Count)]]++;
            breakout = breakout || team.SoldierCount == 0 || rteam.SoldierCount == 0;
        }
        List<(Guid, int, bool)> gains = team.BoostSoldiers(300 + 200 * rds + r.Next(rds * 500));
        return (new TeamResult(damage, gains, winning == true), destroyed);
    }
    private List<string> DamageRival(int iter)
    {
        List<string> ret = [];
        double defdmg = 0;
        int civdead = 0, learn = 0;
        for (int i = 0; i < iter; i++)
        {
            defdmg += r.NextDouble() * 0.1;
            civdead += r.Next(10);
            learn += r.Next(100);
        }
        if (Defenses - defdmg < 1.0)
            defdmg = Defenses - 1.0;
        Defenses -= defdmg;
        if (NumCivilians - civdead < 0)
            civdead = NumCivilians;
        NumCivilians -= civdead;
        if (Learning - learn < 0)
            learn = Learning;
        Learning -= learn;
        if (civdead > 0)
            ret.Add($"{civdead} civilian{(civdead == 1 ? " was" : "s were")} killed in the attack.");
        if (defdmg > 0)
            ret.Add($"The defenses were damaged!");
        if (learn > 0)
            ret.Add("Some of the knowledge and secrets were destroyed."); //stolen?  later.... :3
        return ret;
    }
    private bool Clean()
    {
        int i = 0;
        bool any = false;
        while (i < Teams.Count)
        {
            if (Teams[i].SoldierCount == 0)
            {
                any = true;
                Teams.RemoveAt(i);
            }
            else
                i++;
        }
        return any;
    }
    private void AssignTeams()
    {
        foreach (RivalTeam team in Teams)
            team.SetAssign();
    }

    public void monthlyDrift()
    {
        known -= r.NextDouble() * .03;
        if (known < 0)
            known = 0;
    }

    public void updateDisps(double inc)
    {
        if (avgpower == 0 || known == 1.0)
            return;
        double preknown = known;
        if (known + inc >= 1.0)
        {
            known = 1.0;
            disppower = avgpower;
            return;
        }
        else
            known += inc;
        disppower += (int)((avgpower - disppower) * (inc / (1.0 - preknown)));
    }

    public bool StillAlive() //this was a triumph?
    {
        for (int tn = Teams.Count - 1; hp < 0 && tn >= 0; tn--)
        {
            hp += (3 + r.Next(4)) * Teams[tn].SoldierCount;
            Teams.RemoveAt(tn);
            if (dispteams > 0)
                dispteams--;
        }
        if (hp < 0 && avgpower > 0) //all teams are used up, plus foreign surveillance
        {
            //time to gut the enemy!
            numteams = 0;
            dispteams = 0;
            avgpower = 0; //this is how we identify a dead rival //jumping the gun tho!
            disppower = 0;
            return false;
        }
        return true;
    }

    public int TeamsLeft()
    {
        return Teams.Count(e => e.SoldierCount > 0);
    }

    public int IdleTeamsLeft()
    {
        return Teams.Count(e => e.SoldierCount > 0 && e.Mission == null);
    }

    public string RandomName(List<Rival> rl)
    {
        //The Village/ETC of ADJ NOUN
        string temp = "The ";
        switch ((int)(r.NextDouble() * 5)) {
            case 0: temp = temp + "Village of "; break;
            case 1: temp = temp + "Fortress of "; break;
            case 2: temp = temp + "Castle of "; break;
            case 3: temp = temp + "Stronghold of "; break;
            case 4: temp = temp + "Citadel of "; break;
        }
        switch ((int)(r.NextDouble() * 27)) {
            case 0: temp = temp + "Burning "; break;
            case 1: temp = temp + "Windy "; break;
            case 2: temp = temp + "Mighty "; break;
            case 3: temp = temp + "Silver "; break;
            case 4: temp = temp + "Golden "; break;
            case 5: temp = temp + "Ancient "; break;
            case 6: temp = temp + "Angry "; break;
            case 7: temp = temp + "Hungry "; break;
            case 8: temp = temp + "Wicked "; break;
            case 9: temp = temp + "Rushing "; break;
            case 10: temp = temp + "Bloody "; break;
            case 11: temp = temp + "Smoking "; break;
            case 12: temp = temp + "Glaring "; break;
            case 13: temp = temp + "Broken "; break;
            case 14: temp = temp + "Flying "; break;
            case 15: temp = temp + "Looming "; break;
            case 16: temp = temp + "Weeping "; break;
            case 17: temp = temp + "Ashen "; break;
            case 18: temp = temp + "Barren "; break;
            case 19: temp = temp + "Towering "; break;
            case 20: temp = temp + "Smoldering "; break;
            case 21: temp = temp + "Patient "; break;
            case 22: temp = temp + "Hidden "; break;
            case 23: temp = temp + "Secret "; break;
            case 24: temp = temp + "Shadowy "; break;
            case 25: temp = temp + "Swift "; break;
            case 26: temp = temp + "Armored "; break;
            default: temp = temp + "Silly "; break;
        }
        string noun = Names.RivalNoun();
        if (rl.Count < 35)
            while (ContainsName(rl, noun))
                noun = Names.RivalNoun();
        temp = temp + noun;
        return temp;
    }

    public bool ContainsName(List<Rival> list, string s)
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i] != null && list[i].RivalName.Contains(s))
                return true;
        return false;
    }

    public static int onefive(int num)
    {
        return (int)(num * Math.Sqrt(num));
    }

    //selects a random team to create a mission to disrupt a Red spellcasting
    //no need to set the team to busy or anything since the target is in/next to the Rival anyway, no travel times
    public RivalTeam? getRandTeam()
    {
        int[] ret = new int[5]; ;
        int ntav = 0;
        for (int i = 0; i < Teams.Count; i++)
            if (Teams[i].Mission == null && Teams[i].SoldierCount > 0)
                ntav++;
        if (ntav == 0)
            return null;
        ntav = (int)(r.NextDouble() * ntav) + 1;
        for (int i = 0; i < Teams.Count; i++)
        {
            if (Teams[i].Mission == null && Teams[i].SoldierCount > 0)
            {
                ntav--;
                if (ntav <= 0)
                {
                    return Teams[i];
                }
            }
        }
        return null;
    }

    public void xpGain() //monthly increases in numbers
    {
        foreach (RivalTeam team in Teams)
            team.XPGain(r);
        hp++;
    }


}