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
        public bool IsCardNeeded => true;
        public string MoveType { get => ActionTypes.DirectFlight; }

        public bool CanMove(MapCity city)
        {
            return Character.Cards.Any(card => card.City == city.City);
        }

        public bool Move(MapCity city)
        {
            Character.CurrentMapCity = city;
            Character.RemoveCard(Character.SelectedCard);
            return true;
        }
    }
}