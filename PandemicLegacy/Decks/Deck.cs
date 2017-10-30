﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PandemicLegacy.Decks
{
    public class Deck<T> : List<T> where T : Card
    {
        public Deck(IEnumerable<T> cards)
        {
            this.AddRange(cards);
        }

        public void Shuffle()
        {
            int n = this.Count();
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = this[k];
                this[k] = this[n];
                this[n] = value;
            }
        }

        public T Draw()
        {
            if (this.Count > 0)
            {
                var card = this[0];
                this.RemoveAt(0);
                return card;
            }
            else
            {
                return null;
            }
        }

        public static IList<T> Shuffle(IList<T> cards)
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }

            return cards;
        }
    }
}
