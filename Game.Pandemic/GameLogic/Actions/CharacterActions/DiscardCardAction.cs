using System.Collections.Generic;
using System.Linq;
using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Actions.Selections;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.CharacterActions
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

        protected override void Initialize()
        {
            AddSelectionState(0,
                new CardsSelection(
                    SelectionCallback((IEnumerable<Card> x) => _cards = x.Cast<PlayerCard>()),
                    Character.Cards,
                    "Select cards to discard",
                    cards => ValidateCards(cards.Cast<PlayerCard>())));
        }

        private bool ValidateCards(IEnumerable<PlayerCard> cards)
        {
            return Character.Cards.Count - cards.Count() == Character.CardsLimit;
        }
    }
}