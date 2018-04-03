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
    public class MoveSelectionViewModel : ViewModelBase
    {
        public ICommand MoveSelectedCommand { get; private set; }

        public IEnumerable<IMoveAction> Moves { get; private set; }

        public MoveSelectionViewModel(IEnumerable<IMoveAction> moves, Action<string> action)
        {
            Moves = moves;
            MoveSelectedCommand = new RelayCommand<string>(action);
        }
    }
}

