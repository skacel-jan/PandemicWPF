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
        public MainViewModel MainViewModel { get; set; }

        private ICommand _startGameCommand;
        public ICommand StartGameCommand
        {
            get
            {
                return _startGameCommand ?? (_startGameCommand = new RelayCommand(() => MainViewModel.SetGameView()));
            }
        }

        private ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                return _exitCommand ?? (_exitCommand = new RelayCommand(() => Application.Current.Shutdown()));
            }
        }

        public MainMenuViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }
    }
}
