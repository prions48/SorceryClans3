using SorceryClans3.Data.Models;
namespace SorceryClans3.Data.Tools
{
    public static partial class Names
    {
        public static string Domesticated(PetType type, int lvl)
        {
            switch (type)
            {
                case PetType.Mount: return "Horse";//all this 
                case PetType.Spy: return PetSpy(lvl);
                case PetType.Scout: return PetScout(lvl);
                case PetType.Sentinel: return PetSentinel(lvl);
                case PetType.Brute: return PetBrute(lvl);
                case PetType.Breaker: return PetBreaker(lvl);
                case PetType.Magic: return PetSentinel(lvl);
                case PetType.Hunter: return PetSentinel(lvl);
                //to do more here
                default: return "Jackalope";
            }
        }
        private static string PetSpy(int lvl)
        {
            switch (r.Next(lvl / 4 + 3))
            {
                case 0: return "Rat";
                case 1: return "Ferret";
                case 2: return "Rabbit";
                case 3: return "Pigeon";
                case 4: return "Duck";
                case 5: return "Goose";
                default: return "Parrot";
            }
        }
        private static string PetScout(int lvl)
        {
            switch (r.Next(lvl / 4 + 3))
            {
                case 0: return "Wildcat";
                case 1: return "Coyote";
                case 2: return "Falcon";
                case 3: return "Fox";
                case 4: return "Badger";
                case 5: return "Wolverine";
                case 6: return "Hawk";
                default: return "Eagle";
            }
        }
        private static string PetSentinel(int lvl)
        {
            switch (r.Next(lvl / 5 + 3))
            {
                case 0: return "Raccoon";
                case 1: return "Crow";
                case 2: return "Bat";
                default: return "Owl";
            }
        }
        private static string PetBrute(int lvl)
        {
            switch (r.Next(lvl / 5 + 3))
            {
                case 0: return "Bear";
                case 1: return "Rhinoceras";
                case 2: return "Bat";
                default: return "Owl";
            }
        }
        private static string PetBreaker(int lvl)
        {
            switch (r.Next(lvl / 5 + 3))
            {
                case 0: return "Raccoon";
                case 1: return "Crow";
                case 2: return "Bat";
                default: return "Owl";
            }
        }
    }
}