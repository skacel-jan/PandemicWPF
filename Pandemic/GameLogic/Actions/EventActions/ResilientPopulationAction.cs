using System.Collections.Generic;
using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class ResilientPopulationAction : EventAction
    {
        private InfectionCard _card;

        public ResilientPopulationAction(EventCard card, Game game) : base(card, game)
        {
        }

        protected override void AddEffects()
        {
            Effects.Add(new RemoveInfectionCardEffect(_card, Game));
        }

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            yield return new CardSelection(SetSelectionCallback((Card c) => _card = (InfectionCard)c), game.Infection.DiscardPile.Cards, "Select infection card");
        }
    }
}