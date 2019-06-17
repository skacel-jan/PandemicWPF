using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    internal class ShareTypeSelection : Selection
    {
        private Action<ShareType> _action;
        private IEnumerable<ShareType> _items;
        
        public ShareTypeSelection(Action<ShareType> action, IEnumerable<ShareType> enumerable, string infoText)
        {
            _action = action;
            _items = enumerable;
            InfoText = infoText;
        }

        public override void Execute(SelectionService service)
        {
            service.SelectShareType(_action, _items, InfoText);
        }
    }
}