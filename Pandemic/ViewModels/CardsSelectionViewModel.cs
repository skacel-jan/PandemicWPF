using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class CardsSelectionViewModel : SelectionViewModel<Card>
    {
        public CardsSelectionViewModel(IEnumerable<Card> items, Action<Card> callbackAction) : base(items, callbackAction)
        {
        }
    }

    public class CardsViewModel : ViewModelBase
    {
        public CardsViewModel(IEnumerable<Card> cards)
        {
            Items = cards;
        }

        public IEnumerable<Card> Items { get; private set; }
    }
}