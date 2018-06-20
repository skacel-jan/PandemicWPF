﻿using System;

namespace Pandemic
{
    public class DriveOrFerry : IMoveAction
    {
        public DriveOrFerry(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }

        public string MoveType { get => ActionTypes.DriveOrFerry; }

        public bool IsCardRequired => false;

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.IsCityConnected(city);
        }

        public void Move(Game game, MapCity city, Action finishAction)
        {
            Character.CurrentMapCity = city;
            finishAction();
        }
    }
}