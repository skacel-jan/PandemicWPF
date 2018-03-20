using Appccelerate.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandemic
{
    public enum ShareType
    {
        Give,
        Take
    }

    public class ActionStateMachine
    {
        private PassiveStateMachine<ActionStates, string> _actionsStateMachine;

        private MoveData _moveData;
        private ShareKnowledgeData _shareKnowledgeData;
        private SpecialActions _specialActions;

        public ActionStateMachine(Board board, SpecialActions specialActions, IGameData gameData)
        {
            Board = board;
            _specialActions = specialActions;

            foreach (var character in gameData.Characters)
            {
                character.RegisterSpecialActions(specialActions);
            }

            GameData = gameData;
            _actionsStateMachine = new PassiveStateMachine<ActionStates, string>();

            _actionsStateMachine.In(ActionStates.Waiting)
                .On(Treat)
                    .If(() => CurrentCharacter.CurrentMapCity.DiseasesToTreat.Count > 1)
                        .Goto(ActionStates.DiseaseSelection)
                    .If(() => CurrentCharacter.CurrentMapCity.DiseasesToTreat.Count == 1)
                        .Execute(TreatDisease)
                .On(Build)
                    .If(() => CurrentCharacter.CanBuildResearchStation() && GameData.ResearchStationsPile > 0)
                        .Execute(BuildStructure)
                    .If(() => CurrentCharacter.CanBuildResearchStation() && GameData.ResearchStationsPile == 0)
                        .Goto(ActionStates.CitySelection)
                .On(Discover)
                    .If(() => CurrentCharacter.MostCardsColorCount > CurrentCharacter.CardsForCure)
                        .Goto(ActionStates.CardsSelection).Execute(SelectCardsForCure)
                    .Otherwise()
                        .Execute(() => DiscoverCure(CurrentCharacter.Cards.Where(card => card.City.Color == CurrentCharacter.MostCardsColor)))
                .On(Share)
                    .Goto(ActionStates.CharacterSelection)
                .On(Move)
                    .Goto(ActionStates.MoveSelection).Execute(EnableDestinationCities)
                .On(DriveOrFerry).Execute<MapCity>(MoveToCity);

            _actionsStateMachine.In(ActionStates.CitySelection)
                .ExecuteOnEntry(SelectCity)
                .On(Build)
                    .Goto(ActionStates.Waiting).Execute<MapCity>(DestroyStructure)
                .On(Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.CardsSelection)
                .On(Discover)
                    .Goto(ActionStates.Waiting).Execute<IEnumerable<CityCard>>(DiscoverCure)
                 .On(Share)
                    .Goto(ActionStates.Waiting).Execute((PlayerCard c) =>
                    {
                        _shareKnowledgeData.Card = c;
                        ShareKnowledge();
                    })
                 .On(Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.ShareTypeSelection)
                .ExecuteOnEntry(SelectShareType)
                .On(Share)
                    .Goto(ActionStates.CardsSelection).Execute(SelectCardForShare)
                .On(Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.CharacterSelection)
                .ExecuteOnEntry(SelectCharacter)
                .On(Share)
                    .Goto(ActionStates.ShareTypeSelection).Execute<Character>((character) => _shareKnowledgeData = new ShareKnowledgeData() { Character = character })
                .On(Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.DiseaseSelection)
                .ExecuteOnEntry(ChooseDisease)
                .On(Treat)
                    .Goto(ActionStates.Waiting).Execute<DiseaseColor>(TreatDisease)
                .On(Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.MoveSelection)
                .On(Move).Execute<MapCity>(DestinationCitySelected)
                .On(Cancel)
                    .Goto(ActionStates.Waiting);
        }

        public event EventHandler ActionDone;

        public event EventHandler<CardsSelectingEventArgs> CardsSelecting;

        public event EventHandler<InfoTextEventArgs> CitySelecting;

        public event EventHandler DiseaseSelecting;

        public event EventHandler CharacterSelecting;

        public event EventHandler<MoveTypeEventArgs> MoveTypeSelecting;

        public event EventHandler<ShareTypeEventArgs> ShareTypeSelecting;

        public enum ActionStates
        {
            Waiting,
            DiseaseSelection,
            CitySelection,
            CardsSelection,
            CharacterSelection,
            ShareTypeSelection,
            MoveSelection
        }

        public Board Board { get; }

        public Character CurrentCharacter { get; set; }

        public IGameData GameData { get; }

        public void DoAction(string actionType)
        {
            _actionsStateMachine.Fire(actionType);
        }

        public void DoAction(string actionType, object parameter)
        {
            _actionsStateMachine.Fire(actionType, parameter);
        }

        public void Start()
        {
            _actionsStateMachine.Initialize(ActionStates.Waiting);
            _actionsStateMachine.Start();
        }

        public void Stop()
        {
            _actionsStateMachine.Stop();
        }

        protected virtual void OnActionDone(EventArgs e)
        {
            ActionDone?.Invoke(this, e);
        }

        protected virtual void OnCardsSelecting(CardsSelectingEventArgs e)
        {
            CardsSelecting?.Invoke(this, e);
        }

        protected virtual void OnCitySelecting(InfoTextEventArgs e)
        {
            CitySelecting?.Invoke(this, e);
        }

        protected virtual void OnDiseaseSeleting(EventArgs e)
        {
            DiseaseSelecting?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCharacterSelecting(EventArgs e)
        {
            CharacterSelecting?.Invoke(this, e);
        }

        protected virtual void OnMoveTypeSelecting(MoveTypeEventArgs e)
        {
            MoveTypeSelecting?.Invoke(this, e);
        }

        protected virtual void OnShareTypeSelecting(ShareTypeEventArgs e)
        {
            ShareTypeSelecting?.Invoke(this, e);
        }

        private void BuildStructure()
        {
            PlayerCard card = CurrentCharacter.BuildResearhStation();
            Board.PlayerDiscardPile.AddCard(card);
            Board.GameData.ResearchStationsPile--;

            OnActionDone(EventArgs.Empty);
        }

        private void DestinationCitySelected(MapCity destinationCity)
        {
            if (CurrentCharacter.CanDriveOrFerry(destinationCity))
            {
                CurrentCharacter.DriveOrFerry(destinationCity);
                MoveDone();
            }
            else if (CurrentCharacter.CanShuttleFlight(destinationCity))
            {
                CurrentCharacter.ShuttleFlight(destinationCity);
                MoveDone();
            }
            else
            {
                _moveData = new MoveData() { City = destinationCity };

                var moves = CurrentCharacter.GetPossibleMoveTypes(destinationCity);

                if (moves.Count > 1)
                {
                    var action = new Action<string>((string moveType) =>
                    {
                        DoAction(moveType);
                    });

                    OnMoveTypeSelecting(new MoveTypeEventArgs(moves, action));
                }
                else
                {
                    //ActionViewModel = new CardSelectionViewModel(CurrentCharacter.Cards, Messenger.MoveAction);
                }
            }
        }

        private void DestroyStructure(MapCity mapCity)
        {
            if (mapCity != null)
            {
                mapCity.HasResearchStation = false;
                Board.GameData.ResearchStationsPile++;
            }

            DoAction(Build);
        }

        private void DiscoverCure(IEnumerable<CityCard> cards)
        {
            var colorToCure = cards.First().City.Color;
            Board.DiscoverCure(colorToCure);
            foreach (var card in new List<CityCard>(cards))
            {
                CurrentCharacter.RemoveCard(card as PlayerCard);
                Board.PlayerDiscardPile.AddCard(card);
            }

            _specialActions.DoDiseaseCuredActions(colorToCure);
            OnActionDone(EventArgs.Empty);
        }

        private void EnableDestinationCities()
        {
            Task.Run(() =>
            {
                foreach (var city in CurrentCharacter.GetPossibleDestinationCities(Board.WorldMap.Cities.Values))
                {
                    city.IsMoveEnabled = true;
                }
            });
        }

        private void ChooseDisease()
        {
            OnDiseaseSeleting(EventArgs.Empty);
        }

        private void MoveDone()
        {
            Task.Run(() =>
            {
                foreach (var city in Board.WorldMap.Cities.Values)
                {
                    city.IsMoveEnabled = false;
                }
            });
            OnActionDone(EventArgs.Empty);
        }

        private void MoveToCity(MapCity city)
        {
            if (CurrentCharacter.CanDriveOrFerry(city))
            {
                CurrentCharacter.DriveOrFerry(city);
                MoveDone();
            }
        }

        private void SelectCardForShare()
        {
            var action = new Action<Card>((Card card) =>
            {
                if (card is CityCard cityCard)
                {
                    if (cityCard.City == CurrentCharacter.CurrentMapCity.City)
                    {
                        DoAction(Share, cityCard);
                    }
                }
            });

            var cards = _shareKnowledgeData.Type == ShareType.Give ? CurrentCharacter.Cards : _shareKnowledgeData.Character.Cards;

            OnCardsSelecting(new CardsSelectingEventArgs(cards,
                string.Format("Select card of a current city ({0}) to share", CurrentCharacter.CurrentMapCity.City), action));
        }

        private void SelectCardsForCure()
        {
            DiseaseColor color = CurrentCharacter.MostCardsColor;
            int cardsForCure = CurrentCharacter.CardsForCure;
            var selectedCards = new List<CityCard>();

            var action = new Action<Card>((Card card) =>
            {
                if (card is CityCard cityCard)
                {
                    if (color == cityCard.City.Color && !selectedCards.Remove(cityCard))
                    {
                        selectedCards.Add(cityCard);
                    }
                    else if (!selectedCards.Remove(cityCard))
                    {
                        selectedCards.Add(cityCard);
                    }

                    if (cardsForCure == selectedCards.Count)
                    {
                        DoAction(Discover, selectedCards);
                    }
                }
            });

            OnCardsSelecting(new CardsSelectingEventArgs(CurrentCharacter.Cards.Where(card => card.City.Color == CurrentCharacter.MostCardsColor),
                string.Format("Select {0} cards to discover a cure", CurrentCharacter.CardsForCure), action));
        }

        private void SelectCity()
        {
            OnCitySelecting(new InfoTextEventArgs(string.Format("Cannot build another research station. One must be destroyed.{0}Please select city where research station is built.", Environment.NewLine)));
        }

        private void SelectCharacter()
        {
            OnCharacterSelecting(EventArgs.Empty);
        }

        private void SelectShareType()
        {
            OnShareTypeSelecting(new ShareTypeEventArgs((type) =>
            {
                _shareKnowledgeData.Type = type;
                DoAction(Share);
            }));
        }

        private void ShareKnowledge()
        {
            if (_shareKnowledgeData.Type == ShareType.Give)
            {
                CurrentCharacter.ShareKnowledgeGive(_shareKnowledgeData.Card, _shareKnowledgeData.Character);
            }
            else
            {
                CurrentCharacter.ShareKnowledgeTake(_shareKnowledgeData.Card, _shareKnowledgeData.Character);
            }

            OnActionDone(EventArgs.Empty);
        }

        private void TreatDisease()
        {
            TreatDisease(CurrentCharacter.CurrentMapCity.DiseasesToTreat.First());
        }

        private void TreatDisease(DiseaseColor color)
        {
            var count = CurrentCharacter.TreatDisease(color);
            Board.IncreaseCubePile(color, count);

            OnActionDone(EventArgs.Empty);
        }

        #region "Action types"

        public static readonly string Build = "Build";
        public static readonly string Cancel = "Cancel";
        public static readonly string DirectFlight = "DirectFlight";
        public static readonly string Discover = "Discover";
        public static readonly string DriveOrFerry = "DriveOrFerry";
        public static readonly string CharterFlight = "CharterFlight";
        public static readonly string Move = "Move";
        public static readonly string Share = "Share";
        public static readonly string ShuttleFlight = "ShuttleFlight";
        public static readonly string Treat = "Treat";

        #endregion "Action types"
    }

    public class MoveData
    {
        public PlayerCard Card { get; set; }
        public MapCity City { get; set; }
    }

    public class ShareKnowledgeData
    {
        public PlayerCard Card { get; set; }
        public Character Character { get; set; }
        public ShareType Type { get; set; }
    }

    public class SpecialActions
    {
        public SpecialActions()
        {
            DiseaseCuredActions = new List<Action<DiseaseColor>>();
        }

        public IList<Action<DiseaseColor>> DiseaseCuredActions { get; }

        public void DoDiseaseCuredActions(DiseaseColor color)
        {
            foreach (var action in DiseaseCuredActions)
            {
                action(color);
            }
        }
    }
}