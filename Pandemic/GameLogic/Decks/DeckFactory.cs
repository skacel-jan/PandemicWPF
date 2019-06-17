using Pandemic.Cards;
using Pandemic.Decks;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic.Decks
{
    public class DeckFactory
    {
        public WorldMap WorldMap { get; }

        private readonly Lazy<IList<EventCard>> _eventCards = new Lazy<IList<EventCard>>(CreateEventCards);

        public PlayerDeck CreatePlayerDeck(WorldMap worldMap)
        {
            PlayerDeck playerDeck = new PlayerDeck(worldMap.Cities.Select(c => new CityCard(c.City)));

            foreach (var card in GetEventCards())
            {
                playerDeck.AddCard(card);
            }

            return playerDeck;
        }

        public IEnumerable<EventCard> GetEventCards()
        {
            return _eventCards.Value;
        }

        private static IList<EventCard> CreateEventCards()
        {
            return new List<EventCard>()
            {
                new EventCard("Government Grant", (game, eventCard) => new GovernmentGrantAction(eventCard, game)),
                new EventCard("One Quiet Night", (game, eventCard) => new OneQuietNightAction(eventCard, game)),
                new EventCard("Airlift", (game, eventCard) => new AirliftAction(eventCard, game)),
                new EventCard("Resilient population", (game, eventCard) => new ResilientPopulationAction(eventCard, game))
            };
        }
    }
}
