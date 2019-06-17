using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.GameLogic;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Pandemic.ViewModels
{
    public class GameMenuViewModel : ViewModelBase
    {
        private RelayCommand<Window> _backToGameCommand;
        private RelayCommand _exitCommand;
        private RelayCommand<Window> _maimMenuCommand;
        private RelayCommand<Window> _saveGameCommand;
        private RelayCommand<Window> _loadGameCommand;
        private readonly Game _game;

        public GameMenuViewModel(Game game)
        {
            this._game = game;
        }

        public RelayCommand<Window> BackToGameCommand
        {
            get => (_backToGameCommand) ?? (_backToGameCommand = new RelayCommand<Window>((w) => w.DialogResult = true));
        }

        public RelayCommand ExitCommand
        {
            get => (_exitCommand) ?? (_exitCommand = new RelayCommand(() => Application.Current.Shutdown()));
        }

        /// <summary>
        /// Sets and gets the MainMenuCommand property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public RelayCommand<Window> MainMenuCommand
        {
            get => (_maimMenuCommand) ?? (_maimMenuCommand = new RelayCommand<Window>(MainMenu));
        }

        public RelayCommand<Window> SaveGameCommand
        {
            get => _saveGameCommand ?? (_saveGameCommand = new RelayCommand<Window>(async (w) => await SaveGame(w)));
        }

        public RelayCommand<Window> LoadGameCommand
        {
            get => _loadGameCommand ?? (_loadGameCommand = new RelayCommand<Window>(async (w) => await LoadGame(w)));
        }

        private async Task LoadGame(Window window)
        {
            await _game.Load();

            window.DialogResult = true;
        }

        private async Task SaveGame(Window window)
        {
            await _game.Save();            
            window.DialogResult = true;
        }

        private void MainMenu(Window view)
        {
            MessengerInstance.Send(new NavigateToViewModelMessage(MessageTokens.MainMenu));
            view.DialogResult = true;
        }
    }
}