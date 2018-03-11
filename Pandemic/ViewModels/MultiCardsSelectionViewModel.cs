using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class MultiCardsSelectionViewModel : ViewModelBase
    {
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

        public IEnumerable<CityCard> Cards { get; private set; }
        public ICommand CardSelectedCommand { get; private set; }
        public DiseaseColor? DiseaseColor { get; private set; }
        public int SelectCardCount { get; private set; }
        public IList<CityCard> SelectedCards { get; private set; }

        protected void OnCardSelected(CityCard card)
        {
            if (DiseaseColor.HasValue)
            {
                if (DiseaseColor == card.City.Color && !SelectedCards.Remove(card))
                {
                    SelectedCards.Add(card);
                }
            }
            else if (!SelectedCards.Remove(card))
            {
                SelectedCards.Add(card);
            }

            if (SelectCardCount == SelectedCards.Count)
            {
                MessengerInstance.Send(new GenericMessage<IList<CityCard>>(SelectedCards));
            }
        }
    }
}