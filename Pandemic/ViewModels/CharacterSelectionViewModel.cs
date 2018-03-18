using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class CharacterSelectionViewModel : ViewModelBase
    {
        public ICommand CharacterSelectedCommand { get; }

        public IEnumerable<Character> Characters { get; }

        public CharacterSelectionViewModel(IEnumerable<Character> characters)
        {
            Characters = characters ?? throw new ArgumentNullException(nameof(characters));

            CharacterSelectedCommand = new RelayCommand<Character>(character => OnCharacterSelected(character));
        }

        protected void OnCharacterSelected(Character character)
        {
            MessengerInstance.Send(new GenericMessage<Character>(character));
        }
    }
}
