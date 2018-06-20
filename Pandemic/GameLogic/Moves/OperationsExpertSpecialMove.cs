using System;

namespace Pandemic
{
    public class OperationsExpertSpecialMove : IMoveAction
    {
        public OperationsExpertSpecialMove(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public bool IsCardRequired => true;
        public string MoveType => ActionTypes.OperationsExpertSpecialMove;

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && Character.Cards.Count > 0;
        }

        public void Move(Game game, MapCity city, Action finishAction)
        {
            Character.CurrentMapCity = city;
            //Character.RemoveCard(card);
        }
    }
}