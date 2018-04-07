using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PlayerCard Card { get; set; }

        public bool IsPossible(MapCity city)
        {
            return Character.HasCityCard(Character.CurrentMapCity.City);
        }

        public bool Move(MapCity city)
        {
            Character.CurrentMapCity = city;
            Character.RemoveCard(Card);
            return true;
        }
    }
}
