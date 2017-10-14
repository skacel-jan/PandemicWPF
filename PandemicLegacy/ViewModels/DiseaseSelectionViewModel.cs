﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PandemicLegacy.ViewModels
{
    public class DiseaseSelectionViewModel : ViewModelBase
    {
        public event EventHandler<DiseaseColor> DiseaseSelected;

        public ICommand DiseaseSelectedCommand { get; private set; }

        public IEnumerable<DiseaseColor> Diseases { get; private set; }

        public DiseaseSelectionViewModel(IEnumerable<DiseaseColor> diseases)
        {
            Diseases = diseases;
            DiseaseSelectedCommand = new RelayCommand<DiseaseColor>(color => OnDiseaseSelected(color));
        }

        protected void OnDiseaseSelected(DiseaseColor color)
        {
            DiseaseSelected?.Invoke(this, color);
        }
    }
}