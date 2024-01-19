using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.Effects
{
    internal class GiveCardEffect : IEffect
    {
        private readonly Character _characterFrom;
        private readonly Character _characterTo;
        private readonly CityCard _card;

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