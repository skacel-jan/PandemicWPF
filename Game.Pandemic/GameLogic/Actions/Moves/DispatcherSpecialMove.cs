using System.Linq;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.Moves
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