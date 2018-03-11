using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Pandemic.Decks;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic
{
    public class Board : ObservableObject
    {
        public Board(WorldMap worldMap, IInfectionDeckFactory infectionDeckFactory, PlayerDeck playerDeck, IGameData gameData)
        {
            WorldMap = worldMap;
            InfectionDeckFactory = infectionDeckFactory;
            InfectionDeck = InfectionDeckFactory.GetInfectionDeck(WorldMap.Cities.Values.Select(x => x.City));
            PlayerDeck = playerDeck;

            //PlayerDeck = new PlayerDeck(playerDeck.Cards.Take(11).Select(x => (x as PlayerCard).City));

            GameData = gameData;

            InfectionDiscardPile = InfectionDeckFactory.GetEmptyInfectionDeck();
            PlayerDiscardPile = new PlayerDeck(new List<City>());
        }

        public IGameData GameData { get; }
        public Deck<InfectionCard> InfectionDeck { get; set; }
        public IInfectionDeckFactory InfectionDeckFactory { get; }
        public Deck<InfectionCard> InfectionDiscardPile { get; private set; }

        public PlayerDeck PlayerDeck { get; private set; }
        public PlayerDeck PlayerDiscardPile { get; private set; }

        public WorldMap WorldMap { get; private set; }

        public void BuildResearchStation(MapCity mapCity)
        {
            mapCity.HasResearchStation = true;
            GameData.ResearchStationsPile--;
        }

        public void DecreaseCubePile(DiseaseColor color, int cubesCount)
        {
            GameData.Diseases[color].Cubes -= cubesCount;
        }

        public void DiscoverCure(DiseaseColor color)
        {
            GameData.Diseases[color].IsCured = true;
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
            return GameData.Diseases[color].Cubes <= 0;
        }

        public void IncreaseCubePile(DiseaseColor color, int cubesCount)
        {
            GameData.Diseases[color].Cubes += cubesCount;
        }

        public bool RaiseInfection(City city, DiseaseColor color)
        {           
            if (!GameData.Diseases[color].IsEradicated)
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
            GameData.InfectionPosition++;
            if (GameData.InfectionPosition == 3 || GameData.InfectionPosition == 5)
            {
                GameData.InfectionRate++;
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