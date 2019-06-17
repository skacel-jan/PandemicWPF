using GalaSoft.MvvmLight;
using Pandemic.Cards;
using Pandemic.Decks;
using Pandemic.GameLogic;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandemic
{
    public class Game : ObservableObject
    {
        private readonly SaveLoad _saveLoad;
        private readonly bool _autoSave;
        private int _actions;
        private IGamePhase _gamePhase;
        private GameInfo _info;
        private readonly Stack<GameInfo> _gameInfos;
        private int _outbreaks;
        private int _researchStationPile = 1;        

        internal Game(WorldMap worldMap, IDictionary<DiseaseColor, Disease> diseases, GameSettings gameSettings,
            PlayerDeck playerDeck, SelectionService selectionService)
        {
            WorldMap = worldMap;
            Characters = new CircularCollection<Character>(gameSettings.GetCharacters(worldMap[City.Atlanta], this));
            Infection = new Infection(new Deck<InfectionCard>(WorldMap.Cities.Select(c => new InfectionCard(c.City))));

            PlayerDeck = playerDeck ?? throw new ArgumentNullException(nameof(playerDeck));

            AllPlayerCards = PlayerDeck.Cards.ToDictionary(c => c.Name, c => c);
            AllInfectionCards = Infection.Deck.Cards.ToDictionary(c => c.Name, c => c);

            SelectionService = selectionService ?? throw new ArgumentNullException(nameof(selectionService));

            PlayerDiscardPile = new DiscardPile<PlayerCard>();
            RemovedCards = new DiscardPile<Card>();

            Diseases = diseases;
            GameSettings = gameSettings;

            _gameInfos = new Stack<GameInfo>();

            //InfectionDeck.Shuffle();
            //PlayerDeck.Shuffle();

            Diseases[DiseaseColor.Blue].Status = Disease.State.Cured;

            ChangeGamePhase(new InitialPhase(this));
            WorldMap["Atlanta"].ChangeInfection(DiseaseColor.Black, 2);

            _saveLoad = new SaveLoad();

            _autoSave = true;
        }

        internal void ResolveEffect(IEffect effect)
        {
            effect.Execute();
        }

        internal void Continue()
        {
            GamePhase.Continue();
        }

        public event EventHandler ActionDone;

        public event EventHandler<GamePhaseChangedEventArgs> GamePhaseChanged;

        public event EventHandler GameEnded;

        public int Difficulty => GameSettings.Difficulty;

        public SelectionService SelectionService { get; }

        public int Actions
        {
            get => _actions;
            set
            {
                if (Set(ref _actions, value))
                {
                    OnActionDone(EventArgs.Empty);
                }
            }
        }

        protected virtual async void OnActionDone(EventArgs e)
        {
            ActionDone?.Invoke(this, e);
            if (_autoSave)
            {
                await Save().ConfigureAwait(false);
            }
        }

        public Character CurrentCharacter { get => Characters.Current; }

        public IDictionary<DiseaseColor, Disease> Diseases { get; private set; }
        public GameSettings GameSettings { get; }
        public IEnumerable<EventCard> EventCards => AllPlayerCards.Values.OfType<EventCard>().Where(x => x.Character != null);

        public IGamePhase GamePhase
        {
            get => _gamePhase;
            private set
            {
                Set(ref _gamePhase, value);
            }
        }

        public CircularCollection<Character> Characters { get; private set; }

        public Infection Infection { get; private set; }

        public GameInfo Info
        {
            get => _info;
            set
            {
                var newInfo = value;
                if (newInfo == null && _gameInfos.Count > 0)
                {
                    newInfo = _gameInfos.Pop();
                }
                Set(ref _info, newInfo);
            }
        }

        public int Outbreaks
        {
            get => _outbreaks;
            set => Set(ref _outbreaks, value);
        }

        public PlayerDeck PlayerDeck { get; private set; }

        public Dictionary<string, PlayerCard> AllPlayerCards { get; }
        public Dictionary<string, InfectionCard> AllInfectionCards { get; }
        public DiscardPile<PlayerCard> PlayerDiscardPile { get; private set; }

        public DiscardPile<Card> RemovedCards { get; }

        public int ResearchStationsPile
        {
            get => _researchStationPile;
            set => Set(ref _researchStationPile, value);
        }

        public WorldMap WorldMap { get; private set; }

        public int Turn { get; internal set; }

        public bool ChangeGamePhase(IGamePhase gamePhase)
        {
            var eventArgs = new GamePhaseChangedEventArgs(GamePhase, gamePhase);

            GamePhase?.End();
            GamePhase = gamePhase;
            GamePhase?.Start();

            GamePhaseChanged?.Invoke(this, eventArgs);

            return true;
        }

        public void AddCardToPlayerDiscardPile(PlayerCard card)
        {
            PlayerDiscardPile.AddCard(card);
        }

        internal void ResolveSelection(Selection selection)
        {
            selection.Execute(SelectionService);
        }

        public void DecreaseCubePile(DiseaseColor color, int cubesCount)
        {
            Diseases[color].Cubes -= cubesCount;
        }

        public bool IsCubePileEmpty(DiseaseColor color)
        {
            return Diseases[color].Cubes <= 0;
        }

        public void IncreaseCubePile(DiseaseColor color, int cubesCount)
        {
            Diseases[color].Cubes += cubesCount;
        }

        public bool IncreaseInfection(City city, DiseaseColor color)
        {
            if (Diseases[color].Status < Disease.State.Eradicated)
            {
                int addedInfections = WorldMap[city.Name].ChangeInfection(color, 1);
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

        public void SetInfo(string text)
        {
            if (Info != null)
            {
                _gameInfos.Push(Info);
            }
            Info = new GameInfo(text);
        }

        public void SetInfo(string text, string actionText, Action action)
        {
            if (Info != null)
            {
                _gameInfos.Push(Info);
            }
            Info = new GameInfo(text, actionText, action);
        }

        public async Task Save()
        {
            await _saveLoad.Save(this).ConfigureAwait(false);
        }

        public async Task Load()
        {
            SavedState loadedState = await _saveLoad.Load();
            SetLoadedState(loadedState);
        }

        internal void EndGame()
        {
            GameEnded?.Invoke(this, EventArgs.Empty);
        }

        private void SetLoadedState(SavedState savedState)
        {
            Actions = savedState.Actions;

            foreach (var citySave in savedState.Cities)
            {
                MapCity city = WorldMap[citySave.Name];
                city.HasResearchStation = citySave.HasResearchStation;
                city.Characters.Clear();
                foreach (var (Color, Value) in citySave.Infection)
                {
                    city.Infections[Color] = Value;
                }
            }

            Characters = new CircularCollection<Character>(
                savedState.Characters.Select(c =>
                {
                    var character = GameSettings.CharacterFactory.GetCharacter(c.Role, WorldMap[c.MapCity], this);
                    WorldMap[c.MapCity].Characters.Add(character);

                    foreach (var card in c.Cards)
                    {
                        character.AddCard(AllPlayerCards[card]);
                    }

                    return character;
                }));
            Characters.First().IsActive = true;
            GameSettings.Difficulty = savedState.Difficulty;
            Diseases = savedState.Diseases;
            Outbreaks = savedState.Outbreaks;
            Turn = savedState.Turn;

            PlayerDeck = new PlayerDeck(savedState.PlayerDeck.Select(c => c == "Epidemic" ? new EpidemicCard() : AllPlayerCards[c]));

            PlayerDiscardPile = new DiscardPile<PlayerCard>(savedState.PlayerDeck.Select(c => c == "Epidemic" ? new EpidemicCard() : AllPlayerCards[c]));

            var infectionDeck = new Deck<InfectionCard>(savedState.InfectionCardsInDeck.Select(c => AllInfectionCards[c]));
            var infectionDiscardPile = new DiscardPile<InfectionCard>(savedState.InfectionCardsInDiscardPile.Select(c => AllInfectionCards[c]));

            Infection = new Infection(infectionDeck, infectionDiscardPile)
            {
                Actual = savedState.InfectionActual,
                Position = savedState.InfectionPosition,
                Rate = savedState.InfectionRate
            };
        }

        internal void ResolveAction(IGameAction action)
        {
            action.Execute();
        }
    }

    public class GameInfo
    {
        public GameInfo(string text)
        {
            Text = text;
        }

        public GameInfo(string text, string buttonText, Action action)
            : this(text)
        {
            ButtonText = buttonText;
            Action = action;
        }

        public Action Action { get; }
        public string ButtonText { get; }
        public string Text { get; }
    }
}