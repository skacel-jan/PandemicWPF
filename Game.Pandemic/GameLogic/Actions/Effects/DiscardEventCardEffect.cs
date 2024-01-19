using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Decks;

namespace Game.Pandemic.GameLogic.Actions.Effects
{
    public class DiscardEventCardEffect : IEffect
    {
        private readonly DiscardPile<PlayerCard> _pile;
        private readonly EventCard _card;

        public DiscardEventCardEffect(DiscardPile<PlayerCard> pile, EventCard card)
        {
            _pile = pile;
            _card = card;
        }

        public void Execute()
        {
            _card.Character.RemoveCard(_card);
            _pile.AddCard(_card);
        }
    }
}