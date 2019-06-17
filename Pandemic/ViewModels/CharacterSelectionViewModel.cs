using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class CharacterSelectionViewModel : ViewModelBase
    {
        private readonly Action<Character> _selectCharacterCallback;

        public ICommand CharacterSelectedCommand { get; }

        public IEnumerable<Character> Characters { get; }

        public CharacterSelectionViewModel(Action<Character> selectCharacterCallback, IEnumerable<Character> characters)
        {
            _selectCharacterCallback = selectCharacterCallback;
            Characters = characters;
            CharacterSelectedCommand = new RelayCommand<Character>(_selectCharacterCallback);
        }
    }
}