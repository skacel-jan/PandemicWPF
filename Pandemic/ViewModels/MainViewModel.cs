using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
    {
            get { return _currentViewModel; }
            set { Set( ref _currentViewModel, value); }
        }


        public MainViewModel()
        {
            SetMainMenuView();
        }

        public void SetGameView()
        {
            CurrentViewModel = new GameViewModel();
        }

        public void SetMainMenuView()
        {
            CurrentViewModel = new MainMenuViewModel(this);            
        }
    }
}
