using System.ComponentModel.DataAnnotations;

namespace SorceryClans3.Data.Models
{
    public class SpellMission
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
        public SpellMission(Spell spell, Team team, Soldier? casting = null, Soldier? target = null, int? quantity = null)
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
        }
        public Spell? AdvanceDay()
        {
            PowerPointsRemaining -= CastingTeam.MScore/30;
            ColorPointsRemaining -= CastingTeam.GetColors.Count(e => e == Color);
            if (PowerPointsRemaining <= 0 && ColorPointsRemaining <= 0)
                return CastingSpell;
            return null;
        }
    }
}