using System;
using System.Collections.Generic;
using Game.Pandemic.GameLogic.Characters;
using Game.Pandemic.GameLogic.Services;

namespace Game.Pandemic.GameLogic.Actions.Selections
{
    public class CharacterSelection : Selection
    {
        private readonly Action<Character> _selectCharacterCallback;
        private readonly IEnumerable<Character> _characters;

        public CharacterSelection(Action<Character> setCharacter, IEnumerable<Character> characters, string infoText)
        {
            _selectCharacterCallback = setCharacter;
            _characters = characters;
            InfoText = infoText;
        }

        public override void Execute(SelectionService service)
        {
            service.SelectCharacter(_selectCharacterCallback, _characters, InfoText);
        }
    }
}