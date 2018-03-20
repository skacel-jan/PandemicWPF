using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class ShareTypeSelectionViewModel : ViewModelBase
    {
        public ShareTypeSelectionViewModel(Action<ShareType> action)
        {
            ShareTypeSelectedCommand = new RelayCommand<ShareType>(action);
        }

        public ICommand ShareTypeSelectedCommand { get; }
    }
}