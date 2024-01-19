using System;
using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Cards;

namespace Game.Pandemic.GameLogic.Actions.EventActions
{
    public abstract class EventAction : GameActionBase
    {
        public EventCard EventCard { get; }
        public override string Name => ActionTypes.Event;

        protected EventAction(EventCard card, Game game) : base(game)
        {
            EventCard = card ?? throw new ArgumentNullException(nameof(card));
        }

        protected override void AddEffects()
        {
            if (EventCard.IsHeldByContingencyPlanner)
            {
                Effects.Add(new RemoveEventCardEffect(Game.RemovedCards, EventCard));
            }
            else
            {
                Effects.Add(new DiscardEventCardEffect(Game.PlayerDiscardPile, EventCard));
            }
        }
    }
}