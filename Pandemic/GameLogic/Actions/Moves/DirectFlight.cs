using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class DirectFlight : ICardMoveAction
    {
        public DirectFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType => ActionTypes.DirectFlight;

        public bool IsPossible(MapCity city)
        {
            return Character.HasCityCard(city.City);
        }

        public override string ToString() => MoveType;

        public bool Validate(MapCity city, CityCard card)
        {
            return city.City == card.City;
        }
    }
}