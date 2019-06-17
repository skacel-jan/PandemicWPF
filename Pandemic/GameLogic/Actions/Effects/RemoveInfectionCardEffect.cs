using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    internal class RemoveInfectionCardEffect : IEffect
    {
        private readonly InfectionCard _card;
        private readonly Game _game;

        public RemoveInfectionCardEffect(InfectionCard card, Game game)
        {
            _card = card;
            _game = game;
        }

        public void Execute()
        {
            _game.Infection.DiscardPile.RemoveCard(_card);
            _game.RemovedCards.AddCard(_card);
        }
    }
}