using SorceryClans3.Data.Tools;
using SorceryClans3.Data.Abstractions;
using SorceryClans3.Data.Models;
using SorceryClans3.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using MudBlazor.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MathNet.Numerics.Random;

namespace SorceryClans3.Data.Models
{
    public class Soldier : IHP
    {
        public Guid ID { get; set; }
        public string ClanName { get; set; }
        public string GivenName { get; set; }
        public string SoldierName { get { return ClanName + " " + GivenName; } }
        public SoldierType Type { get; set; } = SoldierType.Standard;
        public int PowerLevel { get; set; }
        private int PowerLimit { get; set; }
        public int ComBase { get; set; }
        public int MagBase { get; set; }
        public int SubBase { get; set; }
        public int HPBase { get; set; }
        public double LeadershipXP { get; set; }
        public bool LeadAssessed { get; set; } = false;
        public double MaxTeach { get; set; }
        public double TeachSkill { get; set; }
        public int LeadTrainRemains { get; set; }
        public double CounterIntelMax { get; set; }
        public double CounterIntelSkill { get; set; }
        public bool IsLeading { get; set; } = false;
        public int CharismaBase { get; set; }
        public int LogisticsBase { get; set; }
        public int TacticsBase { get; set; }
        public int IntegrityBase { get; set; }
        public int? TravelBase { get; set; }
        private int _hp;
        public int HPCurrent
        {
            get
            {
                return _hp;
            }
            set
            {
                if (value > HPMax)
                    _hp = HPMax;
                else
                    _hp = value;
            }
        }
        private int _int;
        public int IntegrityCurrent
        {
            get
            {
                return _int;
            }
            set
            {
                if (value > IntegrityMax)
                    _int = IntegrityMax;
                else
                    _int = value;
            }
        }
        public HealthLevel Health { get; set; } = HealthLevel.Uninjured;
        public Guid ClanID { get; set; }
        //public Guid TeamID { get; set; }
        public Team? Team { get; set; } //testing, might break JSONs?
        public Power? Power { get; set; }
        public Artifact? Artifact { get; set; }
        public Medical? Medical { get; set; }
        public List<Style> Styles { get; set; } = new List<Style>();
        public IDictionary<Guid, double> Teamwork { get; set; } = new Dictionary<Guid, double>();
        public IDictionary<MagicColor, double> ResearchSkill { get; set; } = new Dictionary<MagicColor, double>();
        public double ResearchAffinity { get; set; }
        public int TeamWeight
        {
            get //how much work to lead
            {
                if (SubTo != null || !Type.Independent())
                    return 0;
                return 1 + (PowerLevel / 1000);
            }
        }
        public List<Soldier> SubSoldiers { get; set; } = new List<Soldier>();
        public Soldier? SubTo { get; set; }
        public Guid? TypeID { get; set; }
        public List<Guid> Boosts { get; set; } = new List<Guid>();
        public bool IsSub { get { return SubTo != null; } }
        Random r = new();
        public Soldier()
        {
            ID = Guid.NewGuid();
            ClanName = "Unknown";
            GivenName = "Soldier";
            r = new Random();
            PowerLevel = r.Next(100, 5000);
            ComBase = r.Next(11);
            MagBase = r.Next(11);
            SubBase = r.Next(11);
            CharismaBase = r.Next(11);
            LogisticsBase = r.Next(11);
            TacticsBase = r.Next(11);
            IntegrityBase = r.Next(4, 8);
            SetSkills();
            CalcLimit();
            TravelBase = 5 + r.NextDouble() < .6 ? (r.Next(3) - 1) : r.Next(2);
            Medical = new Medical();
            HPBase = r.Next(5, 15);
            HPCurrent = HPMax;
        }
        private void SetSkills()
        {
            LeadershipXP = r.NextDouble() * 1.5 - 1.0;
            LeadTrainRemains = r.Next(4) + (r.Next(2) == 0 ? r.Next(8) : r.Next(2));
            MaxTeach = r.NextDouble() * 2; //linked to Charisma
            ResearchAffinity = r.NextDouble() + 0.5; //linked to Logistics
            CounterIntelMax = r.NextDouble();
            if (MaxTeach < .7 && ResearchAffinity < 0.8 && CounterIntelMax < .5)
            {
                switch (r.Next(3))
                {
                    case 0: MaxTeach = .7 + r.NextDouble() * 1.3; if (CharismaBase < 6) CharismaBase = 6 + r.Next(5); break;
                    case 1: ResearchAffinity = .8 + r.NextDouble() * 0.7; if (LogisticsBase < 6) LogisticsBase = 6 + r.Next(5); break;
                    case 2: CounterIntelMax = 0.5 + r.NextDouble() * 0.5; if (TacticsBase < 6) TacticsBase = 6 + r.Next(5); break;
                }
            }
            TeachSkill = MaxTeach * r.NextDouble();
            CounterIntelSkill = CounterIntelMax * r.NextDouble();
        }
        public Soldier(Clan clan)
        {
            ID = Guid.NewGuid();
            ClanID = clan.ID;
            ClanName = clan.ClanName;
            GivenName = Names.SoldierName();
            PowerLevel = r.Next(100, 5000);
            Medical = new Medical(clan.HealElite);
            ComBase = r.Next(11) + clan.ComElite;
            MagBase = r.Next(11) + clan.MagElite;
            SubBase = r.Next(11) + clan.SubElite;
            TravelBase = 10 + (r.NextDouble() < .6 ? (r.Next(3) - 1) * r.Next(3) : r.Next(3));
            CharismaBase = r.Next(11);
            LogisticsBase = r.Next(11);
            TacticsBase = r.Next(11);
            IntegrityBase = r.Next(4, 8);
            SetSkills();
            CalcLimit();
            HPBase = r.Next(5, 15) + clan.HPElite;
            HPCurrent = HPMax;
            Power = clan.Power?.GeneratePower();
            if (clan.Style != null && clan.Style.MinReqs.IsAbove(this))
            {
                Styles.Add(clan.Style.CreateStyle());
            }
        }
        public List<MagicColor> GetColors
        {
            get
            {
                if (Power == null)
                    return new List<MagicColor>();
                return Power.GetColors(PowerLevel);
            }
        }
        public int Combat
        {
            get
            {
                return ComBase + (Power?.CBonus ?? 0) + (Artifact?.ComBoost ?? 0) + Styles.Select(e => e.CBonus).Sum();
            }
        }
        public int ComNat
        {
            get
            {
                return ComBase + (Power?.CBonus ?? 0) + Styles.Sum(e => e.CBonus);
            }
        }
        public int MagNat
        {
            get
            {
                return MagBase + (Power?.MBonus ?? 0) + Styles.Sum(e => e.MBonus);
            }
        }
        public int SubNat
        {
            get
            {
                return SubBase + (Power?.SBonus ?? 0) + Styles.Sum(e => e.SBonus);
            }
        }
        public int HPNat
        {
            get
            {
                return HPBase + (Power?.HBonus ?? 0);
            }
        }
        public int Magic
        {
            get
            {
                return MagBase + (Power?.MBonus ?? 0) + (Artifact?.MagBoost ?? 0) + Styles.Select(e => e.MBonus).Sum();
            }
        }
        public int Subtlety
        {
            get
            {
                return SubBase + (Power?.SBonus ?? 0) + (Artifact?.SubBoost ?? 0) + Styles.Select(e => e.SBonus).Sum();
            }
        }
        public int Charisma
        {
            get
            {
                if (!IsLeading)
                    return 0;
                return CharismaBase + (Artifact?.ChaBoost ?? 0);
            }
        }
        public int Logistics
        {
            get
            {
                if (!IsLeading)
                    return 0;
                return LogisticsBase + (Artifact?.LogBoost ?? 0);
            }
        }
        public int Tactics
        {
            get
            {
                if (!IsLeading)
                    return 0;
                return TacticsBase + (Artifact?.TacBoost ?? 0);
            }
        }
        private int RevCha
        {
            get
            {
                int cha = Charisma;
                if (cha < 10)
                    return 10 - cha;
                return 0;
            }
        }
        private int RevLog
        {
            get
            {
                int log = Logistics;
                if (log < 10)
                    return 10 - log;
                return 0;
            }
        }
        private int RevTac
        {
            get
            {
                int tac = Tactics;
                if (tac < 10)
                    return 10 - tac;
                return 0;
            }
        }
        public int HPMax
        {
            get
            {
                return HPBase + (Power?.HBonus ?? 0) + (Artifact?.HPBoost ?? 0);
            }
        }
        public int IntegrityMax
        {
            get
            {
                return IntegrityBase; //add artifact boost at some point?
            }
        }
        public int? Travel
        {
            get
            {
                if (TravelBase == null)
                    return null;
                return (int)(HurtFactor * (TravelBase.Value + (Power?.DBonus ?? 0))); //add artifact here soon
            }
        }
        public int TravNat
        {
            get
            {
                if (TravelBase == null)
                    return 0;
                return TravelBase.Value + (Power?.DBonus ?? 0);
            }
        }
        public int TravelGroupBoost
        {
            get
            {
                if (Power == null)
                    return 0;
                return Power.GBonus;
            }
        }
        public bool IsHealer { get { return Medical != null && Medical.Trained; } }
        public bool IsInjured { get { return HPCurrent < HPMax || Health != HealthLevel.Uninjured; } }
        private double HurtFactor
        {
            get
            {
                switch (Health)
                {
                    case HealthLevel.Uninjured: return 1.0;
                    case HealthLevel.Hurt: return 1.0;
                    case HealthLevel.Wounded: return 0.8;
                    case HealthLevel.Critical: return 0.5;
                    default: return 0;
                }
            }
        }
        public int HealNeeded
        {
            get
            {
                if (HPCurrent >= HPMax || HPCurrent < 0 || Health == HealthLevel.Dead)
                    return 0;
                int hlvl = (int)Health;
                return 50 * (hlvl + 1);
            }
        }
        public int HealBase
        {
            get
            {
                if (Medical == null || !Medical.Assessed || !Medical.Trained)
                    return 0;
                return Medical.HealBase;
            }
        }
        public int HealNat
        {
            get
            {
                if (Medical == null || !Medical.Assessed || !Medical.Trained)
                    return 0;
                return Medical.HealBase + (Power?.KBonus ?? 0) + Styles.Sum(e => e.KBonus);
            }
        }
        public int? Heal
        {
            get
            {
                if (Medical == null || !Medical.Assessed || !Medical.Trained)
                    return null;
                return Medical.HealBase + (Power?.KBonus ?? 0) + (Artifact?.HealBoost ?? 0) + Styles.Select(e => e.KBonus).Sum();
            }
        }
        public int HealScore
        {
            get
            {
                if (Medical == null || !Medical.Assessed || !Medical.Trained)
                    return 0;
                return (Heal ?? 0) * Medical.MedicalPower;
            }
        }
        public int ComLeadership
        {
            get
            {
                if (!Type.Independent() || !IsLeading)
                    return 0;
                if (LeadershipXP < 0)
                    return (int)((RevCha * 2 + RevTac + (RevLog / 2)) * LeadershipXP);
                return (int)((Charisma * 2 + Tactics + (Logistics / 2)) * LeadershipXP);
            }
        }
        public int MagLeadership
        {
            get
            {
                if (!Type.Independent() || !IsLeading)
                    return 0;
                if (LeadershipXP < 0)
                    return (int)((RevLog * 2 + RevCha + (RevTac / 2)) * LeadershipXP);
                return (int)((Logistics * 2 + Charisma + (Tactics / 2)) * LeadershipXP);
            }
        }
        public int SubLeadership
        {
            get
            {
                if (!Type.Independent() || !IsLeading)
                    return 0;
                if (LeadershipXP < 0)
                    return (int)((RevTac * 2 + RevLog + (RevCha / 2)) * LeadershipXP);
                return (int)((Tactics * 2 + Logistics + (Charisma / 2)) * LeadershipXP);
            }
        }
        public double GetSkill(MagicColor color)
        {
            if (ResearchSkill.ContainsKey(color))
            {
                return ResearchSkill[color];
            }
            return ResearchAffinity / 3.0;
        }
        public double IncrementSkill(MagicColor color)
        {
            if (ResearchSkill.ContainsKey(color))
            {
                ResearchSkill[color] += r.NextDouble() * 0.04 + 0.02;
                if (ResearchSkill[color] > ResearchAffinity)
                    ResearchSkill[color] = ResearchAffinity;
                return ResearchSkill[color];
            }
            return ResearchAffinity / 3.0;
        }
        public (Guid, int, bool) GainPower(int factor)
        {
            int pg = ModifyGain(factor);
            bool hp = false;
            if ((pg * 1.0 / PowerLevel > 0.15 || (pg > 200 && r.Next(2) == 0) || r.Next(8) == 0) && r.Next(15 + Combat) > HPBase - 10)
            {
                HPBase++;
                HPCurrent++;
                hp = true;
            }
            PowerLevel += pg;
            LevelLead();
            LevelMedic();
            return (ID, pg, hp);
        }
        public void LevelLead(int additional = 10, double boost = 0.08)
        {
            LevelExtraLeads(additional);
            if (!IsLeading)
                return;
            if (LeadershipXP < .5)
                LeadershipXP += (r.NextDouble() * boost) + 0.02;
            else if (LeadershipXP < .8)
                LeadershipXP += (r.NextDouble() * boost / 2) + 0.01;
            else
                LeadershipXP += r.NextDouble() * boost / 4;
            if (LeadershipXP > 1.0)
                LeadershipXP = 1.0;
        }
        private void LevelExtraLeads(int additional = 10)
        {
            if (additional > 1 && r.Next(additional) == 0)
            {
                CounterIntelSkill += r.NextDouble() * 0.05 + 0.02;
                if (CounterIntelSkill > CounterIntelMax)
                    CounterIntelSkill = CounterIntelMax;
            }
            if (additional > 1 && r.Next(additional) == 0)
            {
                TeachSkill += r.NextDouble() * 0.05 + 0.02;
                if (TeachSkill > MaxTeach)
                    TeachSkill = MaxTeach;
            }
        }
        public void TrainLead()
        {
            if (IsLeading)
                return;
            LeadAssessed = true;
            if (LeadTrainRemains > 0)
            {
                switch (r.Next(3))
                {
                    case 0: if (CharismaBase < 10) CharismaBase++; break;
                    case 1: if (LogisticsBase < 10) LogisticsBase++; break;
                    default: if (TacticsBase < 10) TacticsBase++; break;
                }
                LeadTrainRemains--;
            }
        }
        private void LevelMedic(int odds = 4)
        {
            if (IsHealer && r.Next(odds) == 0)
            {
                Medical!.GainMedicPower();
            }
        }
        private int ModifyGain(int pg)
        {
            int lvl = (int)(pg / (3 + 2 * Math.Log10(PowerLevel + 1)));
            lvl = Type.PowerAdj(lvl);
            if (lvl == 0)
                return 0;
            if (Power != null)
                lvl = (int)(0.85 * lvl);
            if (PowerLevel > 1000 && lvl > PowerLevel)
                lvl = PowerLevel + (lvl - PowerLevel) / 5;
            if (PowerLevel > 750 * PowerLimit)
                lvl /= 2;
            if (PowerLevel > 950 * PowerLimit)
                lvl /= 3;
            if (PowerLevel > 1100 * PowerLimit)
                lvl /= 5;
            if (PowerLevel > 1200 * PowerLimit)
                lvl /= 10;
            while (lvl > 1000)
                lvl = (int)(lvl * 0.7);
            return lvl;
        }
        public void Hurt(int hp)
        {
            double pcthurt = hp * 1.0 / HPMax;
            HPCurrent -= hp;
            //if ((pcthurt - 0.1) / 0.7 > r.NextDouble())
            //Health = Health.Hurt(r.Next(2)+1);
            if (HPCurrent * 1.0 / HPMax < (1.0 - ((int)(Health + 1) * .25)))
                Health = Health.Hurt();
            if (hp * r.NextDouble() > 3.0)
                Health = Health.Hurt();
            if (HPCurrent > 0 && Health == HealthLevel.Dead)
                Health = HealthLevel.Critical;
            if (HPCurrent == 0 && Health != HealthLevel.Dead)
                Health = HealthLevel.Critical;
            if (HPCurrent < 0)
                Health = HealthLevel.Dead;
        }
        public HealStatus MedicalHeal(int mp)
        {
            if (HPCurrent < 0 || Health == HealthLevel.Dead)
                return HealStatus.NoHealNeeded;
            if (HPCurrent >= HPMax && Health == HealthLevel.Uninjured)
                return HealStatus.NoHealNeeded;
            bool success = true;
            double pct = mp * 0.01;
            if (r.NextDouble() < pct && (pct < .9 ? true : r.NextDouble() < .9))
                HPCurrent++;
            else
                success = false;
            if (Health == HealthLevel.Uninjured)
                return success ? HealStatus.HealOnly : HealStatus.AllFail;
            pct = pct / ((int)Health + 1);
            if (pct > .9)
                pct = .9;
            if (r.NextDouble() < pct)
            {
                Health = Health.Heal();
                return success ? HealStatus.AllSuccess : HealStatus.LevelOnly;
            }
            else
                return success ? HealStatus.HealOnly : HealStatus.AllFail;
        }
        public Patient GetPatient()
        {
            return new Patient
            {
                Health = this.Health,
                SoldierName = this.SoldierName,
                HPCurrent = this.HPCurrent,
                _hpMax = this.HPMax,
                PatientID = this.ID
            };
        }
        public void CalcLimit()
        {
            PowerLimit = ((ComBase + MagBase + SubBase) / 4) + r.Next(3) + (EliteStats(ComBase, MagBase, SubBase) ? r.Next(3, 12) : r.Next(2));
        }
        private bool EliteStats(int c, int m, int s)
        {
            if (c + m + s > 20)
                return true;
            if (c >= 10 || m >= 10 || s >= 10)
                return true;
            return false;
        }
        public void AddSub(Soldier soldier)
        {
            SubSoldiers.Add(soldier);
            soldier.SubTo = this;
        }
        public void CreateHealer()
        {
            if (PowerLevel < 500 || Medical == null)
                return;
            Medical.Assessed = true; //for testing
            Medical.Trained = true; //for testing
            Medical.MedicalPowerBase = 1;
        }
        public void CreateLeader()
        {
            if (PowerLevel < 2500)
                return;
            LeadAssessed = true; //for testing mostly
            IsLeading = true;
            LeadTrainRemains = 0;
        }
        #region Displays
        public string GetSkillDisplay(MagicColor color)
        {
            double total = ResearchAffinity / 3;
            if (ResearchSkill.ContainsKey(color))
                total = ResearchSkill[color];
            if (total < .5)
                return "Unskilled";
            if (total < .8)
                return "Poor";
            if (total < 1.1)
                return "Good";
            if (total < 1.4)
                return "Excellent";
            return "Superior";
        }
        public string RankText
        {
            get
            {
                switch (Type)
                {
                    case SoldierType.Beast: return "Beast";
                    case SoldierType.GreaterDemon:
                    case SoldierType.LesserDemon: return "Demon";
                    case SoldierType.GreaterUndead:
                    case SoldierType.LesserUndead: return "Undead";
                    case SoldierType.LesserSpirit:
                    case SoldierType.GreaterSpirit: return "Spirit";
                    case SoldierType.Nephilim: return "Nephilim";
                    case SoldierType.Standard: return "To do soon";
                    default: return "Unknown";
                }
            }
        }
        public string FatigueIcon
        {
            get
            {
                if (Medical == null)
                    return MudBlazor.Icons.Material.TwoTone.Cancel;
                return Medical.FatigueIcon;
            }
        }
        public MudBlazor.Color FatigueColor
        {
            get
            {
                if (Medical == null)
                    return MudBlazor.Color.Default;
                return Medical.FatigueColor;
            }
        }
        public string ResearchDisplay
        {
            get
            {
                if (!LeadAssessed)
                    return "";
                double total = ResearchAffinity;
                if (total < .5)
                    return "Terrible";
                if (total < .8)
                    return "Poor";
                if (total < 1.1)
                    return "Good";
                if (total < 1.4)
                    return "Excellent";
                return "Superior";
            }
        }
        public string LeadTrainDisplay
        {
            get
            {
                if (IsLeading)
                    return "Trained";
                if (!LeadAssessed)
                    return "";
                if (LeadTrainRemains <= 0)
                    return "Achieved";
                if (LeadTrainRemains <= 3)
                    return "Small";
                if (LeadTrainRemains <= 6)
                    return "Moderate";
                if (LeadTrainRemains <= 9)
                    return "Great";
                return "Tremendous";
            }
        }
        public string LeadPotentialDisplay
        {
            get
            {
                if (IsLeading)
                    return "Leading";
                if (!LeadAssessed)
                    return "";
                int rawscore = CharismaBase + LogisticsBase + TacticsBase;
                int highscore = Math.Max(Math.Max(CharismaBase, LogisticsBase), TacticsBase);
                if (rawscore <= 5 && highscore <= 3)
                    return "None";
                if (rawscore <= 10 || highscore <= 5)
                    return "Small";
                if (rawscore <= 15 || highscore <= 7)
                    return "Moderate";
                if (rawscore <= 20 || highscore <= 9)
                    return "Great";
                if (rawscore <= 25)
                    return "Tremendous";
                return "Legendary";
            }
        }
        public string TeachDisplay
        {
            get
            {
                if (!LeadAssessed)
                    return "";
                if (TeachSkill < .4)
                    return "Terrible";
                if (TeachSkill < .8)
                    return "Mediocre";
                if (TeachSkill < 1.2)
                    return "Good";
                if (TeachSkill < 1.6)
                    return "Excellent";
                return "Legendary";
            }
        }
        public string TeachDisplayPotential
        {
            get
            {
                if (!LeadAssessed)
                    return "";
                double togain = MaxTeach - TeachSkill;
                if (togain < .05)
                    return "Achieved";
                if (togain < .3)
                    return "Integrating";
                if (togain < .6)
                    return "Progressing";
                if (togain < .9)
                    return "Developing";
                return "Untapped";
            }
        }
        public string DisplayCounterIntel
        {
            get
            {
                if (!LeadAssessed)
                    return "";
                if (CounterIntelSkill < .2)
                    return "Poor";
                if (CounterIntelSkill < .4)
                    return "Fair";
                if (CounterIntelSkill < .6)
                    return "Good";
                if (CounterIntelSkill < .85)
                    return "Excellent";
                return "Legendary";
            }
        }
        public string DisplayCounterPotential
        {
            get
            {
                if (!LeadAssessed)
                    return "";
                double togain = CounterIntelMax - CounterIntelSkill;
                if (togain < .05)
                    return "Achieved";
                if (togain < .2)
                    return "Progressing";
                if (togain < .6)
                    return "Developing";
                return "Untapped";
            }
        }
        #endregion
    }
}