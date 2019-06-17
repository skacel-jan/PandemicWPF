using Pandemic.Cards;
using Pandemic.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class OperationsExpertSpecialMove : ICardMoveAction
    {
        public OperationsExpertSpecialMove(OperationsExpert character)
        {
            Character = character;
        }

        public Character Character { get; }
        public string MoveType => "Special move";

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && Character.Cards.OfType<CityCard>().Any();
        }        

        public override string ToString() => MoveType;

        public bool Validate(MapCity city, CityCard card) => true;
    }
}