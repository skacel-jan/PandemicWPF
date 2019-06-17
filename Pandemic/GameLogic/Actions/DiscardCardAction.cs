using System;
using System.Collections.Generic;
using System.Linq;
using Pandemic.Cards;
using Pandemic.GameLogic.Actions;

namespace Pandemic.GameLogic
{
    internal class DiscardPlayerCardAction : CharacterAction
    {
        private IEnumerable<PlayerCard> _cards;

        public DiscardPlayerCardAction(Character character, Game game) : base(character, game)
        {
        }

        public override string Name => "DiscardCard";

        public override bool CanExecute()
        {
            return Character.HasMoreCardsThenLimit;
        }

        protected override void AddEffects()
        {
            foreach (var card in _cards)
            {
                Effects.Add(new DiscardPlayerCardEffect(card, Game.PlayerDiscardPile));
            }
        }

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            yield return new CardsSelection<PlayerCard>(SetSelectionCallback((IEnumerable<PlayerCard> cards) => _cards = cards), Character.Cards, "Select cards to discard", ValidateCards);
        }

        private bool ValidateCards(IEnumerable<PlayerCard> cards)
        {
            return Character.Cards.Count - cards.Count() == Character.CardsLimit;
        }
    }
}