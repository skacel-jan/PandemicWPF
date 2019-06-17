namespace Pandemic.GameLogic.Actions
{
    public class DispatcherMoveAction : MoveAction
    {
        public DispatcherMoveAction(Character character, Game game) : base(character, game)
        {
            MoveActions.Add(new DispatcherSpecialMove(character));
        }
    }
}