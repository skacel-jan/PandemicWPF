using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PandemicLegacy.ViewModels
{
    public class MoveSelectionViewModel : ViewModelBase
    {
        public event EventHandler<string> MoveSelected;

        public ICommand MoveSelectedCommand { get; private set; }

        public IEnumerable<string> Moves { get; private set; }

        public MoveSelectionViewModel(IEnumerable<string> moves)
        {
            Moves = moves;
            MoveSelectedCommand = new RelayCommand<string>(move => OnMoveSelected(move));
        }

        protected void OnMoveSelected(string move)
        {
            MoveSelected?.Invoke(this, move);
        }
    }
}

