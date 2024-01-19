using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Decks;

namespace Game.Pandemic.GameLogic.Actions.Effects
{
    public class RemoveEventCardEffect : IEffect
    {
        private readonly DiscardPile<Card> _pile;
        private readonly EventCard _card;

        public RemoveEventCardEffect(DiscardPile<Card> pile, EventCard card)
        {
            _pile = pile;
            _card = card;
        }

        public void Execute()
        {
            _pile.AddCard(_card);
            _card.Character = null;
        }
    }
}

