using GalaSoft.MvvmLight;
using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.Decks
{
    public class Deck<T> : ObservableObject, IDeck<T> where T : Card
    {
        public Deck() : this(Enumerable.Empty<T>()) { }

        public Deck(IEnumerable<T> cards)
        {
            InnerCards = new List<T>(cards);
        }

        protected List<T> InnerCards { get; }
        public IEnumerable<T> Cards => InnerCards;

        public virtual void AddCard(T card, DeckSide side = DeckSide.Top)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }
            InnerCards.Insert(side == DeckSide.Top ? 0 : InnerCards.Count - 1, card);
            RaisePropertyChanged(nameof(Cards));
        }

        public virtual void AddCards(IEnumerable<T> cards, DeckSide side = DeckSide.Top)
        {
            if (cards == null)
            {
                throw new ArgumentNullException(nameof(cards));
            }

            InnerCards.InsertRange(side == DeckSide.Top ? 0 : InnerCards.Count - 1, cards);

            RaisePropertyChanged(nameof(Cards));
        }

        public virtual T Draw(DeckSide side)
        {
            if (InnerCards.Count > 0)
            {
                var card = InnerCards[0];
                InnerCards.RemoveAt(0);
                RaisePropertyChanged(nameof(Cards));
                return card;
            }
            else
            {
                return null;
            }
        }

        public void Shuffle()
        {
            int n = InnerCards.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = InnerCards[k];
                InnerCards[k] = InnerCards[n];
                InnerCards[n] = value;
            }

            RaisePropertyChanged(nameof(Cards));
        }
    }
}