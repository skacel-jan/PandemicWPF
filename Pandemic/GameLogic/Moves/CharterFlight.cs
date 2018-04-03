using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class CharterFlight : IMoveAction
    {
        public CharterFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public bool IsCardNeeded => true;
        public string MoveType { get => ActionTypes.CharterFlight; }

        public bool CanMove(MapCity city)
        {
            return Character.HasCityCard(Character.CurrentMapCity.City);
        }

        public bool Move(MapCity city)
        {
            Character.CurrentMapCity = city;
            Character.RemoveCard(Character.SelectedCard);
            return true;
        }
    }
}
