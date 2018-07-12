using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                MessengerInstance.Send(new NavigateToViewModelMessage(MessageTokens.StartNewGame))));

        }

        private ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get => _exitCommand ?? (_exitCommand = new RelayCommand(() => Application.Current.Shutdown()));

        }
    }
}
