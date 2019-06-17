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

        public IEnumerable<Character> GetCharacters(MapCity startingCity, Game game)
        {
            return CharacterFactory.GetCharacters(SelectedCharacters, startingCity, game);
        }
    }
}