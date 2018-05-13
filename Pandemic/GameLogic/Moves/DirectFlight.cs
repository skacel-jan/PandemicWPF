using Pandemic.Cards;
using System.Linq;

namespace Pandemic
{
    public class DirectFlight : IMoveCardAction
    {
        public DirectFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType { get => ActionTypes.DirectFlight; }

        public bool IsPossible(MapCity city)
        {
            return Character.CityCards.Any(card => card.City == city.City);
        }

        public bool Move(MapCity city, PlayerCard card)
        {
            if (city.City == card.City)
            {
                Character.CurrentMapCity = city;
                Character.RemoveCard(card);
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}