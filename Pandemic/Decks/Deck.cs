using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pandemic.Decks
{
    public class Deck<T> : IDeck<T>, IShuffle<T>, IDraw<T> where T : Card
    {
        public Deck()
        {
            Cards = new ObservableCollection<T>();
        }

        public Deck(IEnumerable<T> cards)
        {
            Cards = new ObservableCollection<T>(cards);
        }

        public ObservableCollection<T> Cards { get; }

        public T Draw()
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

        public void AddCard(T card)
        {
            Cards.Add(card);
        }

        public void AddCards(IEnumerable<T> cards)
        {
            foreach (var card in cards)
            {
                AddCard(card);
            }
        }
    }
}