using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace Pandemic
{
    public interface IGameData
    {
        IDictionary<DiseaseColor, Disease> Diseases { get; }
        int InfectionPosition { get; set; }
        int InfectionRate { get; set; }
        int Outbreaks { get; set; }
        int ResearchStationsPile { get; set; }
        IEnumerable<Character> Characters { get; }
    }

    public class GameData : ObservableObject, IGameData
    {
        private IDictionary<DiseaseColor, Disease> _diseases;
        private int _infectionRate;
        private int _outbreaks;
        private int _researchStationPile;

        public GameData(DiseaseFactory diseaseFactory, IEnumerable<Character> characters)
        {
            Characters = characters;
            Diseases = diseaseFactory.GetDiseases();

            InfectionRate = 2;
            InfectionPosition = 0;
            Outbreaks = 0;

            ResearchStationsPile = 2;
        }

        public IDictionary<DiseaseColor, Disease> Diseases
        {
            get => _diseases;
            set => Set(ref _diseases, value);
        }

        public int InfectionPosition { get; set; }

        public int InfectionRate
        {
            get => _infectionRate;
            set => Set(ref _infectionRate, value);
        }

        public int Outbreaks
        {
            get => _outbreaks;
            set => Set(ref _outbreaks, value);
        }

        public int ResearchStationsPile
        {
            get => _researchStationPile;
            set => Set(ref _researchStationPile, value);
        }

        public IEnumerable<Character> Characters { get; }
    }
}