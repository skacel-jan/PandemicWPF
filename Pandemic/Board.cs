using GalaSoft.MvvmLight;
using Pandemic.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class Board : ObservableObject
    {
        public int InfectionRate { get; private set; }
        public int InfectionPosition { get; private set; }
        public int Outbreaks { get; private set; }
        public InfectionDeck InfectionDeck { get; private set; }
        public InfectionDeck InfectionDiscardPile { get; private set; }
        public PlayerDeck PlayerDeck { get; private set; }
        public PlayerDeck PlayerDiscardPile { get; private set; }
        public WorldMap WorldMap { get; private set; }

        private int _blueCubesPile;
        public int BlueCubesPile { get => _blueCubesPile; private set { Set(ref _blueCubesPile, value); } }

        private int _blackCubesPile;
        public int BlackCubesPile { get => _blackCubesPile; private set { Set(ref _blackCubesPile, value); } }

        private int _redCubesPile;
        public int YellowCubesPile { get => _redCubesPile; private set { Set(ref _redCubesPile, value); } }

        private int _yellowCubesPile;
        public int RedCubesPile { get => _yellowCubesPile; private set { Set(ref _yellowCubesPile, value); } }

        private int _researchStationPile;
        public int ResearchStationsPile { get => _researchStationPile; private set { Set(ref _researchStationPile, value); } }

        public Board(WorldMap map, InfectionDeck infectionDeck, PlayerDeck playerDeck)
        {
            WorldMap = map;
            InfectionDeck = infectionDeck;
            PlayerDeck = playerDeck;

            Outbreaks = 0;
            InfectionRate = 2;
            InfectionPosition = 0;

            InfectionDiscardPile = new InfectionDeck(new List<InfectionCard>());
            PlayerDiscardPile = new PlayerDeck(new List<PlayerCard>());

            BlueCubesPile = 24;
            BlackCubesPile = 24;
            YellowCubesPile = 24;
            RedCubesPile = 24;

            ResearchStationsPile = 6;
        }

        public void RaiseInfectionPosition()
        {
            InfectionPosition++;
            if (InfectionPosition == 3 || InfectionPosition == 5)
            {
                InfectionRate++;
            }
        }

        public Card DrawPlayerCard()
        {
            Card card = PlayerDeck.FirstOrDefault();
            PlayerDeck.Remove(card);
            return card;
        }

        public InfectionCard DrawInfectionCard()
        {
            InfectionCard card = InfectionDeck.First();
            InfectionDiscardPile.Add(card);
            InfectionDeck.Remove(card);
            return card;
        }

        public InfectionCard DrawInfectionBottomCard()
        {
            InfectionCard card = InfectionDeck.Last();
            InfectionDiscardPile.Add(card);
            InfectionDeck.Remove(card);
            return card;
        }

        public bool RaiseInfection(City city)
        {
            switch (city.Color)
            {
                case DiseaseColor.Black:
                    BlackCubesPile--;
                    break;
                case DiseaseColor.Blue:
                    BlueCubesPile--;
                    break;
                case DiseaseColor.Red:
                    RedCubesPile--;
                    break;
                case DiseaseColor.Yellow:
                    YellowCubesPile--;
                    break;
            }
            return WorldMap.GetCity(city.Name).RaiseInfection(city.Color);
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

        public void DestroyResearchStation(MapCity mapCity)
        {
            mapCity.HasResearchStation = false;
            ResearchStationsPile++;
        }

        public bool CheckCubesPile(DiseaseColor color)
        {
            switch (color)
            {
                case DiseaseColor.Black:
                    return BlackCubesPile == 0;
                case DiseaseColor.Blue:
                    return BlueCubesPile == 0;
                case DiseaseColor.Red:
                    return RedCubesPile == 0;
                case DiseaseColor.Yellow:
                    return YellowCubesPile == 0;
                default:
                    return false;
            }
        }

        public void IncreaseCubePile(DiseaseColor color, int cubesCount)
        {
            switch (color)
            {
                case DiseaseColor.Black:
                    BlackCubesPile += cubesCount;
                    break;
                case DiseaseColor.Blue:
                    BlueCubesPile += cubesCount;
                    break;
                case DiseaseColor.Red:
                    RedCubesPile += cubesCount;
                    break;
                case DiseaseColor.Yellow:
                    YellowCubesPile += cubesCount;
                    break;
            }
        }

        public void DecreaseCubePile(DiseaseColor color, int cubesCount)
        {
            switch (color)
            {
                case DiseaseColor.Black:
                    BlackCubesPile -= cubesCount;
                    break;
                case DiseaseColor.Blue:
                    BlueCubesPile -= cubesCount;
                    break;
                case DiseaseColor.Red:
                    RedCubesPile -= cubesCount;
                    break;
                case DiseaseColor.Yellow:
                    YellowCubesPile -= cubesCount;
                    break;
            }
        }
    }
}
