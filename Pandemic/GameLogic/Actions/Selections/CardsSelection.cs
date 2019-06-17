using System;
using System.Collections.Generic;
using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    internal class CardsSelection : Selection
    {
        private readonly Action<IEnumerable<Card>> _action;
        private readonly IEnumerable<Card> _cards;
        private readonly Func<IEnumerable<Card>, bool> _validateCards;

        public CardsSelection(Action<IEnumerable<Card>> action, IEnumerable<Card> cards, string infoText, 
            Func<IEnumerable<Card>, bool> validateCards)
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