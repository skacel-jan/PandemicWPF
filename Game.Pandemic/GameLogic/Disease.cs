using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Game.Pandemic.GameLogic
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

        private int _cubes;
        private State _status;

        public Disease(DiseaseColor color)
        {
            Color = color;
            Cubes = STARTING_CUBES_COUNT;
        }

        public enum State
        {
            NotCured = 0,
            Cured,
            Eradicated
        }

        public DiseaseColor Color { get; private set; }

        public int Cubes
        {
            get => _cubes;
            set
            {
                if (SetProperty(ref _cubes, value))
                {
                    if (Status == State.Cured && _cubes == STARTING_CUBES_COUNT)
                    {
                        Status = State.Eradicated;
                    }
                }
            }
        }

        public State Status { get => _status; set => SetProperty(ref _status, value); }
    }

    public class DiseaseFactory
    {
        private IDictionary<DiseaseColor, Disease> _diseases;

        public IDictionary<DiseaseColor, Disease> GetDiseases()
        {
            return _diseases ?? (_diseases = new Dictionary<DiseaseColor, Disease>()
                {
                    { DiseaseColor.Black, new Disease(DiseaseColor.Black) },
                    { DiseaseColor.Blue, new Disease(DiseaseColor.Blue)},
                    { DiseaseColor.Red, new Disease(DiseaseColor.Red)},
                    { DiseaseColor.Yellow, new Disease(DiseaseColor.Yellow)}
                }
            );
        }
    }
}