using Pandemic.Cards;
using Pandemic.Decks;
using System;
using System.Linq;

namespace Pandemic.GameLogic
{
    public class GameFactory
    {
        public GameFactory(IWorldMapFactory worldMapFactory, GameSettings gameSettings)
        {
            DiseaseFactory = new DiseaseFactory();
            WorldMapFactory = worldMapFactory ?? throw new ArgumentNullException(nameof(worldMapFactory));
            GameSettings = gameSettings;
        }

        public DiseaseFactory DiseaseFactory { get; }
        public IWorldMapFactory WorldMapFactory { get; }
        public GameSettings GameSettings { get; }

        public Game CreateGame()
        {
            var diseases = DiseaseFactory.GetDiseases();
            var worldMap = WorldMapFactory.CreateWorldMap(diseases);

            var events = new EventCardFactory().GetEventCards();

            PlayerDeck playerDeck = new PlayerDeck(worldMap.Cities.Select(c => new CityCard(c.City)));

            foreach (var card in events)
            {
                playerDeck.AddCard(card);
            }

            return new Game(worldMap, diseases, GameSettings, playerDeck, new SelectionService(worldMap));
        }
    }
}