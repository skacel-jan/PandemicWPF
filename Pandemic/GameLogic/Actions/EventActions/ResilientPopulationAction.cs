using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class ResilientPopulationAction : EventAction
    {
        public ResilientPopulationAction(EventCard card, Game game) : base(card, game)
        {
        }

        public override void Execute()
        {
            Game.SelectionService.Select(new SelectAction<Card>(SelectCard, Game.Infection.DiscardPile.Cards,
                "Select infection card", (card) => card is InfectionCard infectionCard));
        }

        private void SelectCard(Card card)
        {
            Game.Infection.DiscardPile.RemoveCard((InfectionCard)card);
            Game.RemovedCards.AddCard(card);

            FinishAction();
        }
    }
}