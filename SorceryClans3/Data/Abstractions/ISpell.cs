using System.ComponentModel.DataAnnotations;
using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Abstractions
{
    public interface ISpell
    {
        [Key] public Guid ID { get; set; }
        public string SpellName { get; }
        public int MoneyToCast { get; set; }
        public int PowerToCast { get; set; }
        public int ColorToCast { get; set; }
        public MagicColor Color { get; set; }
        public string SpellIcon { get; }
        public MudBlazor.Color IconColor { get; }
    }
}