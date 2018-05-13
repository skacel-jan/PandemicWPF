using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic
{
    public class CharacterSelectionService
    {
        public IEnumerable<Character> Characters { get; internal set; }

        public CharacterSelectionService(IEnumerable<Character> characters)
        {
            Characters = characters ?? throw new ArgumentNullException(nameof(characters));
        }

        public event EventHandler<CharacterSelectingEventArgs> CharacterSelecting;

        public void SelectCharacter(string text, IEnumerable<Character> characters, Action<Character> action)
        {
            OnCharacterSelecting(new CharacterSelectingEventArgs(characters, text, action));
        }

        protected virtual void OnCharacterSelecting(CharacterSelectingEventArgs e)
        {
            CharacterSelecting?.Invoke(this, e);
        }
    }
}
