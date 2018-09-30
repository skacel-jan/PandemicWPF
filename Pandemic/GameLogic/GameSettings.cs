using Pandemic.Characters;
using System.Collections.Generic;

namespace Pandemic.GameLogic
{
    public class GameSettings
    {
        public IEnumerable<string> StartingCharacters { get; internal set; } = new[] { "Medic" };
        public int Difficulty { get; internal set; } = 4;

        public CircularCollection<Character> GetCharacters(MapCity startingCity)
        {
            var characters = new CharacterFactory(new CharacterActionsFactory())
                .GetCharacters(StartingCharacters, startingCity);

            return new CircularCollection<Character>(characters);
        }
    }
}