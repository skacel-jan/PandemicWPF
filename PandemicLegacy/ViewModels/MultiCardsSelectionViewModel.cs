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
    public class MultiCardsSelectionViewModel : ViewModelBase
    {
        public ICommand CardSelectedCommand { get; private set; }

        public IEnumerable<CityCard> Cards { get; private set; }

        public IList<CityCard> SelectedCards { get; private set; }

        public int SelectCardCount { get; private set; }

        public DiseaseColor? DiseaseColor { get; private set; }

        public MultiCardsSelectionViewModel(IEnumerable<CityCard> cards, int selectCardCount)
        {
            Cards = cards;
            SelectCardCount = selectCardCount;
            SelectedCards = new List<CityCard>(selectCardCount);
            CardSelectedCommand = new RelayCommand<CityCard>(card => OnCardSelected(card));
        }

        public MultiCardsSelectionViewModel(IEnumerable<CityCard> cards, int selectCardCount, DiseaseColor color)
            : this(cards.Where(x => x.City.Color == color), selectCardCount)
        {
            DiseaseColor = color;
        }

        protected void OnCardSelected(CityCard card)
        {
            if (DiseaseColor.HasValue)
            {
                if (DiseaseColor == card.City.Color && !SelectedCards.Remove(card))
                    SelectedCards.Add(card);
            }
            else if (!SelectedCards.Remove(card))
            {
                SelectedCards.Add(card);
            }                                

            if (SelectCardCount == SelectedCards.Count)
            {
                MessengerInstance.Send(SelectedCards, "CardsSelection");
            }
        }
    }
}
