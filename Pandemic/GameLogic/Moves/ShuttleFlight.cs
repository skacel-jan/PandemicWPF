﻿namespace Pandemic
{
    public class ShuttleFlight : IMoveAction
    {
        public ShuttleFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public bool IsCardNeeded => false;
        public string MoveType { get => ActionTypes.ShuttleFlight; }

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && city.HasResearchStation;
        }

        public bool Move(MapCity city)
        {
            Character.CurrentMapCity = city;
            return true;
        }
    }
}