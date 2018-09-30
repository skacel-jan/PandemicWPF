using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private ICommand _startGameCommand;

        public ICommand StartGameCommand
        {
            get => _startGameCommand ?? (_startGameCommand = new RelayCommand(() =>
                MessengerInstance.Send(new NavigateToViewModelMessage(MessageTokens.NewGameSettings))));
        }

        private ICommand _exitCommand;

        public ICommand ExitCommand
        {
            get => _exitCommand ?? (_exitCommand = new RelayCommand(() => Application.Current.Shutdown()));
        }
    }
}