using Pandemic.Cards;
using Pandemic.Characters;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class DiscoverCureAction : CharacterAction
    {
        public DiscoverCureAction(Character character) : base(character)
        {
        }

        public override string Name => ActionTypes.Discover;

        public override bool CanExecute(Game game)
        {
            return Character.CurrentMapCity.HasResearchStation && Character.CardsCountOfColor(Character.MostCardsColor) >= Character.CardsForCure;
        }

        protected override void Execute()
        {
            if (Character.MostCardsColorCount > Character.CardsForCure)
            {
                SelectCardsForCure();
            }
            else
            {
                DiscoverCure(Character.CityCards.Where(card => card.City.Color == Character.MostCardsColor), Character.MostCardsColor);
            }
        }

        private void DiscoverCure(IEnumerable<CityCard> cards, DiseaseColor color)
        {
            Game.DiscoverCure(color);
            foreach (var card in cards.ToList())
            {
                Character.RemoveCard(card);
                Game.AddCardToPlayerDiscardPile(card);
            }

            Game.Characters.OfType<Medic>().SingleOrDefault()?.SpecialTreatDisease();

            FinishAction();
        }

        private void SelectCards(IEnumerable<Card> cards)
        {
            DiscoverCure(cards.Cast<CityCard>(), Character.MostCardsColor);
        }

        private void SelectCardsForCure()
        {
            var action = new MultiSelectAction<IEnumerable<Card>>(SelectCards, Character.CityCards.Where(card => card.City.Color == Character.MostCardsColor),
                $"Select {Character.CardsForCure} cards of {Character.MostCardsColor} color to discover a cure", ValidateCards);

            Game.SelectionService.Select(action);

            bool ValidateCards(IEnumerable<Card> cards)
            {
                return cards.Count() == Character.CardsForCure && cards.Cast<CityCard>().All(c => c.City.Color == Character.MostCardsColor);
            }
        }
    }
}