using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Game.Pandemic.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private ICommand _startGameCommand;

        public ICommand StartGameCommand
        {
            get => _startGameCommand ?? (_startGameCommand = new RelayCommand(() =>
                Messenger.Send(new NavigateToViewModelMessage(MessageTokens.NewGameSettings), MessageTokens.GameChannel)));
        }

        private ICommand _exitCommand;

        public ICommand ExitCommand
        {
            get => _exitCommand ?? (_exitCommand = new RelayCommand(() => Application.Current.Shutdown()));
        }

        private ICommand _loadGameCommand;

        public ICommand LoadGameCommand
        {
            get => _loadGameCommand ?? (_loadGameCommand = new RelayCommand(() =>
                Messenger.Send(new NavigateToViewModelMessage(MessageTokens.LoadGame), MessageTokens.GameChannel)));
        }
    }
}