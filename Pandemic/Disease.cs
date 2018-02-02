using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace Pandemic
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
        public static readonly int STARTING_CUBES_COUNT = 24;

        private bool _isCured;
        private bool _isEradicated;
        private int _cubes;

        public Disease(DiseaseColor color)
        {
            Color = color;
            Cubes = STARTING_CUBES_COUNT;
        }

        public DiseaseColor Color { get; private set; }

        public bool IsCured
        {
            get => _isCured;
            set => Set(ref _isCured, value);
        }

        public bool IsEradicated
        {
            get => _isEradicated;
            set => Set(ref _isEradicated, value);
        }

        public int Cubes
        {
            get => _cubes;
            set => Set(ref _cubes, value);
        }

        public static Dictionary<DiseaseColor, Disease> CreateDiseases()
        {
            return new Dictionary<DiseaseColor, Disease>()
            {
                { DiseaseColor.Black, new Disease(DiseaseColor.Black) },
                { DiseaseColor.Blue, new Disease(DiseaseColor.Blue)},
                { DiseaseColor.Red, new Disease(DiseaseColor.Red)},
                { DiseaseColor.Yellow, new Disease(DiseaseColor.Yellow)}
            };
        }
    }
}