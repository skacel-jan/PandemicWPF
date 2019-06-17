using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.Characters;
using Pandemic.GameLogic;
using Pandemic.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pandemic.ViewModels
{
    public class GameSetingsViewModel : ViewModelBase
    {
        private RelayCommand _startGameCommand;

        public GameSetingsViewModel(GameSettings gameSettings)
        {
            Difficulties = new int[] { 4, 5, 6 };
            var characters = new string[] { Medic.MEDIC, OperationsExpert.OPERATIONS_EXPERT, Researcher.RESEARCHER, ContingencyPlanner.CONTINGENCY_PLANNER,
                Scientist.SCIENTIST, QuarantineSpecialist.QUARANTINE_SPECIALIST, Dispatcher.DISPATCHER};

            Characters = new ObservableCollection<SelectedItemWrapper<string>>(characters.Select(c => new SelectedItemWrapper<string>(c)));
            foreach (var character in Characters)
            {
                character.PropertyChanged += Character_PropertyChanged;
            }

            GameSettings = gameSettings ?? throw new System.ArgumentNullException(nameof(gameSettings));
        }

        private void Character_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StartGameCommand.RaiseCanExecuteChanged();
        }

        public IEnumerable<int> Difficulties { get; }

        public GameSettings GameSettings { get; }

        public int SelectedDifficulty { get; set; } = 4;

        public ObservableCollection<SelectedItemWrapper<string>> Characters { get; }

        public RelayCommand StartGameCommand
        {
            get => _startGameCommand ?? (_startGameCommand = new RelayCommand(PrepareGame, () => Characters.Count(c => c.IsSelected) >= 2));
        }

        public void PrepareGame()
        {
            GameSettings.SelectedCharacters = Characters.Where(c => c.IsSelected).Select(c => c.Item);
            GameSettings.Difficulty = SelectedDifficulty;

            MessengerInstance.Send(new NavigateToViewModelMessage(MessageTokens.StartNewGame));
        }
    }
}