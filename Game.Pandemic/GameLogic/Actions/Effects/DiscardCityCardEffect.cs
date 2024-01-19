using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Decks;

namespace Game.Pandemic.GameLogic.Actions.Effects
{
    internal class DiscardPlayerCardEffect : IEffect
    {
        private readonly PlayerCard _cityCard;
        private readonly DiscardPile<PlayerCard> _playerDiscardPile;

        public DiscardPlayerCardEffect(PlayerCard cityCard, DiscardPile<PlayerCard> playerDiscardPile)
        {
            _cityCard = cityCard;
            _playerDiscardPile = playerDiscardPile;
        }

        public void Execute()
        {
            _cityCard.Character.RemoveCard(_cityCard);
            _playerDiscardPile.AddCard(_cityCard);
        }
    }
}