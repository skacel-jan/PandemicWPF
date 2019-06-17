using Pandemic.Cards;
using Pandemic.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class OperationsExpertSpecialMove : ICardMoveAction
    {
        private readonly Game _game;
        private int _lastTurnExecuted = 0;

        public OperationsExpertSpecialMove(OperationsExpert character, Game game)
        {
            Character = character;
            _game = game;
        }

        public Character Character { get; }
        public string MoveType => "Special move";

        public bool IsPossible(MapCity city)
        {
            return _game.Turn != _lastTurnExecuted && Character.CurrentMapCity.HasResearchStation && Character.Cards.Count > 0;
        }        

        public override string ToString() => MoveType;

        public bool Validate(MapCity city, CityCard card) => true;
    }
}