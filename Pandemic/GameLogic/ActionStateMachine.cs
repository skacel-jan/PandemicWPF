using Appccelerate.StateMachine;
using Pandemic.Cards;
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
                character.CardDiscarded += Character_CardDiscarded;
            }

            GameData = gameData;
            _actionsStateMachine = new PassiveStateMachine<ActionStates, string>();

            _actionsStateMachine.In(ActionStates.Waiting)
                .On(ActionTypes.Treat)
                    .If(() => CurrentCharacter.CurrentMapCity.DiseasesToTreat.Count > 1)
                        .Goto(ActionStates.DiseaseSelection)
                    .If(() => CurrentCharacter.CurrentMapCity.DiseasesToTreat.Count == 1)
                        .Execute(TreatDisease)
                .On(ActionTypes.Build)
                    .If(() => CurrentCharacter.BuildBehaviour.CanBuild(CurrentCharacter.CurrentMapCity) && GameData.ResearchStationsPile > 0)
                        .Execute(BuildStructure)
                    .If(() => CurrentCharacter.BuildBehaviour.CanBuild(CurrentCharacter.CurrentMapCity) && GameData.ResearchStationsPile == 0)
                        .Goto(ActionStates.CitySelection).Execute(SelectResearchStationToDestroy)
                .On(ActionTypes.Discover)
                    .If(() => CurrentCharacter.MostCardsColorCount > CurrentCharacter.CardsForCure)
                        .Goto(ActionStates.CardsSelection).Execute(SelectCardsForCure)
                    .Otherwise()
                        .Execute(() => DiscoverCure(CurrentCharacter.CityCards.Where(card => card.City.Color == CurrentCharacter.MostCardsColor)))
                .On(ActionTypes.Share)
                    .Goto(ActionStates.ShareTypeSelection)
                .On(ActionTypes.Move)
                    .Goto(ActionStates.MoveSelection)
                        .Execute(EnableDestinationCities)
                .On(ActionTypes.DriveOrFerry)
                    .Execute<MapCity>(MoveToCity)
                .On(ActionTypes.Event)
                    .Goto(ActionStates.CardsSelection)
                        .Execute(SelectEventCard);

            _actionsStateMachine.In(ActionStates.CitySelection)
                .On(ActionTypes.Build)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.CardsSelection)
                .On(ActionTypes.Discover)
                    .Goto(ActionStates.Waiting).Execute<IEnumerable<CityCard>>(DiscoverCure)
                 .On(ActionTypes.Share)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.CharterFlight)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.DirectFlight)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.OperationsExpertSpecialMove)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.GovernmentGrant)
                    .Goto(ActionStates.CitySelection).Execute<EventCard>(GovernmentGrantCitySelection)
                .On(ActionTypes.Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.ShareTypeSelection)
                .ExecuteOnEntry(SelectShareType)
                .On(ActionTypes.Share)
                    .Goto(ActionStates.CharacterSelection)
                .On(ActionTypes.Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.CharacterSelection)
                .ExecuteOnEntry(SelectCharacter)
                .On(ActionTypes.Share)
                    .Goto(ActionStates.CardsSelection)
                        .Execute(SelectCardForShare)
                .On(ActionTypes.Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.DiseaseSelection)
                .ExecuteOnEntry(ChooseDisease)
                .On(ActionTypes.Treat)
                    .Goto(ActionStates.Waiting).Execute<DiseaseColor>(TreatDisease)
                .On(ActionTypes.Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.MoveSelection)
                .On(ActionTypes.Move)
                    .Execute<MapCity>(DestinationCitySelected)
                .On(ActionTypes.DriveOrFerry)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.ShuttleFlight)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.CharterFlight)
                    .Goto(ActionStates.CardsSelection).Execute(SelectCardForMove)
                .On(ActionTypes.DirectFlight)
                    .Goto(ActionStates.CardsSelection).Execute(SelectCardForMove)
                .On(ActionTypes.OperationsExpertSpecialMove)
                    .Goto(ActionStates.CardsSelection).Execute(SelectCardForMove)
                .On(ActionTypes.Cancel)
                    .Goto(ActionStates.Waiting);
        }

        public event EventHandler ActionDone;

        public event EventHandler<CardsSelectingEventArgs> CardsSelecting;

        public event EventHandler<CitySelectingEventArgs> CitySelecting;

        public event EventHandler DiseaseSelecting;

        public event EventHandler EventDone;

        public event EventHandler<CharacterSelectingEventArgs> CharacterSelecting;

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

        protected virtual void OnCitySelecting(CitySelectingEventArgs e)
        {
            CitySelecting?.Invoke(this, e);
        }

        protected virtual void OnDiseaseSeleting(EventArgs e)
        {
            DiseaseSelecting?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnEventDone(EventArgs e)
        {
            EventDone?.Invoke(this, e);
        }

        protected virtual void OnCharacterSelecting(CharacterSelectingEventArgs e)
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
            if (CurrentCharacter.BuildBehaviour.CanBuild(CurrentCharacter.CurrentMapCity))
            {
                CurrentCharacter.BuildBehaviour.Build(CurrentCharacter.CurrentMapCity);
                Board.GameData.ResearchStationsPile--;

                OnActionDone(EventArgs.Empty);
            }
        }

        private void DestinationCitySelected(MapCity destinationCity)
        {
            var possibleMoves = CurrentCharacter.GetPossibleMoves(destinationCity).Where(x => x.IsPossible(destinationCity));
            foreach (var move in possibleMoves)
            {
                CurrentCharacter.Move(move.MoveType, destinationCity);
                MoveDone();
                DoAction(move.MoveType);
                return;
            }

            var possibleCardMoves = CurrentCharacter.GetPossibleCardMoves(destinationCity).Where(x => x.IsPossible(destinationCity));

            _moveData = new MoveData() { City = destinationCity };

            if (possibleCardMoves.Count() > 1)
            {
                var action = new Action<IMoveCardAction>((IMoveCardAction moveAction) =>
                {
                    _moveData.MoveAction = moveAction;
                    DoAction(moveAction.MoveType);
                });

                OnMoveTypeSelecting(new MoveTypeEventArgs(possibleCardMoves, action));
            }
            else
            {
                _moveData.MoveAction = possibleCardMoves.First(); ;
                DoAction(_moveData.MoveAction.MoveType);
            }
        }

        private void DestroyStructure(MapCity mapCity)
        {
            if (mapCity != null)
            {
                mapCity.HasResearchStation = false;
                Board.GameData.ResearchStationsPile++;
            }

            DoAction(ActionTypes.Build);
        }

        private void DiscoverCure(IEnumerable<CityCard> cards)
        {
            var colorToCure = cards.First().City.Color;
            Board.DiscoverCure(colorToCure);
            foreach (var card in new List<CityCard>(cards))
            {
                CurrentCharacter.RemoveCard(card);
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
                    city.IsSelectable = true;
                }
            });
        }

        private void GovernmentGrantCitySelection(EventCard eventCard)
        {
            foreach (var city in Board.WorldMap.Cities.Values.Where(x => !x.HasResearchStation))
            {
                city.IsSelectable = true;
            }

            var action = new Action<MapCity>((MapCity mapCity) =>
            {
                mapCity.HasResearchStation = true;
                eventCard.Character.RemoveCard(eventCard);

                Task.Run(() =>
                {
                    foreach (var city in Board.WorldMap.Cities.Values)
                    {
                        city.IsSelectable = false;
                    }
                });
            });

            OnCitySelecting(new CitySelectingEventArgs("Select city", action));
        }

        private void Character_CardDiscarded(object sender, CardDiscardedEventArgs e)
        {
            Board.AddCardToPlayerDiscardPile(e.Card);
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
                    city.IsSelectable = false;
                }
            });
            OnActionDone(EventArgs.Empty);
        }

        private void MoveToCity(MapCity city)
        {
            if (CurrentCharacter.MoveStrategy.GetMoveAction(ActionTypes.DriveOrFerry).IsPossible(city))
            {
                CurrentCharacter.Move(ActionTypes.DriveOrFerry, city);
                MoveDone();
            }
        }

        private void SelectCardForMove()
        {
            var action = new Action<Card>((Card card) =>
            {
                if (CurrentCharacter.Move(_moveData.MoveAction.MoveType, _moveData.City, card as PlayerCard))
                {
                    MoveDone();
                    DoAction(_moveData.MoveAction.MoveType);
                }
            });

            OnCardsSelecting(new CardsSelectingEventArgs(CurrentCharacter.Cards, "Select card of a destination city", action));
        }

        private void SelectCardForShare()
        {
            var action = new Action<Card>((Card card) =>
            {
                if (card is PlayerCard playerCard)
                {
                    if (_shareKnowledgeData.FromCharacter.ShareKnowledgeBehaviour.CanShare(playerCard))
                    {
                        _shareKnowledgeData.FromCharacter.ShareKnowledgeBehaviour.Share(_shareKnowledgeData.ToCharacter, playerCard);
                        OnActionDone(EventArgs.Empty);
                        DoAction(ActionTypes.Share);
                    }
                }
            });

            var cards = _shareKnowledgeData.FromCharacter.Cards;

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
                        DoAction(ActionTypes.Discover, selectedCards);
                    }
                }
            });

            OnCardsSelecting(new CardsSelectingEventArgs(CurrentCharacter.CityCards.Where(card => card.City.Color == CurrentCharacter.MostCardsColor),
                string.Format("Select {0} cards to discover a cure", CurrentCharacter.CardsForCure), action));
        }

        private void SelectResearchStationToDestroy()
        {
            foreach (var city in Board.WorldMap.Cities.Values.Where(x => x.HasResearchStation))
            {
                city.IsSelectable = true;
            }

            var action = new Action<MapCity>((MapCity mapCity) =>
            {
                mapCity.HasResearchStation = false;
                Board.GameData.ResearchStationsPile++;

                Task.Run(() =>
                {
                    foreach (var city in Board.WorldMap.Cities.Values)
                    {
                        city.IsSelectable = false;
                    }
                });

                DoAction(ActionTypes.Build);
            });

            OnCitySelecting(new CitySelectingEventArgs(string.Format("Cannot build another research station. One must be destroyed.{0}Please select city where research station is built.", Environment.NewLine),
                action));
        }

        private void SelectEventCard()
        {
            var action = new Action<Card>(c =>
            {
                var eventCard = c as EventCard;

                DoAction(eventCard.EventCode, eventCard);
            });

            OnCardsSelecting(new CardsSelectingEventArgs(Board.EventCards, "Select event card", action));
        }

        private void SelectCharacter()
        {
            var action = new Action<Character>((Character character) =>
            {
                if (_shareKnowledgeData.FromCharacter == null)
                {
                    _shareKnowledgeData.FromCharacter = character;
                }
                else
                {
                    _shareKnowledgeData.ToCharacter = character;
                }
                DoAction(ActionTypes.Share);
            });

            var characters = CurrentCharacter.CurrentMapCity.Characters.Where(x => x.ShareKnowledgeBehaviour.IsPossible() && x.ShareKnowledgeBehaviour.Character != CurrentCharacter).Select(y => y.ShareKnowledgeBehaviour.Character);

            OnCharacterSelecting(new CharacterSelectingEventArgs(characters,
                "Select a character for share knowladge", action));
        }

        private void SelectShareType()
        {
            _shareKnowledgeData = new ShareKnowledgeData();
            var shareBehaviours = CurrentCharacter.CurrentMapCity.Characters.Where(x => x.ShareKnowledgeBehaviour.IsPossible()).Select(y => y.ShareKnowledgeBehaviour);
            if (shareBehaviours.Count() > 1)
            {
                OnShareTypeSelecting(new ShareTypeEventArgs(Enum.GetValues(typeof(ShareType)).Cast<ShareType>(), (type) =>
                {
                    if (type == ShareType.Give)
                    {
                        _shareKnowledgeData.FromCharacter = CurrentCharacter;
                    }
                    else
                    {
                        _shareKnowledgeData.ToCharacter = CurrentCharacter;
                    }

                    DoAction(ActionTypes.Share);
                }));
            }
            else
            {
                if (shareBehaviours.First().Character == CurrentCharacter)
                {
                    _shareKnowledgeData.FromCharacter = CurrentCharacter;
                }
                else
                {
                    _shareKnowledgeData.ToCharacter = CurrentCharacter;
                }

                DoAction(ActionTypes.Share);
            }
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
    }

    public class MoveData
    {
        public PlayerCard Card { get; set; }
        public MapCity City { get; set; }
        public IMoveCardAction MoveAction { get; set; }
    }

    public class ShareKnowledgeData
    {
        //public ShareType Type { get; set; }
        public PlayerCard Card { get; internal set; }

        public Character FromCharacter { get; set; }
        public Character ToCharacter { get; set; }
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