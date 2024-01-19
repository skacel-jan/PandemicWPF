using System;
using System.Collections.Generic;
using Game.Pandemic.GameLogic.Services;

namespace Game.Pandemic.GameLogic.Actions.Selections
{
    internal class ShareTypeSelection : Selection
    {
        private readonly Action<ShareType> _action;
        private readonly IEnumerable<ShareType> _items;
        
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