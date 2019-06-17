using System;
using System.Collections.Generic;
using System.Linq;
using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class DispatcherSpecialMove : IMoveAction
    {
        public DispatcherSpecialMove(Character character)
        {
            Character = character;
        }

        public Character Character { get; }
        public string MoveType => ActionTypes.DispatcherMove;

        public bool IsPossible(MapCity city)
        {
            return city.Characters.Any();
        }

        public override string ToString() => MoveType;
    }
}