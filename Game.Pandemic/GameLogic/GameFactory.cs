using System;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Decks;
using Game.Pandemic.GameLogic.Services;

namespace Game.Pandemic.GameLogic
{
    public class GameFactory
    {
        public DeckFactory DeckFactory { get; }

        public DiseaseFactory DiseaseFactory { get; }

        public GameSettings GameSettings { get; }

        public IWorldMapFactory WorldMapFactory { get; }

        public GameFactory(IWorldMapFactory worldMapFactory, GameSettings gameSettings, DeckFactory deckFactory)
        {
            DiseaseFactory = new DiseaseFactory();
            WorldMapFactory = worldMapFactory ?? throw new ArgumentNullException(nameof(worldMapFactory));
            GameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            DeckFactory = deckFactory ?? throw new ArgumentNullException(nameof(deckFactory));
        }
        public Game CreateGame()
        {
            var diseases = DiseaseFactory.GetDiseases();
            var worldMap = WorldMapFactory.CreateWorldMap(diseases);
            var playerDeck = DeckFactory.CreatePlayerDeck(worldMap);            

            return new Game(worldMap, diseases, GameSettings, playerDeck, new SelectionService(worldMap));
        }
    }
}