using Pandemic.Characters;
using System.Collections.Generic;

namespace Pandemic.GameLogic
{
    public class GameSettings
    {
        public IEnumerable<string> SelectedCharacters { get; internal set; } = new[] { "Medic" };
        public int Difficulty { get; internal set; } = 4;
        public CharacterFactory CharacterFactory { get; }

        public GameSettings(CharacterFactory characterFactory)
        {
            CharacterFactory = characterFactory;
        }

        public CircularCollection<Character> GetCharacters(MapCity startingCity)
        {
            var characters = CharacterFactory.GetCharacters(SelectedCharacters, startingCity);

            return new CircularCollection<Character>(characters);
        }
    }
}