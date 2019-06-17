using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    public class MoveSelection : Selection
    {
        private readonly Action<IMoveAction> _selectActionCallback;
        private readonly IEnumerable<IMoveAction> _possibleCardMoveActions;
        private readonly string _text;

        public MoveSelection(Action<IMoveAction> action, IEnumerable<IMoveAction> possibleCardMoveActions, string text)
        {
            _selectActionCallback = action;
            _possibleCardMoveActions = possibleCardMoveActions;
            _text = text;
        }

        public override void Execute(SelectionService service)
        {
            service.SelectMove(_selectActionCallback, _possibleCardMoveActions, _text);
        }
    }
}