using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    internal class GiveCardEffect : IEffect
    {
        private Character _characterFrom;
        private Character _characterTo;
        private CityCard _card;

        public GiveCardEffect(Character characterFrom, Character characterTo, CityCard card)
        {
            _characterFrom = characterFrom;
            _characterTo = characterTo;
            _card = card;
        }

        public void Execute()
        {
            _characterFrom.RemoveCard(_card);
            _characterTo.AddCard(_card);
        }
    }
}