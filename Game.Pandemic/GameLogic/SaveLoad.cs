using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Game.Pandemic.GameLogic
{
    public class SavedState
    {
        public int Actions { get; set; }

        public int Difficulty { get; set; }

        public IDictionary<DiseaseColor, Disease> Diseases { get; set; }

        public string GamePhase { get; set; }

        public List<SaveLoad.CharacterSave> Characters { get; set; }

        public int InfectionActual { get; set; }

        public int Outbreaks { get; set; }

        public List<string> PlayerDeck { get; set; }

        public List<string> PlayerDiscardPile { get; set; }

        public List<string> RemovedCards { get; set; }

        public int ResearchStationPile { get; set; }

        public int Turn { get; set; }

        public List<string> InfectionCardsInDiscardPile { get; set; }

        public int InfectionRate { get; set; }

        public int InfectionPosition { get; set; }

        public List<string> InfectionCardsInDeck { get; set; }

        public List<SaveLoad.MapCitySave> Cities { get; set; }
    }

    public class SaveLoad
    {
        private readonly object _fileLock = new object();

        private readonly string _savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saves", "SavedGame.save");

        public async Task<SavedState> Load()
        {
            SavedState state = await Task.Run(() =>
            {
                using var fileStream = new FileStream(_savePath, FileMode.Open);
                using var zipStream = new GZipStream(fileStream, CompressionMode.Decompress);
                using var streamReader = new StreamReader(zipStream);
                using var jsonReader = new JsonTextReader(streamReader);
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<SavedState>(jsonReader);
            });

            return state;
        }

        public async Task Save(Game game)
        {
            await Task.Run(() =>
            {
                var savedState = new SavedState()
                {
                    Actions = game.Actions,
                    Characters = game.Characters.Select(c => new CharacterSave()
                    {
                        Role = c.Role,
                        MapCity = c.CurrentMapCity.City.Name,
                        IsActive = c.IsActive,
                        Cards = c.Cards.Select(x => x.Name).ToList()
                    }).ToList(),
                    Difficulty = game.Difficulty,
                    Diseases = game.Diseases,
                    GamePhase = game.GamePhase.ToString(),
                    InfectionActual = game.Infection.Actual,
                    InfectionRate = game.Infection.Rate,
                    InfectionPosition = game.Infection.Position,
                    InfectionCardsInDeck = game.Infection.Deck.Cards.Select(c => c.Name).ToList(),
                    InfectionCardsInDiscardPile = game.Infection.DiscardPile.Cards.Select(c => c.Name).ToList(),
                    Outbreaks = game.Outbreaks,
                    PlayerDeck = game.PlayerDeck.Cards.Select(c => c.Name).ToList(),
                    PlayerDiscardPile = game.PlayerDiscardPile.Cards.Select(c => c.Name).ToList(),
                    RemovedCards = game.RemovedCards.Cards.Select(c => c.Name).ToList(),
                    ResearchStationPile = game.ResearchStationsPile,
                    Turn = game.Turn,
                    Cities = game.WorldMap.Cities.Select(c => new MapCitySave()
                    {
                        HasResearchStation = c.HasResearchStation,
                        Name = c.City.Name,
                        Infection = c.Infections.Select(i => (i.Key, i.Value)).ToList()
                    }).ToList()
                };

                Directory.CreateDirectory(Path.GetDirectoryName(_savePath));

                lock (_fileLock)
                {
                    using (var fileStream = new FileStream(_savePath, FileMode.Create))
                    using (var zipStream = new GZipStream(fileStream, CompressionLevel.Fastest))
                    using (var streamWriter = new StreamWriter(zipStream))
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        JsonSerializer serializer = new JsonSerializer
                        {
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                            Formatting = Formatting.Indented,
                            TypeNameHandling = TypeNameHandling.All
                        };

                        serializer.Serialize(jsonWriter, savedState);


                    };
                }
            });
        }

        public class MapCitySave
        {
            public string Name { get; set; }

            public List<(DiseaseColor Color, int Value)> Infection { get; set; }

            public bool HasResearchStation { get; set; }
        }

        public class CharacterSave
        {
            public string Role { get; set; }

            public List<string> Cards { get; set; }

            public string MapCity { get; set; }

            public bool IsActive { get; set; }
        }
    }
}