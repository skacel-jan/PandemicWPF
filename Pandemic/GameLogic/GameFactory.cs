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

            var selectionService = new SelectionService(worldMap);
            var events = new EventCardFactory(selectionService).GetEventCards();

            PlayerDeck playerDeck = new PlayerDeck(worldMap.Cities.Select(c => new PlayerCard(c.City)));

            foreach (var card in events)
            {
                playerDeck.AddCard(card);
            }

            //playerDeck.AddCards(EventCardFactory.GetEventCards());

            return new Game(worldMap, diseases, GameSettings, playerDeck, selectionService);
        }
    }
}