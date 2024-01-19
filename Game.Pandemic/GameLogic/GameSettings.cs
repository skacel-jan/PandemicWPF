using System.Collections.Generic;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic
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