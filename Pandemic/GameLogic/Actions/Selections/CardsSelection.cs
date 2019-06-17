using System;
using System.Collections.Generic;
using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    internal class CardsSelection<T> : Selection where T: Card
    {
        private Action<IEnumerable<T>> _action;
        private IEnumerable<T> _cards;
        private Func<IEnumerable<T>, bool> _validateCards;

        public CardsSelection(Action<IEnumerable<T>> action, IEnumerable<T> cards, string infoText, 
            Func<IEnumerable<T>, bool> validateCards)
        {
            _action = action;
            _cards = cards;
            InfoText = infoText;
            _validateCards = validateCards;
        }

        public override void Execute(SelectionService service)
        {
            service.SelectCards(_action, _cards, InfoText, _validateCards);
        }
    }
}