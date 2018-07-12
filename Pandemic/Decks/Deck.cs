using GalaSoft.MvvmLight;
using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pandemic.Decks
{
    public class BaseDeck<T> : ObservableObject, IDeck<T> where T : Card
    {
        public BaseDeck()
        {
            Cards = new ObservableCollection<T>();
        }

        public BaseDeck(IEnumerable<T> cards)
        {
            Cards = new ObservableCollection<T>(cards);
        }

        public ObservableCollection<T> Cards { get; }

        public virtual void AddCard(T card)
        {
            if (card != null)
            {
                Cards.Add(card);
            }
        }

        public virtual void AddCards(IEnumerable<T> cards)
        {
            if (cards == null)
            {
                throw new ArgumentNullException(nameof(cards));
            }

            foreach (var card in cards)
            {
                AddCard(card);
            }
        }

        public virtual T DrawTop()
        {
            if (Cards.Count > 0)
            {
                var card = Cards[0];
                Cards.RemoveAt(0);
                return card;
            }
            else
            {
                return null;
            }
        }

        public virtual T DrawBottom()
        {
            if (Cards.Count > 0)
            {
                var card = Cards[Cards.Count - 1];
                Cards.RemoveAt(Cards.Count - 1);
                return card;
            }
            else
            {
                return null;
            }
        }

        public virtual void RemoveCard(T card)
        {
            Cards.Remove(card);
        }
    }

    public class Deck<T> : BaseDeck<T>, IShuffle<T> where T : Card
    {
        public Deck()
        {
        }

        public Deck(IEnumerable<T> cards) : base(cards)
        {
        }

        public void Shuffle()
        {
            int n = Cards.Count();
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = Cards[k];
                Cards[k] = Cards[n];
                Cards[n] = value;
            }
        }
    }

    public class DiscardPile<T> : BaseDeck<T> where T: Card
    {
        public DiscardPile()
        {
        }

        public DiscardPile(IEnumerable<T> cards) : base(cards)
        {
        }

        public T TopCard { get => Cards.LastOrDefault(); }

        public override void AddCard(T card)
        {
            base.AddCard(card);
            RaisePropertyChanged(nameof(TopCard));
        }

        public override void RemoveCard(T card)
        {
            base.RemoveCard(card);
            RaisePropertyChanged(nameof(TopCard));
        }
    }
}