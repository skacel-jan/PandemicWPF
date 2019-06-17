using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
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