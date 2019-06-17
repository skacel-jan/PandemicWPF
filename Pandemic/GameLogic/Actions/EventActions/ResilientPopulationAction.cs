using System.Collections.Generic;
using System.Linq;
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
            base.AddEffects();
            Effects.Add(new RemoveInfectionCardEffect(_card, Game));
        }

        protected override void Initialize()
        {
            AddSelectionState(0,
                new CardSelection(SetSelectionCallback((Card c) => _card = (InfectionCard)c),
                                  Game.Infection.DiscardPile.Cards,
                                  "Select infection card")
               );


        }
    }
}