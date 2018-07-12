using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class ResilientPopulationAction : EventAction
    {
        protected override void Execute()
        {
            Game.SelectCard(Game.InfectionDiscardPile.Cards, SetCard, "Select infection card");
        }

        private void SetCard(Card card)
        {
            if (card is InfectionCard infectionCard)
            {
                Game.InfectionDiscardPile.RemoveCard(infectionCard);
                Game.RemovedCards.AddCard(card);

                FinishAction();
            }
        }
    }
}