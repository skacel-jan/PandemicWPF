using System;
using System.Collections.Generic;
using Game.Pandemic.GameLogic.Actions.Moves;

namespace Game.Pandemic.ViewModels
{
    public class MoveSelectionViewModel : SelectionViewModel<IMoveAction>
    {
        public MoveSelectionViewModel(Action<IMoveAction> callbackAction, IEnumerable<IMoveAction> items) : base(items, callbackAction)
        {
        }
    }
}