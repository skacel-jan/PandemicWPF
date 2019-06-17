using Pandemic.Cards;
using Pandemic.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic.Actions
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

