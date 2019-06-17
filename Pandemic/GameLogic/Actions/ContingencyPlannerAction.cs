using Pandemic.Cards;
using Pandemic.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class ContingencyPlannerAction : CharacterAction
    {
        private readonly ContingencyPlanner _contingancyPlanner;
        private EventCard _eventCard;

        public ContingencyPlannerAction(ContingencyPlanner contingancyPlanner, Game game) : base(contingancyPlanner, game)
        {
            _contingancyPlanner = contingancyPlanner;
        }

        public override string Name => "Special";

        public override bool CanExecute()
        {
            return Game.PlayerDiscardPile.Cards.Any(c => c is EventCard);
        }

        protected override void AddEffects()
        {
            Effects.Add(new ReturnEventEffect(_eventCard, Game, _contingancyPlanner));
        }

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            yield return new CardSelection(SetSelectionCallback((Card c) => _eventCard = (EventCard)c), Game.PlayerDiscardPile.Cards.OfType<EventCard>(), 
                "Select event card to save");
        }
    }
}