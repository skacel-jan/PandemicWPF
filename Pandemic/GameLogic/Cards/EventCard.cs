using Pandemic.GameLogic.Actions;
using System;

namespace Pandemic.Cards
{
    public class EventCard : PlayerCard
    {
        public EventCard(string name, Func<Game, EventCard, EventAction> createEventAction) : base(name)
        {
            CreateEventAction = createEventAction ?? throw new ArgumentNullException(nameof(createEventAction));
        }

        public bool IsHeldByContingencyPlanner { get; set; }

        public Func<Game, EventCard, EventAction> CreateEventAction { get; }

        public override int SortRank => 1;

        public IGameAction GetEventAction(Game game)
        {
            return CreateEventAction(game, this);
        }
    }
}