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
    public int HP { get { return _hp; } set { if (_hp <= 0) return; _hp = value; if (_hp > HPMax) _hp = HPMax; } }
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
    /************
	Relative power of rivals
	rank 1: 8 teams of 100k C, defenses at 100k
	rank 2: 12 teams of 300k C, defenses at 500k
	rank 5: 35 teams of 1M C, defenses at 3M
	rank 10: 100 teams of 5M, defenses at 50M
	*************/
    public int numteams;
    private int seed;
    public List<RivalTeam> Teams { get; set; } = [];
    public MapLocation Location { get; set; }
    public MudBlazor.Color Color => MudBlazor.Color.Tertiary;
    public string TooltipText => $"{rname} ({seed})";
    public int avgpower;
    public int hp;

    public int dispteams;
    public int disppower;
    public double known;

    public string rname;
    public string cfname;

    public bool Discovered { get; set; }
    public bool AtWar { get; set; }

    public int watchers;
    public int[] teamwatcher;

    private Random r = new();
    public Rival(int s, List<Rival> rl)
    {
        seed = s;
        Location = new(100, seed > 5 ? 60 : 30);
        AtWar = false;
        watchers = -1;
        teamwatcher = new int[10];
        rank = seed;
        hp = seed * 20 + (int)(r.NextDouble() * (20 + 2 * seed));
        numteams = seed * 2 + seed * seed / 2 + 10 + (int)(r.NextDouble() * 10);
        for (int q = 0; q < numteams; q++)
        {
            Teams.Add(new RivalTeam(seed, r));
        }

        rname = RandomName(rl);
        cfname = Names.ClanName();
        known = 0.0;
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
        String noun = GetNoun();
        while (ContainsName(rl, noun))
            noun = GetNoun();
        temp = temp + noun;
        return temp;
    }

    public bool ContainsName(List<Rival> list, string s)
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i] != null && list[i].rname.Contains(s))
                return true;
        return false;
    }

    public string GetNoun()
    {
        string temp = "";
        switch ((int)(r.NextDouble() * 36)) {
            case 0: temp = temp + "Dragons"; break;
            case 1: temp = temp + "Dreams"; break;
            case 2: temp = temp + "Ice"; break;
            case 3: temp = temp + "Storms"; break;
            case 4: temp = temp + "Towers"; break;
            case 5: temp = temp + "Lights"; break;
            case 6: temp = temp + "Stone"; break;
            case 7: temp = temp + "Jaws"; break;
            case 8: temp = temp + "Eyes"; break;
            case 9: temp = temp + "Pain"; break;
            case 10: temp = temp + "Nails"; break;
            case 11: temp = temp + "Horns"; break;
            case 12: temp = temp + "Claws"; break;
            case 13: temp = temp + "Walls"; break;
            case 14: temp = temp + "Veils"; break;
            case 15: temp = temp + "Fires"; break;
            case 16: temp = temp + "Clouds"; break;
            case 17: temp = temp + "Arms"; break;
            case 18: temp = temp + "Demons"; break;
            case 19: temp = temp + "Spirits"; break;
            case 20: temp = temp + "Jewels"; break;
            case 21: temp = temp + "Anvils"; break;
            case 22: temp = temp + "Feathers"; break;
            case 23: temp = temp + "Sands"; break;
            case 24: temp = temp + "Trees"; break;
            case 25: temp = temp + "Horses"; break;
            case 26: temp = temp + "Tigers"; break;
            case 27: temp = temp + "Trumpets"; break;
            case 28: temp = temp + "Mountains"; break;
            case 29: temp = temp + "Blades"; break;
            case 30: temp = temp + "Spears"; break;
            case 31: temp = temp + "Lanterns"; break;
            case 32: temp = temp + "Crowns"; break;
            case 33: temp = temp + "Nights"; break;
            case 34: temp = temp + "Watchers"; break;
            case 35: temp = temp + "Spies"; break;
            default: temp = temp + "Teletubbies"; break;
        }
        return temp;
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