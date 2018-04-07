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
        public Action<Character> CharacterSelectedDelegate { get; }

        public CharacterSelectionViewModel(IEnumerable<Character> characters, Action<Character> characterSelectedDelegate)
        {
            Characters = characters ?? throw new ArgumentNullException(nameof(characters));
            CharacterSelectedDelegate = characterSelectedDelegate;
            CharacterSelectedCommand = new RelayCommand<Character>(CharacterSelectedDelegate);
        }
    }
}
