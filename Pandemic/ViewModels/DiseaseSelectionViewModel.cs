using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.GameLogic;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class DiseaseSelectionViewModel : ViewModelBase
    {
        public ICommand DiseaseSelectedCommand { get; private set; }

        public IEnumerable<DiseaseColor> Diseases { get; private set; }
        public Action<DiseaseColor> Action { get; }

        public DiseaseSelectionViewModel(IEnumerable<DiseaseColor> diseases, Action<DiseaseColor> action)
        {
            Diseases = diseases;
            Action = action;
            DiseaseSelectedCommand = new RelayCommand<DiseaseColor>(color => OnDiseaseSelected(color));
        }

        protected void OnDiseaseSelected(DiseaseColor color)
        {
            Action?.Invoke(color);
        }
    }
}