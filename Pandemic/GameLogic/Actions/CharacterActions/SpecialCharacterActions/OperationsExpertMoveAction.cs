using Pandemic.Characters;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class OperationsExpertMoveAction : MoveAction
    {
        private readonly OperationsExpertSpecialMove _operationsExpertSpecialMove;
        private int _specialMovePlayedTurn = 0;

        public OperationsExpertMoveAction(OperationsExpert character, Game game) : base(character, game)
        {
            _operationsExpertSpecialMove = new OperationsExpertSpecialMove(character);
            CardMoveActions.Add(_operationsExpertSpecialMove);
        }

        protected override void AddEffects()
        {
            base.AddEffects();

            if (SelectedMoveAction == _operationsExpertSpecialMove)
            {
                _specialMovePlayedTurn = Game.Turn;
            }
        }

        protected override void Initialize()
        {
            if (_specialMovePlayedTurn == Game.Turn)
            {
                CardMoveActions.Remove(_operationsExpertSpecialMove);
            }
            else if (!CardMoveActions.Contains(_operationsExpertSpecialMove))
            {
                CardMoveActions.Add(_operationsExpertSpecialMove);
            }

            base.Initialize();
        }
    }
}