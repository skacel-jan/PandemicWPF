using Pandemic.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.Decks
{
    public class PlayerDeck : Deck<Card>
    {
        public PlayerDeck(IEnumerable<PlayerCard> cards) : base(cards)
        {
        }

        public void AddEpidemicCards(int epidemicCount)
        {
            var numberOfDecks = InnerCards.Count / epidemicCount;
            var deckIncrement = InnerCards.Count % epidemicCount;

            List<Card> resultDeck = new List<Card>();
            foreach (var i in Enumerable.Range(0, epidemicCount))
            {
                var count = numberOfDecks + (deckIncrement > 0 ? 1 : 0);
                var deck = new Deck<Card>(Cards.Skip(i * count).Take(count));
                deckIncrement = deckIncrement == 0 ? 0 : deckIncrement - 1;
                deck.AddCard(new EpidemicCard("Epidemic"));
                deck.Shuffle();
                resultDeck.AddRange(deck.Cards);
            }
            InnerCards.Clear();

            foreach (var card in resultDeck)
            {
                InnerCards.Add(card);
            }
        }

        public IEnumerable<EventCard> EventCards => Cards.OfType<EventCard>();
    }
}