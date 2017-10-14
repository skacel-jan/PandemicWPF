using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PandemicLegacy.ViewModels
{
    public class CardsSelectionViewModel : ViewModelBase
    {
        public event EventHandler<Card> CardSelected;

        public ICommand CardSelectedCommand { get; private set; }

        public IEnumerable<Card> Cards { get; private set; }

        public CardsSelectionViewModel(IEnumerable<Card> cards, MapCity city)
        {
            Cards = cards;
            CardSelectedCommand = new RelayCommand<Card>(card => OnCardSelected(card));
        }

        protected void OnCardSelected(Card card)
        {
            CardSelected?.Invoke(this, card);
        }
    }
}
