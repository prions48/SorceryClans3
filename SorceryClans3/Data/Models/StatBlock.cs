namespace SorceryClans3.Data.Models
{
    public class StatBlock
    {
        public int? Com { get; set; }
        public int? Mag { get; set; }
        public int? Cha { get; set; }
        public int? Log { get; set; }
        public int? Tac { get; set; }
        public int? Sub { get; set; }
        public int? HP { get; set; }
        public int? Heal { get; set; }
        public int? Travel { get; set; }
        public int? Power { get; set; }
        public StatBlock(int? c, int? m, int? s, int? h, int? k, int? p, int? d, int? z, int? l, int? t)
        {
            Com = c;
            Mag = m;
            Sub = s;
            HP = h;
            Heal = k;
            Travel = d;
            Cha = z;
            Log = l;
            Tac = t;
            Power = p;
        }
        
        public bool IsAbove(Soldier s)
        {
            return (Com == null || s.ComNat >= Com) && (Mag == null || s.MagNat >= Mag)
            && (Sub == null || s.SubNat >= Sub) && (Heal == null || s.HealNat >= Heal)
            && (Power == null || s.PowerLevel >= Power) && (HP == null || s.HPNat >= HP)
            && (Travel == null || s.TravNat >= Travel) && (Cha == null || s.Charisma >= Cha)
            && (Log == null || s.Charisma >= Log) && (Tac == null ||  s.Tactics >= Tac);
        }
        public bool IsBelow(Soldier s)
        {
            return (Com == null || s.ComNat <= Com) && (Mag == null || s.MagNat <= Mag)
            && (Sub == null || s.SubNat <= Sub) && (Heal == null || s.HealNat <= Heal)
            && (Power == null || s.PowerLevel <= Power) && (HP == null || s.HPNat <= HP)
            && (Travel == null || s.TravNat <= Travel) && (Cha == null || s.Charisma <= Cha)
            && (Log == null || s.Charisma <= Log) && (Tac == null || s.Tactics <= Tac);
        }
    }
}