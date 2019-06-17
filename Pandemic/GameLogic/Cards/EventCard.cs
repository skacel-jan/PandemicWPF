using Pandemic.GameLogic.Actions;
using System;

namespace Pandemic.Cards
{
    public class EventCard : PlayerCard
    {
        public EventCard(string name, Func<Game, EventCard, Character, EventAction> createEventAction) : base(name)
        {
            CreateEventAction = createEventAction ?? throw new ArgumentNullException(nameof(createEventAction));
        }

        public bool IsReturnedByContingencyPlanner { get; set; }

        public Func<Game, EventCard, Character, EventAction> CreateEventAction { get; }

        public IGameAction GetEventAction(Game game)
        {
            return CreateEventAction(game, this, Character);
        }
    }
}