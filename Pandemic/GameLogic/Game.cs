using GalaSoft.MvvmLight;
using Pandemic.Cards;
using Pandemic.Decks;
using Pandemic.GameLogic;
using System;
using System.Collections.Generic;

namespace Pandemic
{
    public class Game : ObservableObject
    {
        private IGamePhase _gamePhase;

        private GameInfo _info;
        private int _outbreaks;

        private int _researchStationPile;
        private int _actions;

        public Game(WorldMap worldMap, DiseaseFactory diseaseFactory, CircularCollection<Character> characters,
                    Infection infection, IEnumerable<EventCard> eventCards, PlayerDeck playerDeck, Deck<InfectionCard> infectionDeck)
        {
            WorldMap = worldMap;
            Characters = characters;
            Characters.PropertyChanged += Characters_PropertyChanged;
            Infection = infection;

            PlayerDeck = playerDeck ?? throw new ArgumentNullException(nameof(playerDeck));
            InfectionDeck = infectionDeck ?? throw new ArgumentNullException(nameof(infectionDeck));
            CitySelectionService = new CitySelectionService(WorldMap.Cities.Values);
            InfectionDiscardPile = new DiscardPile<InfectionCard>();
            PlayerDiscardPile = new DiscardPile<Card>();
            RemovedCards = new DiscardPile<Card>();

            Diseases = diseaseFactory.GetDiseases();

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

        public event EventHandler ActionDone;

        public event EventHandler<CardsSelectingEventArgs> CardSelecting;

        public event EventHandler<DiseaseSelectingEventArgs> DiseaseSelecting;

        public event EventHandler EventFinished;

        public event EventHandler CharacterChanged;

        public event EventHandler<CharacterSelectingEventArgs> CharacterSelecting;

        public event EventHandler InfoChanged;

        public event EventHandler<ShareTypeSelectingEventArgs> ShareTypeSelecting;

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

        public CitySelectionService CitySelectionService { get; }

        public Character CurrentCharacter { get; set; }

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

        public bool RaiseInfection(City city, DiseaseColor color)
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

        public void SelectCard(IEnumerable<Card> cards, Action<Card> action, string text)
        {
            CardSelecting?.Invoke(this, new CardsSelectingEventArgs(cards, action, text));
        }

        public void SelectCity(IEnumerable<MapCity> cities, Action<MapCity> action, string text)
        {
            CitySelectionService.SelectCity(cities, action, text);
        }

        public void SelectDisease(IEnumerable<DiseaseColor> diseases, Action<DiseaseColor> action, string text)
        {
            DiseaseSelecting?.Invoke(this, new DiseaseSelectingEventArgs(diseases, action, text));
        }

        public void SelectCharacter(IEnumerable<Character> characters, Action<Character> action, string text)
        {
            CharacterSelecting?.Invoke(this, new CharacterSelectingEventArgs(characters, action, text));
        }

        internal void SelectShareType(IEnumerable<ShareType> shareTypes, Action<ShareType> action, string text)
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