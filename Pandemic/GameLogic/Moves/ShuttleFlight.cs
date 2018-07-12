﻿using System;

namespace Pandemic.GameLogic.Actions
{
    public class ShuttleFlight : IMoveAction
    {
        public ShuttleFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType => ActionTypes.ShuttleFlight;

        public bool IsCardRequired => false;

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && city.HasResearchStation;
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            game.MoveCharacter(Character, city);
            moveActionCallback();
        }
    }
}