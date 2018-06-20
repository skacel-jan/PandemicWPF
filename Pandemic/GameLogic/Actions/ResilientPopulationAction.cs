using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class ResilientPopulationAction : EventAction
    {
        protected override void Execute()
        {
            _game.SelectCard(_game.InfectionDiscardPile.Cards, SetCard, "Select infection card");
        }

        private void SetCard(Card card)
        {
            if (card is InfectionCard infectionCard)
            {
                _game.InfectionDiscardPile.RemoveCard(infectionCard);
                _game.RemovedCards.AddCard(card);

                FinishAction();
            }
        }
    }
}