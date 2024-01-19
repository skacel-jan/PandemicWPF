using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Actions.Selections;
using Game.Pandemic.GameLogic.Cards;

namespace Game.Pandemic.GameLogic.Actions.EventActions
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
                new CardSelection(SelectionCallback((Card c) => _card = (InfectionCard)c),
                                  Game.Infection.DiscardPile.Cards,
                                  "Select infection card")
               );


        }
    }
}