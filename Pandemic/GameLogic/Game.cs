using GalaSoft.MvvmLight;
using Pandemic.Cards;
using Pandemic.Decks;
using Pandemic.GameLogic;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;

namespace Pandemic
{
    public class Game : ObservableObject
    {
        private int _actions;
        private IGamePhase _gamePhase;

        private GameInfo _info;
        private int _outbreaks;

        private int _researchStationPile;

        public Game(WorldMap worldMap, IDictionary<DiseaseColor, Disease> diseases, CircularCollection<Character> characters,
                    Infection infection, IEnumerable<EventCard> eventCards, PlayerDeck playerDeck, Deck<InfectionCard> infectionDeck)
        {
            WorldMap = worldMap;
            Characters = characters;
            Characters.PropertyChanged += Characters_PropertyChanged;
            Infection = infection;

            PlayerDeck = playerDeck ?? throw new ArgumentNullException(nameof(playerDeck));
            InfectionDeck = infectionDeck ?? throw new ArgumentNullException(nameof(infectionDeck));
            InfectionDiscardPile = new DiscardPile<InfectionCard>();
            PlayerDiscardPile = new DiscardPile<Card>();
            RemovedCards = new DiscardPile<Card>();

            Diseases = diseases;

            EventCards = new List<EventCard>();

            PlayerDeck.AddEventCards(eventCards);
            foreach (var eventCard in eventCards)
            {
                eventCard.EventFinished += (s, e) => EventFinished?.Invoke(s, e);
            }

            //InfectionDeck.Shuffle();
            //PlayerDeck.Shuffle();

            GamePhase = new InitialPhase(this);
            WorldMap.GetCity("Atlanta").ChangeInfection(DiseaseColor.Black, 2);
        }

        public void EndGame()
        {
            GameEnd?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ActionDone;

        public event EventHandler<CardsSelectingEventArgs> CardSelecting;

        public event EventHandler<DiseaseSelectingEventArgs> DiseaseSelecting;

        public event EventHandler EventFinished;

        public event EventHandler CharacterChanged;

        public event EventHandler<CharacterSelectingEventArgs> CharacterSelecting;

        public event EventHandler InfoChanged;

        public event EventHandler<MoveTypeSelectingEventArgs> MoveTypeSelecting;

        public event EventHandler<ShareTypeSelectingEventArgs> ShareTypeSelecting;

        public event EventHandler GameEnd;

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
            set
            {
                _gamePhase?.End();
                Set(ref _gamePhase, value);
                _gamePhase?.Start();
            }
        }

        public CircularCollection<Character> Characters { get; }

        public Infection Infection { get; }

        public Deck<InfectionCard> InfectionDeck { get; set; }

        public DiscardPile<InfectionCard> InfectionDiscardPile { get; private set; }

        public GameInfo Info
        {
            get => _info;
            set
            {
                _info = value;
                InfoChanged?.Invoke(this, EventArgs.Empty);
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
            Diseases[color].IsCured = true;
        }

        public void DoAction(string actionType)
        {
            GamePhase.Action(actionType);
        }

        public bool CheckCubesPile(DiseaseColor color)
        {
            return Diseases[color].Cubes <= 0;
        }

        public void IncreaseCubePile(DiseaseColor color, int cubesCount)
        {
            Diseases[color].Cubes += cubesCount;
        }

        public bool IncreaseInfection(City city, DiseaseColor color)
        {
            if (!Diseases[color].IsEradicated)
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

        public void MoveCharacter(Character character, MapCity city)
        {
            character.CurrentMapCity = city;
        }

        public void SelectCard(IEnumerable<Card> cards, Action<Card> action, string text)
        {
            CardSelecting?.Invoke(this, new CardsSelectingEventArgs(cards, action, text));
        }

        public void SelectCity(IEnumerable<MapCity> cities, Action<MapCity> action, string text)
        {
            WorldMap.SelectCity(cities, action, text);
        }

        public void SelectDisease(IEnumerable<DiseaseColor> diseases, string text, Action<DiseaseColor> action)
        {
            DiseaseSelecting?.Invoke(this, new DiseaseSelectingEventArgs(diseases, action, text));
        }

        public void SelectCharacter(IEnumerable<Character> characters, string text, Action<Character> action)
        {
            CharacterSelecting?.Invoke(this, new CharacterSelectingEventArgs(characters, action, text));
        }

        public void SelectMove(IEnumerable<IMoveAction> possibleCardMoves, string text, Action<IMoveAction> action)
        {
            MoveTypeSelecting?.Invoke(this, new MoveTypeSelectingEventArgs(possibleCardMoves, action, text));
        }

        public void SelectShareType(IEnumerable<ShareType> shareTypes, string text, Action<ShareType> action)
        {
            ShareTypeSelecting?.Invoke(this, new ShareTypeSelectingEventArgs(shareTypes, action, text));
        }

        private void Characters_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CircularCollection<Character>.Current))
            {
                CharacterChanged?.Invoke(this, e);
            }
        }
    }

    public class GameInfo
    {
        public GameInfo(string text, string buttonText, Action action)
        {
            Text = text;
            ButtonText = buttonText;
            Action = action;
        }

        public Action Action { get; }
        public string ButtonText { get; }
        public string Text { get; }
    }
}