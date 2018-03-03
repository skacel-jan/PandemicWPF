using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pandemic.Decks
{
    public class Deck<T> where T : Card
    {
        public ObservableCollection<T> Cards { get; }

        public Deck()
        {
            Cards = new ObservableCollection<T>();
        }

        public Deck(IEnumerable<T> cards)
        {
            Cards = new ObservableCollection<T>(cards);
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
    }
}
