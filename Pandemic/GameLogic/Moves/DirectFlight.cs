using System.Linq;

namespace Pandemic
{
    public class DirectFlight : IMoveCardAction
    {
        public DirectFlight(Character character)
        {
            Character = character;
        }

        public PlayerCard Card { get; set; }
        public Character Character { get; set; }
        public string MoveType { get => ActionTypes.DirectFlight; }

        public bool IsPossible(MapCity city)
        {
            return Character.Cards.Any(card => card.City == city.City);
        }

        public bool Move(MapCity city)
        {
            Character.CurrentMapCity = city;
            Character.RemoveCard(Card);
            return true;
        }
    }
}