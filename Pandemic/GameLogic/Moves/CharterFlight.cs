using Pandemic.Cards;
using System;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class CharterFlight : IMoveAction
    {
        public CharterFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public bool IsCardRequired => true;
        public string MoveType => ActionTypes.CharterFlight;

        public bool IsPossible(Game game, MapCity city)
        {
            return Character.HasCityCard(Character.CurrentMapCity.City);
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            var action = new SelectAction<Card>(SetCard, Character.Cards.OfType<CityCard>(),
                $"Select card of a current city {city.City.Name}",
                (Card card) => card is CityCard cityCard && Character.CurrentMapCity.City == cityCard.City);

            game.SelectionService.Select(action);

            void SetCard(Card selectedCard)
            {
                Character.CurrentMapCity = city;
                Character.RemoveCard(selectedCard);
                game.AddCardToPlayerDiscardPile(selectedCard);
                moveActionCallback();
            }
        }

        public override string ToString() => MoveType;
    }
}