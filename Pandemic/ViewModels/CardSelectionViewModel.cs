using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class CardSelectionViewModel : ViewModelBase
    {
        public CardSelectionViewModel(IEnumerable<Card> cards, string token)
        {
            Cards = cards;
            CardSelectedCommand = new RelayCommand<Card>(card => OnCardSelected(card));

            Token = token;
        }

        public IEnumerable<Card> Cards { get; }
        public ICommand CardSelectedCommand { get; }
        public string Token { get; }

        protected void OnCardSelected(Card card)
        {
            MessengerInstance.Send(new GenericMessage<Card>(card), Token);
        }
    }
}