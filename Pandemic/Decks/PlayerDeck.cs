using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.Decks
{
    public interface IEventCardFactory
    {
        IEnumerable<EventCard> GetEventCards();
    }

    public class EventCardFactory : IEventCardFactory
    {
        public EventCardFactory()
        {
            //Board = board ?? throw new ArgumentNullException(nameof(board));
        }

        public Board Board { get; }

        public IEnumerable<EventCard> GetEventCards()
        {
            yield return GetGovermentGrant();
        }

        private EventCard GetGovermentGrant()
        {
            var action = new Action(() =>
                {
                });
            return new EventCard("Government Grant");
        }
    }

    public class PlayerDeck : Deck<Card>
    {
        public PlayerDeck(IEnumerable<City> cities, IEventCardFactory eventCardFactory) : base(cities.Select(city => new PlayerCard(city)))
        {
            Cards.Insert(0, new EventCard("Grant"));
            //foreach (var card in eventCardFactory.GetEventCards())
            //{
            //    Cards.Add(card);
            //}
        }

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