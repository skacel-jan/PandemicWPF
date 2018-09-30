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
        private int _actions;
        private IGamePhase _gamePhase;

        private GameInfo _info;
        private Stack<GameInfo> _gameInfos;
        private int _outbreaks;

        private int _researchStationPile;

        public int Difficulty { get; }

        public SelectionService SelectionService { get; }

        internal Game(WorldMap worldMap, IDictionary<DiseaseColor, Disease> diseases, GameSettings gameSettings,
            PlayerDeck playerDeck, SelectionService selectionService)
        {
            WorldMap = worldMap;
            Characters = gameSettings.GetCharacters(worldMap.GetCity(City.Atlanta));
            Infection = new Infection(new Deck<InfectionCard>(WorldMap.Cities.Select(c => new InfectionCard(c.City))));

            PlayerDeck = playerDeck ?? throw new ArgumentNullException(nameof(playerDeck));

            SelectionService = selectionService ?? throw new ArgumentNullException(nameof(selectionService));
            Difficulty = gameSettings.Difficulty;

            PlayerDiscardPile = new DiscardPile<Card>();
            RemovedCards = new DiscardPile<Card>();

            Diseases = diseases;

            EventCards = new List<EventCard>();

            _gameInfos = new Stack<GameInfo>();            

            //InfectionDeck.Shuffle();
            //PlayerDeck.Shuffle();

            Diseases[DiseaseColor.Blue].Status = Disease.State.Cured;

            ChangeGamePhase(new InitialPhase(this));
            WorldMap.GetCity("Atlanta").ChangeInfection(DiseaseColor.Black, 2);

        }

        internal void EndGame()
        {
            GameEnded?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ActionDone;

        public event EventHandler<DiseaseSelectingEventArgs> DiseaseSelecting;

        public event EventHandler<MoveTypeSelectingEventArgs> MoveTypeSelecting;

        public event EventHandler<ShareTypeSelectingEventArgs> ShareTypeSelecting;

        public event EventHandler<GamePhaseChangedEventArgs> GamePhaseChanged;

        public event EventHandler GameEnded;

        public int Actions
        {
            get => _actions;
            set
            {
                if (Set(ref _actions, value))
                {
                    ActionDone?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public Character CurrentCharacter { get => Characters.Current; }

        public IDictionary<DiseaseColor, Disease> Diseases { get; }

        public IList<EventCard> EventCards { get; }

        public IGamePhase GamePhase
        {
            get => _gamePhase;
            private set
            {
                Set(ref _gamePhase, value);
            }
        }

        public bool ChangeGamePhase(IGamePhase gamePhase)
        {
            var eventArgs = new GamePhaseChangedEventArgs(GamePhase, gamePhase);

            GamePhase?.End();
            GamePhase = gamePhase;
            GamePhase?.Start();

            GamePhaseChanged?.Invoke(this, eventArgs);

            return true;
        }

        public CircularCollection<Character> Characters { get; }

        public Infection Infection { get; }

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

        public DiscardPile<Card> PlayerDiscardPile { get; private set; }

        public DiscardPile<Card> RemovedCards { get; }

        public int ResearchStationsPile
        {
            get => _researchStationPile;
            set => Set(ref _researchStationPile, value);
        }

        public WorldMap WorldMap { get; private set; }
        public int Turn { get; internal set; }

        public void AddCardToPlayerDiscardPile(Card card)
        {
            PlayerDiscardPile.AddCard(card);

            if (card is EventCard eventCard)
            {
                EventCards.Remove(eventCard);
            }
        }

        public void DecreaseCubePile(DiseaseColor color, int cubesCount)
        {
            Diseases[color].Cubes -= cubesCount;
        }

        public void DiscoverCure(DiseaseColor color)
        {
            if (Diseases[color].Cubes == Disease.STARTING_CUBES_COUNT)
            {
                Diseases[color].Status = Disease.State.Eradicated;
            }
            else
            {
                Diseases[color].Status = Disease.State.Cured;
            }
        }

        public void DoAction(IGameAction action)
        {
            GamePhase.Action(action);
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

        public void SelectDisease(IEnumerable<DiseaseColor> diseases, string text, Action<DiseaseColor> action)
        {
            DiseaseSelecting?.Invoke(this, new DiseaseSelectingEventArgs(diseases, action, text));
        }

        public void SelectMove(IEnumerable<IMoveAction> possibleCardMoves, string text, Action<IMoveAction> action)
        {
            MoveTypeSelecting?.Invoke(this, new MoveTypeSelectingEventArgs(possibleCardMoves, action, text));
        }

        public void SelectShareType(IEnumerable<ShareType> shareTypes, string text, Action<ShareType> action)
        {
            ShareTypeSelecting?.Invoke(this, new ShareTypeSelectingEventArgs(shareTypes, action, text));
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