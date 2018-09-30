using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace Pandemic.ViewModels
{
    public class GameMenuViewModel : ViewModelBase
    {
        private RelayCommand<Window> _backToGameCommand;
        private RelayCommand _exitCommand;
        private RelayCommand<Window> _maimMenuCommand;
        private RelayCommand<Window> _saveGameCommand;

        public RelayCommand<Window> BackToGameCommand
        {
            get => (_backToGameCommand) ?? (_backToGameCommand = new RelayCommand<Window>((w) => w.DialogResult = true));
        }

        public RelayCommand ExitCommand
        {
            get => (_exitCommand) ?? (_exitCommand = new RelayCommand(() => Application.Current.Shutdown()));
        }

        /// <summary>
        /// Sets and gets the MaimMenuCommand property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public RelayCommand<Window> MaimMenuCommand
        {
            get => (_maimMenuCommand) ?? (_maimMenuCommand = new RelayCommand<Window>(MainMenu));
        }

        public RelayCommand<Window> SaveGameCommand
        {
            get => _saveGameCommand ?? (_saveGameCommand = new RelayCommand<Window>((w) => w.DialogResult = true));
        }

        private void MainMenu(Window view)
        {
            MessengerInstance.Send(new NavigateToViewModelMessage(MessageTokens.MainMenu));
            view.DialogResult = true;
        }
    }
}