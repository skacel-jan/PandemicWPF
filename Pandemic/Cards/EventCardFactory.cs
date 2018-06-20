using Pandemic.GameLogic.Actions;
using System.Collections.Generic;

namespace Pandemic.Cards
{
    public class EventCardFactory
    {
        private IList<EventCard> _eventCards;

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
                new EventCard("Government Grant", () => new GovernmentGrantAction()),
                new EventCard("One Quiet Night", () => new OneQuietNightAction()),
                new EventCard("Airlift", () => new AirliftAction()),
                new EventCard("Resilient population", () => new ResilientPopulationAction()),
            };
        }
    }
}