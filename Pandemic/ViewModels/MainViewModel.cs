using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
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
            MessengerInstance.Register<NavigateToViewModelMessage>(this, NavigateTo);
            CurrentViewModel = new MainMenuViewModel();
        }

        private void NavigateTo(NavigateToViewModelMessage message)
        {
            switch (message.NavigateTo)
            {
                case Messenger.StartNewGame:
                    SetGameView();
                    break;
            }
        }

        public void SetGameView()
        {
            CurrentViewModel = new GameViewModel();
        }
    }
}
