using GalaSoft.MvvmLight;
using Pandemic.Decks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic
{
    public class Board : ObservableObject
    {
        private int _infectionRate;
        private int _outbreaks;
        private int _researchStationPile;

        public Board(WorldMap map, InfectionDeck infectionDeck, PlayerDeck playerDeck, IDictionary<DiseaseColor, Disease> diseases)
        {
            WorldMap = map;
            InfectionDeck = infectionDeck;
            PlayerDeck = playerDeck;

            InfectionRate = 2;
            InfectionPosition = 0;
            Outbreaks = 0;

            ResearchStationsPile = 6;

            Diseases = diseases;

            InfectionDiscardPile = new InfectionDeck(new List<InfectionCard>());
            PlayerDiscardPile = new PlayerDeck(new List<PlayerCard>());
        }

        public InfectionDeck InfectionDeck { get; set; }
        public InfectionDeck InfectionDiscardPile { get; private set; }
        public int InfectionPosition { get; private set; }

        public int InfectionRate
        {
            get => _infectionRate;
            private set => Set(ref _infectionRate, value);
        }

        public int Outbreaks
        {
            get => _outbreaks;
            private set => Set(ref _outbreaks, value);
        }

        public PlayerDeck PlayerDeck { get; private set; }
        public PlayerDeck PlayerDiscardPile { get; private set; }


        public int ResearchStationsPile
        {
            get => _researchStationPile;
            private set => Set(ref _researchStationPile, value);
        }

        public WorldMap WorldMap { get; private set; }

        private IDictionary<DiseaseColor, Disease> _diseases;
        public IDictionary<DiseaseColor, Disease> Diseases
        {
            get => _diseases;
            set => Set(ref _diseases, value);
        }

        public void BuildResearchStation(MapCity mapCity)
        {
            mapCity.HasResearchStation = true;
            ResearchStationsPile--;
        }

        public void BuildResearchStation(MapCity mapCity, PlayerCard card)
        {
            BuildResearchStation(mapCity);
            PlayerDiscardPile.Add(card);
        }

        public void DecreaseCubePile(DiseaseColor color, int cubesCount)
        {
            Diseases[color].Cubes -= cubesCount;
        }

        public void DestroyResearchStation(MapCity mapCity)
        {
            mapCity.HasResearchStation = false;
            ResearchStationsPile++;
        }

        public InfectionCard DrawInfectionBottomCard()
        {
            InfectionCard card = InfectionDeck.Last();
            InfectionDiscardPile.Add(card);
            InfectionDeck.Remove(card);
            return card;
        }

        public InfectionCard DrawInfectionCard()
        {
            InfectionCard card = InfectionDeck.First();
            InfectionDiscardPile.Add(card);
            InfectionDeck.Remove(card);
            return card;
        }

        public Card DrawPlayerCard()
        {
            Card card = PlayerDeck.FirstOrDefault();
            PlayerDeck.Remove(card);
            return card;
        }

        public bool CheckCubesPile(DiseaseColor color)
        {
            return Diseases[color].Cubes <= 0;
        }

        public void IncreaseCubePile(DiseaseColor color, int cubesCount)
        {
            Diseases[color].Cubes += cubesCount;
        }

        public bool RaiseInfection(City city, DiseaseColor color)
        {
            int addedInfections = WorldMap.GetCity(city.Name).ChangeInfection(color, 1);
            if (addedInfections == 0)
            {
                Outbreaks++;                
            }
            else
            {
                DecreaseCubePile(color, 1);
            }

            return addedInfections == 0;
        }

        public void RaiseInfectionPosition()
        {
            InfectionPosition++;
            if (InfectionPosition == 3 || InfectionPosition == 5)
            {
                InfectionRate++;
            }
        }

        internal void ShuffleDiscardPile()
        {
            var newDeck = new InfectionDeck(InfectionDiscardPile);
            newDeck.Shuffle();
            foreach (var infectionCard in InfectionDeck)
            {
                newDeck.Add(infectionCard);
            }
            InfectionDeck = newDeck;
            InfectionDiscardPile.Clear();
        }

        internal void DiscoverCure(DiseaseColor color)
        {
            Diseases[color].IsCured = true;
        }
    }
}