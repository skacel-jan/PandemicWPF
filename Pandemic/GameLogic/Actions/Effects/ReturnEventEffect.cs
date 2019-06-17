using Pandemic.Cards;
using Pandemic.Characters;
using System;

namespace Pandemic.GameLogic.Actions
{
    internal class ReturnEventEffect : IEffect
    {
        private readonly EventCard _eventCard;
        private readonly Game _game;
        private readonly ContingencyPlanner _contingencyPlanner;

        public ReturnEventEffect(EventCard eventCard, Game game, ContingencyPlanner contingencyPlanner)
        {
            _eventCard = eventCard;
            _game = game;
            _contingencyPlanner = contingencyPlanner;
        }

        public void Execute()
        {
            _game.PlayerDiscardPile.RemoveCard(_eventCard);
            _eventCard.Character = _contingencyPlanner;
            _eventCard.IsHeldByContingencyPlanner = true;
        }
    }
}