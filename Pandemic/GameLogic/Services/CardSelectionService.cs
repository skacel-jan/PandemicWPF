using Pandemic.Cards;
using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic
{
    public class CardSelectionService
    {
        public event EventHandler<CardsSelectingEventArgs> CardSelecting;

        public void SelectCard(IEnumerable<Card> cards, string text, Action<Card> action)
        {
            OnCardSelecting(new CardsSelectingEventArgs(cards, text, action));
        }

        protected virtual void OnCardSelecting(CardsSelectingEventArgs e)
        {
            CardSelecting?.Invoke(this, e);
        }
    }
}