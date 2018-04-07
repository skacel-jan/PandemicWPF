using System.Collections.Generic;
using System.Linq;

namespace Pandemic
{
    public class MoveStrategy
    {
        protected IDictionary<string, IMoveAction> _moveActions;

        public MoveStrategy(Character character)
        {
            _moveActions = new Dictionary<string, IMoveAction>()
            {
                {ActionTypes.DriveOrFerry, new DriveOrFerry(character) },
                {ActionTypes.DirectFlight, new DirectFlight(character) },
                {ActionTypes.ShuttleFlight, new ShuttleFlight(character) },
                {ActionTypes.CharterFlight, new CharterFlight(character) }
            };
        }

        public IMoveAction GetMoveAction(string moveType, PlayerCard card)
        {
            var move = _moveActions[moveType];
            if (card != null)
            {
                (move as IMoveCardAction).Card = card;
            }

            return move;
        }

        public IEnumerable<IMoveAction> GetPossibleMoves()
        {
            return _moveActions.Values.OrderBy(x => (x is IMoveCardAction));
        }
    }

    public class OperationsExpertMoveStrategy : MoveStrategy
    {
        public OperationsExpertMoveStrategy(Character character) : base(character)
        {
            _moveActions.Add(ActionTypes.OperationsExpertSpecialMove, new OperationsExpertSpecialMove(character));
        }
    }
}