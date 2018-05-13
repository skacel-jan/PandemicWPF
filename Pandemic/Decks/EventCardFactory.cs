using Pandemic.Cards;
using Pandemic.GameLogic;
using System;
using System.Collections.Generic;

namespace Pandemic.Decks
{
    public interface IEventCardFactory
    {
        IEnumerable<EventCard> GetEventCards();
    }

    public class EventCardFactory : IEventCardFactory
    {
        private IList<EventCard> _eventCards;

        public EventCardFactory(CitySelectionService citySelectionService, CharacterSelectionService characterSelectionService,
            CardSelectionService cardSelectionService)
        {
            CitySelectionService = citySelectionService ?? throw new ArgumentNullException(nameof(citySelectionService));
            CharacterSelectionService = characterSelectionService ?? throw new ArgumentNullException(nameof(characterSelectionService));
            CardSelectionService = cardSelectionService ?? throw new ArgumentNullException(nameof(cardSelectionService));
        }

        private void CreateEventCards()
        {
            var oneQuiteNight = new OneQuietNightCard();
            oneQuiteNight.SkipInfectionPhase += (sender, e) => DecksService.RaiseSkipInfectionPhase();
            _eventCards = new List<EventCard>()
            {
                oneQuiteNight,
                new GovernmentGrantCard(CitySelectionService),
                new AirliftCard(CharacterSelectionService, CitySelectionService),
                new ResilientPopulationCard(CardSelectionService, DecksService),
            };
        }

        public CitySelectionService CitySelectionService { get; }
        public CharacterSelectionService CharacterSelectionService { get; }
        public CardSelectionService CardSelectionService { get; }
        public DecksService DecksService { get; set; }

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