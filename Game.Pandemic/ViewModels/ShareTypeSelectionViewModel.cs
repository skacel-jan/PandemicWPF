using System;
using System.Collections.Generic;
using Game.Pandemic.GameLogic;

namespace Game.Pandemic.ViewModels
{
    public class ShareTypeSelectionViewModel : SelectionViewModel<ShareType>
    {
        public ShareTypeSelectionViewModel(Action<ShareType> callbackAction, IEnumerable<ShareType> items) : base(items, callbackAction)
        {
        }
    }
}