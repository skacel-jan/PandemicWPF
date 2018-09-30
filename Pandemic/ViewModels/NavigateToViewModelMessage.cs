using GalaSoft.MvvmLight.Messaging;

namespace Pandemic.ViewModels
{
    internal class NavigateToViewModelMessage : MessageBase
    {
        public string NavigateTo { get; }

        internal NavigateToViewModelMessage(string navigateTo)
        {
            NavigateTo = navigateTo;
        }
    }
}