using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class ShareTypeSelectionViewModel : ViewModelBase
    {
        public ShareTypeSelectionViewModel()
        {
            ShareTypeSelectedCommand = new RelayCommand<string>(type => OnShareTypeSelected(type));
        }

        public ICommand ShareTypeSelectedCommand { get; }

        protected void OnShareTypeSelected(string type)
        {
            MessengerInstance.Send(new GenericMessage<string>(type), MessageTokens.ShareTypeSelected);
        }
    }
}