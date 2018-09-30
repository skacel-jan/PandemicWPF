using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.GameLogic.Actions;
using System.Collections.Generic;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class CharacterSelectionViewModel : ViewModelBase
    {
        private ISelectAction<Character> selectAction;

        public ICommand CharacterSelectedCommand { get; }

        public IEnumerable<Character> Characters { get; }

        public CharacterSelectionViewModel(ISelectAction<Character> selectAction)
        {
            this.selectAction = selectAction;
            Characters = selectAction.Items;
            CharacterSelectedCommand = new RelayCommand<Character>((character) => this.selectAction.Execute(character));
        }
    }
}