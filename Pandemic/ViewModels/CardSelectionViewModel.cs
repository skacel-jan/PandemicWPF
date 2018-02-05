using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class CardSelectionViewModel : ViewModelBase
    {
        public ICommand CardSelectedCommand { get; private set; }
        public IEnumerable<Card> Cards { get; private set; }

        public CardSelectionViewModel(IEnumerable<Card> cards)
        {
            Cards = cards;
            CardSelectedCommand = new RelayCommand<Card>(card => OnCardSelected(card));
        }

        protected void OnCardSelected(Card card)
        {
            MessengerInstance.Send(card, Messenger.CardSelected);
        }
    }
}
