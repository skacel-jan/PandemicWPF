using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class MoveSelectionViewModel : SelectionViewModel<IMoveAction>
    {
        public MoveSelectionViewModel(IEnumerable<IMoveAction> items, Action<IMoveAction> callbackAction) : base(items, callbackAction)
        {
        }
    }
}

