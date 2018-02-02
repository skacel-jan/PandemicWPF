using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.ViewModels
{
    internal class EpidemicViewModel : TextViewModel
    {
        internal enum EpidemicState
        {
            RaiseInfection,
            DrawBottomCard,
            ShuffleDeck
        }

        private EpidemicState _state;

        public EpidemicState MyProperty
        {
            get => _state;
            set => Set(ref _state, value);
        }

        public EpidemicViewModel(string text) : base(text)
        {

        }
    }
}
