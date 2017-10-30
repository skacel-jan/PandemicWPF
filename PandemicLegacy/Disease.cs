using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy
{
    public enum DiseaseColor
    {
        Black,
        Blue,
        Red,
        Yellow
    }

    public class Disease : ObservableObject
    {
        public DiseaseColor Color { get; private set; }

        private bool _isEradicated;
        public bool IsEradicated { get => _isEradicated; set => Set(ref _isEradicated, value); }

        private bool _isCured;
        public bool IsCured { get => _isCured; set => Set(ref _isCured, value); }

        public Disease(DiseaseColor color)
        {
            this.Color = color;
        }
    }

    public class Diseases : Dictionary<DiseaseColor, Disease>
    {
        public Diseases()
        {
            this.Add(DiseaseColor.Black, new Disease(DiseaseColor.Black));
            this.Add(DiseaseColor.Blue, new Disease(DiseaseColor.Blue));
            this.Add(DiseaseColor.Red, new Disease(DiseaseColor.Red));
            this.Add(DiseaseColor.Yellow, new Disease(DiseaseColor.Yellow));
        }

        public Disease Black { get => this[DiseaseColor.Black]; }
        public Disease Blue { get => this[DiseaseColor.Blue]; }
        public Disease Red { get => this[DiseaseColor.Red]; }
        public Disease Yellow { get => this[DiseaseColor.Yellow]; }

    }
}
