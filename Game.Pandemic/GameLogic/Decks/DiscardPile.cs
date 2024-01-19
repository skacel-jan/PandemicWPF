using System.Collections.Generic;
using System.Linq;
using Game.Pandemic.GameLogic.Cards;

namespace Game.Pandemic.GameLogic.Decks
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
            OnPropertyChanged(nameof(TopCard));
        }

        public override void AddCards(IEnumerable<T> cards, DeckSide side = DeckSide.Top)
        {
            base.AddCards(cards, side);
            OnPropertyChanged(nameof(TopCard));
        }

        public void RemoveCard(T card)
        {
            InnerCards.Remove(card);
            OnPropertyChanged(nameof(Cards));
            OnPropertyChanged(nameof(TopCard));
        }

        public void Clear()
        {
            InnerCards.Clear();
            OnPropertyChanged(nameof(Cards));
            OnPropertyChanged(nameof(TopCard));
        }
    }
}