using GalaSoft.MvvmLight;
using Pandemic.GameLogic;
using System.Collections.Generic;

namespace Pandemic
{
    public class GameData : ObservableObject
    {
        private IDictionary<DiseaseColor, Disease> _diseases;
        private int _infectionRate;
        private int _outbreaks;
        private int _researchStationPile;

        public GameData(DiseaseFactory diseaseFactory, IEnumerable<Character> characters, DecksService decks)
        {
            Characters = characters;
            DecksService = decks ?? throw new System.ArgumentNullException(nameof(decks));
            Diseases = diseaseFactory.GetDiseases();

            InfectionRate = 2;
            InfectionPosition = 0;
            Outbreaks = 0;

            ResearchStationsPile = 2;
        }

        public DecksService DecksService { get; }

        public IDictionary<DiseaseColor, Disease> Diseases
        {
            get => _diseases;
            set => Set(ref _diseases, value);
        }

        public IEnumerable<Character> Characters { get; }
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
    }
}