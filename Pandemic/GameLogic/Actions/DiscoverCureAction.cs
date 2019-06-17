using Pandemic.Cards;
using Pandemic.Characters;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
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
            return Character.CurrentMapCity.HasResearchStation && Character.CardsCountOfColor(Character.MostCardsColor) >= Character.CardsForCure;
        }

        protected override void AddEffects()
        {
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

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            if (Character.MostCardsColorCount > Character.CardsForCure)
            {
                yield return new CardsSelection<CityCard>(SetSelectionCallback((IEnumerable<CityCard> c) => _cards = c),
                    Character.CityCards.Where(card => card.City.Color == Character.MostCardsColor),
                    $"Select {Character.CardsForCure} cards of {Character.MostCardsColor} color to discover a cure",
                    ValidateCards);
            }

            bool ValidateCards(IEnumerable<Card> cards)
            {
                return cards.Count() == Character.CardsForCure && cards.All(c => (c as CityCard).City.Color == Character.MostCardsColor);
            }
        }
    }
}