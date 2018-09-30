using System;
using System.Collections.Generic;

namespace Pandemic.ViewModels
{
    public class ShareTypeSelectionViewModel : SelectionViewModel<ShareType>
    {
        public ShareTypeSelectionViewModel(IEnumerable<ShareType> items, Action<ShareType> callbackAction) : base(items, callbackAction)
        {
        }
    }
}