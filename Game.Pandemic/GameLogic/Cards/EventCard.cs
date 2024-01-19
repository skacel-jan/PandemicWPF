using System;
using Game.Pandemic.GameLogic.Actions;
using Game.Pandemic.GameLogic.Actions.EventActions;

namespace Game.Pandemic.GameLogic.Cards
{
    public class EventCard : PlayerCard
    {
        public EventCard(string name, Func<Game, EventCard, EventAction> createEventAction) : base(name)
        {
            CreateEventAction = createEventAction ?? throw new ArgumentNullException(nameof(createEventAction));
        }

        public bool IsHeldByContingencyPlanner { get; set; }

        public Func<Game, EventCard, EventAction> CreateEventAction { get; }

        public override int SortCode => 1;

        public IGameAction GetEventAction(Game game)
        {
            return CreateEventAction(game, this);
        }
    }
}