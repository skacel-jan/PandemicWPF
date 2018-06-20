using Pandemic.Cards;
using System;
using System.Linq;

namespace Pandemic
{
    public class DirectFlight : IMoveAction
    {
        public DirectFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType => ActionTypes.DirectFlight;

        public bool IsCardRequired => true;

        public bool IsPossible(MapCity city)
        {
            return Character.CityCards.Any(card => card.City == city.City);
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            game.SelectCard(Character.Cards.OfType<CityCard>(), (Card card) =>
            {
                if (card is CityCard cityCard)
                {
                    if (city.City == cityCard.City)
                    {
                        Character.CurrentMapCity = city;
                        Character.RemoveCard(card);
                        game.AddCardToPlayerDiscardPile(card);
                        moveActionCallback();
                    }
                }
            }, "Select card of a destination city");
        }
    }
}