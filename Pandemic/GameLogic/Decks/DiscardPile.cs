using Pandemic.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.Decks
{
    public class DiscardPile<T> : Deck<T> where T : Card
    {
        public DiscardPile(IEnumerable<T> cards)
        {
            InnerCards = new List<T>(cards);
        }

        public DiscardPile() : this(Enumerable.Empty<T>())
        {
        }

        public T TopCard { get => Cards.LastOrDefault(); }


        public override void AddCard(T card, DeckSide side = DeckSide.Top)
        {
            base.AddCard(card, side);
            RaisePropertyChanged(nameof(TopCard));
        }

        public override void AddCards(IEnumerable<T> cards, DeckSide side = DeckSide.Top)
        {
            base.AddCards(cards, side);
            RaisePropertyChanged(nameof(TopCard));
        }

        public void RemoveCard(T card)
        {
            InnerCards.Remove(card);
            RaisePropertyChanged(nameof(Cards));
            RaisePropertyChanged(nameof(TopCard));
        }

        public void Clear()
        {
            InnerCards.Clear();
            RaisePropertyChanged(nameof(Cards));
            RaisePropertyChanged(nameof(TopCard));
        }
    }
}