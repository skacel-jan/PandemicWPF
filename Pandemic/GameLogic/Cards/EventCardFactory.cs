using Pandemic.GameLogic.Actions;
using System.Collections.Generic;

namespace Pandemic.Cards
{
    public class EventCardFactory
    {
        private IList<EventCard> _eventCards;

        public EventCardFactory(SelectionService selectionService)
        {
            SelectionService = selectionService;
        }

        public SelectionService SelectionService { get; }

        public IEnumerable<EventCard> GetEventCards()
        {
            if (_eventCards == null)
            {
                CreateEventCards();
            }

            return _eventCards;
        }

        private void CreateEventCards()
        {
            _eventCards = new List<EventCard>()
            {
                new EventCard("Government Grant", (game, eventCard, character) => new GovernmentGrantAction(eventCard, game)),
                new EventCard("One Quiet Night", (game, eventCard, character) => new OneQuietNightAction(eventCard, game)),
                new EventCard("Airlift", (game, eventCard, character) => new AirliftAction(eventCard, game, character)),
                new EventCard("Resilient population", (game, eventCard, character) => new ResilientPopulationAction(eventCard, game)),
            };
        }
    }
}