using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;

namespace Pandemic.ViewModels
{
    public class MoveSelectionViewModel : SelectionViewModel<IMoveAction>
    {
        public MoveSelectionViewModel(IEnumerable<IMoveAction> items, Action<IMoveAction> callbackAction) : base(items, callbackAction)
        {
        }
    }
}