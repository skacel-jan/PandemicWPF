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
        public CardsViewModel(string code, IEnumerable<Card> items)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public string Code { get; }

        public IEnumerable<Card> Items { get; private set; }
    }
}