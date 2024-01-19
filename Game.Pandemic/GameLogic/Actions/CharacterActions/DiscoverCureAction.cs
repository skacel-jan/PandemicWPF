using System.Collections.Generic;
using System.Linq;
using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Actions.Selections;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.CharacterActions
{
    public class DiscoverCureAction : CharacterAction
    {
        private IEnumerable<CityCard> _cards;

        public DiscoverCureAction(Character character, Game game) : base(character, game)
        {
        }

        public override string Name => ActionTypes.Discover;

        public override bool CanExecute()
        {
            return Character.CurrentMapCity.HasResearchStation && Character.CardsOSameColor(Character.MostCardsColor) >= Character.CardsForCure;
        }

        protected override void AddEffects()
        {
            base.AddEffects();

            Effects.Add(new DiscoverCureEffect(Game.Diseases[Character.MostCardsColor]));
            foreach (var card in _cards)
            {
                Effects.Add(new DiscardPlayerCardEffect(card, Game.PlayerDiscardPile));
            }

            var medic = Game.Characters.OfType<Medic>().SingleOrDefault();
            if (medic != null)
            {
                Effects.Add(new TreatDiseaseEffect(Game, Character.MostCardsColor, medic));
            }
        }

        protected override void Initialize()
        {
            AddSelectionState(0, 
                (g) => Character.MostCardsColorCount > Character.CardsForCure,
                new CardsSelection(
                    SelectionCallback((IEnumerable<Card> cards) => _cards = cards.Cast<CityCard>()),
                    Character.CityCards.Where(card => card.City.Color == Character.MostCardsColor),
                    $"Select {Character.CardsForCure} cards of {Character.MostCardsColor} color to discover a cure",
                    ValidateCards));
        }

        private bool ValidateCards(IEnumerable<Card> cards)
        {
            return cards.Count() == Character.CardsForCure && cards.All(c => (c as CityCard).City.Color == Character.MostCardsColor);
        }
    }
}