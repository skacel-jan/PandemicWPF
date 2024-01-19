using Game.Pandemic.GameLogic.Actions.Moves;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.CharacterActions.SpecialCharacterActions
{
    public class DispatcherMoveAction : MoveAction
    {
        public DispatcherMoveAction(Character character, Game game) : base(character, game)
        {
            MoveActions.Add(new DispatcherSpecialMove(character));
        }
    }
}