using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class CharterFlight : ICardMoveAction
    {
        public CharterFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType => ActionTypes.CharterFlight;

        public bool IsPossible(MapCity city)
        {
            return Character.HasCityCard(Character.CurrentMapCity.City);
        }

        public override string ToString() => MoveType;

        public bool Validate(MapCity city, CityCard card)
        {
            return Character.CurrentMapCity.City == card.City;
        }
    }
}