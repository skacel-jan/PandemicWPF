using GalaSoft.MvvmLight;
using Pandemic.Cards;
using Pandemic.Decks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic
{
    public class Board : ObservableObject
    {
        public Board(WorldMap worldMap, PlayerDeck playerDeck, IGameData gameData)
        {
            WorldMap = worldMap;
            InfectionDeck = new Deck<InfectionCard>(WorldMap.Cities.Values.Select(x => new InfectionCard(x.City)));
            PlayerDeck = playerDeck;

            GameData = gameData;

            EventCards = new List<EventCard>();

            InfectionDiscardPile = new DiscardPile<InfectionCard>();
            PlayerDiscardPile = new DiscardPile<Card>();

            //InfectionDeck.Shuffle();
            //PlayerDeck.Shuffle();

            BuildResearchStation(WorldMap.GetCity(City.Atlanta));

            InitialInfection();
        }

        public IList<EventCard> EventCards { get; }
        public IGameData GameData { get; }
        public Deck<InfectionCard> InfectionDeck { get; set; }
        public DiscardPile<InfectionCard> InfectionDiscardPile { get; private set; }

        public PlayerDeck PlayerDeck { get; private set; }
        public DiscardPile<Card> PlayerDiscardPile { get; private set; }

        public WorldMap WorldMap { get; private set; }

        public void AddCardToPlayerDiscardPile(Card card)
        {
            PlayerDiscardPile.AddCard(card);

            if (card is EventCard eventCard)
            {
                EventCards.Remove(eventCard);
            }
        }

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
            InfectionCard card = InfectionDeck.DrawBottom();
            InfectionDiscardPile.Cards.Add(card);
            return card;
        }

        public InfectionCard DrawInfectionCard()
        {
            InfectionCard card = InfectionDeck.DrawTop();
            InfectionDiscardPile.Cards.Add(card);
            return card;
        }

        public Card DrawPlayerCard()
        {
            Card card = PlayerDeck.Cards.FirstOrDefault();
            PlayerDeck.Cards.Remove(card);

            if (card is EventCard eventCard)
            {
                EventCards.Add(eventCard);
            }

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

        public void ShuffleInfectionDiscardPileBack()
        {
            var newDeck = new Deck<InfectionCard>(InfectionDiscardPile.Cards);
            newDeck.Shuffle();
            newDeck.AddCards(InfectionDeck.Cards);
            InfectionDeck = newDeck;
            InfectionDiscardPile.Cards.Clear();
        }

        private void InitialInfection()
        {
            for (int i = 3; i > 0; i--)
            {
                foreach (var x in Enumerable.Range(0, 3))
                {
                    var infectionCard = DrawInfectionCard();
                    int changeInfections = WorldMap.GetCity(infectionCard.City.Name).ChangeInfection(infectionCard.City.Color, i);
                    DecreaseCubePile(infectionCard.City.Color, changeInfections);
                }
            }
        }
    }
}