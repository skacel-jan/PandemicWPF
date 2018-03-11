﻿using GalaSoft.MvvmLight;
using Pandemic.Decks;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic
{
    public class Board : ObservableObject
    {
        private IDictionary<DiseaseColor, Disease> _diseases;
        private int _infectionRate;
        private int _outbreaks;
        private int _researchStationPile;

        public Board(WorldMap worldMap, IInfectionDeckFactory infectionDeckFactory, PlayerDeck playerDeck, DiseaseFactory diseaseFactory)
        {
            WorldMap = worldMap;
            InfectionDeckFactory = infectionDeckFactory;
            InfectionDeck = InfectionDeckFactory.GetInfectionDeck(WorldMap.Cities.Values.Select(x => x.City));
            PlayerDeck = playerDeck;

            InfectionRate = 2;
            InfectionPosition = 0;
            Outbreaks = 0;

            ResearchStationsPile = 6;

            Diseases = diseaseFactory.GetDiseases();

            InfectionDiscardPile = InfectionDeckFactory.GetEmptyInfectionDeck();
            PlayerDiscardPile = new PlayerDeck();
        }

        public IDictionary<DiseaseColor, Disease> Diseases
        {
            get => _diseases;
            set => Set(ref _diseases, value);
        }

        public IDeck<InfectionCard> InfectionDeck { get; set; }
        public IInfectionDeckFactory InfectionDeckFactory { get; }
        public IDeck<InfectionCard> InfectionDiscardPile { get; private set; }
        public int InfectionPosition { get; private set; }

        public int InfectionRate
        {
            get => _infectionRate;
            private set => Set(ref _infectionRate, value);
        }

        public int Outbreaks
        {
            get => _outbreaks;
            set => Set(ref _outbreaks, value);
        }

        public PlayerDeck PlayerDeck { get; private set; }
        public PlayerDeck PlayerDiscardPile { get; private set; }

        public int ResearchStationsPile
        {
            get => _researchStationPile;
            private set => Set(ref _researchStationPile, value);
        }

        public WorldMap WorldMap { get; private set; }

        public void BuildResearchStation(MapCity mapCity)
        {
            mapCity.HasResearchStation = true;
            ResearchStationsPile--;
        }

        public void BuildResearchStation(MapCity mapCity, PlayerCard card)
        {
            BuildResearchStation(mapCity);
            PlayerDiscardPile.Cards.Add(card);
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

        public void DiscoverCure(DiseaseColor color)
        {
            Diseases[color].IsCured = true;
        }

        public InfectionCard DrawInfectionBottomCard()
        {
            InfectionCard card = InfectionDeck.Cards.Last();
            InfectionDiscardPile.Cards.Add(card);
            InfectionDeck.Cards.Remove(card);
            return card;
        }

        public InfectionCard DrawInfectionCard()
        {
            InfectionCard card = InfectionDeck.Cards.First();
            InfectionDiscardPile.Cards.Add(card);
            InfectionDeck.Cards.Remove(card);
            return card;
        }

        public Card DrawPlayerCard()
        {
            Card card = PlayerDeck.Cards.FirstOrDefault();
            PlayerDeck.Cards.Remove(card);
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
            if (!Diseases[color].IsEradicated)
            {
                int addedInfections = WorldMap.GetCity(city.Name).ChangeInfection(color, 1);
                if (addedInfections > 0)
                {
                    DecreaseCubePile(color, addedInfections);
                }
                return addedInfections == 0;
            }
            else
            {
                return false;
            }
        }

        public void RaiseInfectionPosition()
        {
            InfectionPosition++;
            if (InfectionPosition == 3 || InfectionPosition == 5)
            {
                InfectionRate++;
            }
        }

        public void ShuffleDiscardPile()
        {
            var newDeck = InfectionDeckFactory.GetInfectionDeck(InfectionDiscardPile.Cards);
            newDeck.Shuffle();
            foreach (var infectionCard in InfectionDeck.Cards)
            {
                newDeck.Cards.Add(infectionCard);
            }
            InfectionDeck = newDeck;
            InfectionDiscardPile.Cards.Clear();
        }
    }
}