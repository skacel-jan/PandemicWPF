using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;

namespace Pandemic.ViewModels
{
    public class MoveSelectionViewModel : SelectionViewModel<IMoveAction>
    {
        public MoveSelectionViewModel(Action<IMoveAction> callbackAction, IEnumerable<IMoveAction> items) : base(items, callbackAction)
        {
        }
    }
}