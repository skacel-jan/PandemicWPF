using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.Decks
{
    public class PlayerDeck : Deck<PlayerCard>
    {
        public PlayerDeck(IEnumerable<PlayerCard> cards) : base(cards)
        {
        }

        public void AddEpidemicCards(int epidemicCount)
        {
            var numberOfDecks = InnerCards.Count / epidemicCount;
            var deckIncrement = InnerCards.Count % epidemicCount;

            var newDeck = new List<PlayerCard>();
            foreach (var i in Enumerable.Range(0, epidemicCount))
            {
                var count = numberOfDecks + (deckIncrement > 0 ? 1 : 0);
                var cards = Cards.Skip(i * count).Take(count).ToList();
                deckIncrement = deckIncrement == 0 ? 0 : deckIncrement - 1;
                cards.Add(new EpidemicCard());
                Shuffle(cards);
                newDeck.AddRange(cards);
            }            

            InnerCards = newDeck;
        }

        public IEnumerable<EventCard> EventCards => Cards.OfType<EventCard>();
    }
}