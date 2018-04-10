using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic
{
    public class MoveStrategy
    {
        protected IDictionary<string, IMoveAction> _moveActions;

        protected IDictionary<string, IMoveCardAction> _moveCardActions;

        public MoveStrategy(Character character)
        {
            _moveActions = new Dictionary<string, IMoveAction>()
            {
                {ActionTypes.DriveOrFerry, new DriveOrFerry(character) },
                {ActionTypes.ShuttleFlight, new ShuttleFlight(character) }
            };

            _moveCardActions = new Dictionary<string, IMoveCardAction>()
            {
                {ActionTypes.DirectFlight, new DirectFlight(character) },
                {ActionTypes.CharterFlight, new CharterFlight(character) }
            };
        }

        public IMoveAction GetMoveAction(string moveType)
        {
            return _moveActions[moveType];
        }

        public IMoveCardAction GetCardMoveAction(string moveType)
        {
            return _moveCardActions[moveType];
        }

        public IEnumerable<IMoveAction> GetPossibleMoves()
        {
            return _moveActions.Values;
        }

        public IEnumerable<IMoveCardAction> GetPossibleCardMoves()
        {
            return _moveCardActions.Values;
        }
    }

    public class OperationsExpertMoveStrategy : MoveStrategy
    {
        public OperationsExpertMoveStrategy(Character character) : base(character)
        {
            _moveCardActions.Add(ActionTypes.OperationsExpertSpecialMove, new OperationsExpertSpecialMove(character));
        }
    }
}