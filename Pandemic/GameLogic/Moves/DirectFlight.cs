using Pandemic.Cards;
using System;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class DirectFlight : IMoveAction
    {
        public DirectFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public bool IsCardRequired => true;
        public string MoveType => ActionTypes.DirectFlight;

        public bool IsPossible(Game game, MapCity city)
        {
            return Character.HasCityCard(city.City);
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            var action = new SelectAction<CityCard>(SetCard, Character.CityCards,
                $"Select card of a destination city {city.City.Name}", (card) => city.City == card.City);

            game.SelectionService.Select(action);

            void SetCard(CityCard card)
            {
                Character.CurrentMapCity = city;
                Character.RemoveCard(card);
                game.AddCardToPlayerDiscardPile(card);
                moveActionCallback();
            }
        }

        public override string ToString() => MoveType;
    }
}