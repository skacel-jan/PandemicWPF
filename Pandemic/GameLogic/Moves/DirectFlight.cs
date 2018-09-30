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
            return Character.CityCards.Any(card => card.City == city.City);
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            var action = new SelectAction<Card>(SetCard, Character.Cards.OfType<CityCard>(),
                $"Select card of a destination city {city.City.Name}", (c) => c is CityCard cCard && city.City == cCard.City);

            game.SelectionService.Select(action);

            void SetCard(Card card)
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