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

        public IEnumerable<string> Moves { get; private set; }

        public MoveSelectionViewModel(IEnumerable<string> moves)
        {
            Moves = moves;
            MoveSelectedCommand = new RelayCommand<string>(move => OnMoveSelected(move));
        }

        protected void OnMoveSelected(string move)
        {
            MoveType type;
            if (move == "Direct flight")
                type = MoveType.Direct;
            else
                type = MoveType.Charter;
            MessengerInstance.Send(type, "MoveSelection");
        }
    }

    public enum MoveType
    {
        Direct,
        Charter
    }
}

