using System;
using System.Collections.Generic;
using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class DriveOrFerry : IMoveAction
    {
        public DriveOrFerry(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }

        public string MoveType { get => ActionTypes.DriveOrFerry; }

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.IsCityConnected(city);
        }

        public override string ToString() => MoveType;
    }
}