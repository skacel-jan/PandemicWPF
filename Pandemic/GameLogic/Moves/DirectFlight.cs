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
                        game.MoveCharacter(Character, city);
                        Character.RemoveCard(card);
                        game.AddCardToPlayerDiscardPile(card);
                        moveActionCallback();
                    }
                }
            }, "Select card of a destination city");
        }
    }
}