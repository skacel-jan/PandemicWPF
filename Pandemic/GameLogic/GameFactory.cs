using Pandemic.Cards;
using Pandemic.Characters;
using Pandemic.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic
{
    public class GameFactory
    {
        public GameFactory(DiseaseFactory diseaseFactory, IWorldMapFactory worldMapFactory, EventCardFactory eventCardFactory)
        {
            DiseaseFactory = diseaseFactory ?? throw new ArgumentNullException(nameof(diseaseFactory));
            WorldMapFactory = worldMapFactory ?? throw new ArgumentNullException(nameof(worldMapFactory));
            EventCardFactory = eventCardFactory ?? throw new ArgumentNullException(nameof(eventCardFactory));

            CharacterFactory = new CharacterFactory();
        }

        public DiseaseFactory DiseaseFactory { get; }
        public IWorldMapFactory WorldMapFactory { get; }
        public EventCardFactory EventCardFactory { get; }
        public CharacterFactory CharacterFactory { get; }

        public Game CreateGame()
        {

            var diseases = DiseaseFactory.GetDiseases();
            var worldMap = WorldMapFactory.CreateWorldMap(diseases);
            var infectionCards = worldMap.Cities.Values.Select(c => new InfectionCard(c.City));
            CharacterFactory.StartingCity = worldMap.GetCity(City.Atlanta);
            var characters = CharacterFactory.GetCharacters(new string[] { OperationsExpert.OPERATIONS_EXPERT, Medic.MEDIC, Researcher.RESEARCHER });

            return new Game(worldMap, diseases, new CircularCollection<Character>(characters), new Infection(),
                EventCardFactory.GetEventCards(), new PlayerDeck(worldMap.Cities.Values.Select(c => c.City)), new Deck<InfectionCard>(infectionCards));
        }
    }
}
