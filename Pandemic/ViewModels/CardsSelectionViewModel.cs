using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class CardsSelectionViewModel : ViewModelBase
    {
        public CardsSelectionViewModel(IEnumerable<Card> cards, Action<Card> cardSelectedDelegate)
        {
            Cards = cards;
            CardSelectedDelegate = cardSelectedDelegate;
            CardSelectedCommand = new RelayCommand<Card>(card => OnCardSelected(card));
        }

        public IEnumerable<Card> Cards { get; private set; }
        public ICommand CardSelectedCommand { get; private set; }
        public Action<Card> CardSelectedDelegate { get; }

        protected void OnCardSelected(Card card)
        {
            CardSelectedDelegate?.Invoke(card);
        }
    }
}