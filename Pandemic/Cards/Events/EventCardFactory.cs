using Pandemic.Cards;
using Pandemic.GameLogic;
using System;
using System.Collections.Generic;

namespace Pandemic.Decks
{
    public class EventCardFactory
    {
        private IList<EventCard> _eventCards;

        public EventCardFactory(CitySelectionService citySelectionService)
        {
            CitySelectionService = citySelectionService ?? throw new ArgumentNullException(nameof(citySelectionService));
        }

        private void CreateEventCards()
        {
            _eventCards = new List<EventCard>()
            {
                new OneQuietNightCard(),
                new GovernmentGrantCard(CitySelectionService),
                new AirliftCard(CitySelectionService),
                new ResilientPopulationCard(),
            };
        }

        public CitySelectionService CitySelectionService { get; }

        public IEnumerable<EventCard> GetEventCards()
        {
            if (_eventCards == null)
            {
                CreateEventCards();
            }

            return _eventCards;
        }
    }
}