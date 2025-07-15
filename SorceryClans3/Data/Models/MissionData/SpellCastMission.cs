using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Models
{
    public class SpellCastMission
    {
        [Key] public Guid ID { get; set; } = Guid.NewGuid();
        public int PowerPointsRemaining { get; set; }
        public int ColorPointsRemaining { get; set; }
        public MagicColor Color { get; set; }
        public Spell CastingSpell { get; set; }
        public Team CastingTeam { get; set; }
        public Soldier? CastingSoldier { get; set; }
        public Soldier? TargetSoldier { get; set; }
        public int? NumConsumables { get; set; }
        public List<Soldier> AddedSoldiers { get; set; } = [];
        public SpellCastMission(Spell spell, Team team, Soldier? casting = null, Soldier? target = null, int? quantity = null)
        {
            CastingSpell = spell;
            NumConsumables = quantity;
            PowerPointsRemaining = spell.PowerToCast * (quantity ?? 1);
            ColorPointsRemaining = spell.ColorToCast * (quantity ?? 1);
            Color = spell.Color;
            CastingTeam = team;
            team.MissionID = ID;
            CastingSoldier = casting;
            TargetSoldier = target;
            if (TargetSoldier != null)
            {
                if (TargetSoldier.Team == null)
                    TargetSoldier.Team = new() { TeamName = "Receiving Spell", ID = this.ID };
                else
                    TargetSoldier.Team.MissionID = this.ID;
            }
        }
        public SpellCastMission? IncrementDay()
        {
            PowerPointsRemaining -= CastingTeam.MScore / 30;
            ColorPointsRemaining -= CastingTeam.GetColors.Count(e => e == Color);
            if (PowerPointsRemaining <= 0 && ColorPointsRemaining <= 0)
                return Result();
            return null;
        }
        public SpellCastMission Result()
        {
            // do the thing here if needs doing (except creating soldier, that's in the result constructor)
            if (CastingSpell.Power != null && TargetSoldier != null)
            {
                TargetSoldier.Power = CastingSpell.Power.GeneratePower(true);
            }
            else if (CastingSpell.GreaterUndead != null && TargetSoldier != null)
            {
                CastingSpell.GreaterUndead.Apply(TargetSoldier);
            }
            else if (CastingSpell.GreaterDemon != null && TargetSoldier != null)
            {
                CastingSpell.GreaterDemon.Apply(TargetSoldier);
            }
            if (TargetSoldier?.Team != null)
            {
                if (TargetSoldier.Team.ID == this.ID)
                    TargetSoldier.Team = null;
                else
                    TargetSoldier.Team.MissionID = null;
            }
            int castcount = NumConsumables ?? 1;
            if (CastingSpell.UsesConsumables)
            {
                CastingSpell.Consumables += castcount;
                if (CastingSpell.ProcessesConsumables)
                    CastingSpell.UnprocessedConsumables -= castcount;
            }
            if (CastingSpell.GeneratesSoldier)
            {
                for (int i = 0; i < castcount; i++)
                {
                    Soldier? s = CastingSpell.GenerateSoldier();
                    if (s != null)
                        AddedSoldiers.Add(s);
                }
            }
            return this;
        }
        public string CreateMessage()
        {
            if (NumConsumables != null)
            {
                if (CastingSpell.LesserDemon != null)
                {
                    return $"Spell completed: summoned {CastingSpell.LesserDemon.DisplayWithCount(NumConsumables.Value, true)}";
                }
                else if (CastingSpell.AngelIcon != null)
                {
                    return $"Spell completed: built {NumConsumables} Icon{(NumConsumables == 1 ? "" : "s")} of {CastingSpell.AngelIcon.Angel.Name}";
                }
                return $"Spell completed: {NumConsumables.Value} {CastingSpell.ConsumablePrint(NumConsumables.Value)} prepared.";
            }
            if (TargetSoldier != null)
            {
                return $"Spell completed: {CastingSpell.SpellName} cast on {TargetSoldier.SoldierName}";
            }
            return $"Spell completed: {CastingSpell.SpellName}";
        }
    }
}