using Pandemic.Cards;

namespace Pandemic
{
    public class CharterFlight : IMoveCardAction
    {
        public CharterFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType { get => ActionTypes.CharterFlight; }

        public bool IsPossible(MapCity city)
        {
            return Character.HasCityCard(Character.CurrentMapCity.City);
        }

        public bool Move(MapCity city, PlayerCard card)
        {
            if (Character.CurrentMapCity.City == card.City)
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