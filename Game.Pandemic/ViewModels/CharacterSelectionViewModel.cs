using System;
using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.ViewModels
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