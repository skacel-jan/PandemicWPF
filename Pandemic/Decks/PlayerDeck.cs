using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.Decks
{
    public class PlayerDeck : Deck<Card>
    {
        public PlayerDeck(IEnumerable<City> cities) : base(cities.Select(city => new PlayerCard(city)))
        { }

        public void AddEpidemicCards(int epidemicCount)
        {
            var numberOfDecks = Cards.Count / epidemicCount;
            var deckIncrement = Cards.Count % epidemicCount;

            List<Card> resultDeck = new List<Card>();
            foreach (var i in Enumerable.Range(0, epidemicCount))
            {
                var count = numberOfDecks + (deckIncrement > 0 ? 1 : 0);
                var deck = new Deck<Card>(Cards.Skip(i * count).Take(count));
                deckIncrement = deckIncrement == 0 ? 0 : deckIncrement - 1;
                deck.Cards.Add(new EpidemicCard("Epidemic"));
                deck.Shuffle();
                resultDeck.AddRange(deck.Cards);
            }
            Cards.Clear();

            foreach (var card in resultDeck)
            {
                Cards.Add(card);
            }
        }
    }
}