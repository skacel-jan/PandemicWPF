using System;
using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Game.Pandemic.GameLogic;

namespace Game.Pandemic.ViewModels
{
    public class DiseaseSelectionViewModel : ViewModelBase
    {
        public ICommand DiseaseSelectedCommand { get; private set; }

        public IEnumerable<DiseaseColor> Diseases { get; private set; }
        public Action<DiseaseColor> Action { get; }

        public DiseaseSelectionViewModel(Action<DiseaseColor> action, IEnumerable<DiseaseColor> diseases)
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