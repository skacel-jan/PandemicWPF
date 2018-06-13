using Appccelerate.StateMachine;
using Pandemic.Cards;
using Pandemic.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandemic
{
    public class ActionStateMachine
    {
        private PassiveStateMachine<ActionStates, string> _actionsStateMachine;

        private MoveData _moveData;

        private CitySelectionService CitySelectionService;

        public ActionStateMachine(Game game, CitySelectionService citySelectionService)
        {
            Game = game;

            CitySelectionService = citySelectionService ?? throw new ArgumentNullException(nameof(citySelectionService));
            CitySelectionService.CitySelecting += (s, e) => CitySelecting?.Invoke(s, e);

            Game.EventFinished += (s, e) => OnEventDone(e);

            _actionsStateMachine = new PassiveStateMachine<ActionStates, string>();

            _actionsStateMachine.In(ActionStates.Waiting)
                .On(ActionTypes.Move)
                    .Goto(ActionStates.MoveSelection)
                        .Execute(EnableDestinationCities)
                .On(ActionTypes.DriveOrFerry)
                    .Execute<MapCity>(MoveToCity);

            _actionsStateMachine.In(ActionStates.CitySelection)
                .On(ActionTypes.Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.CardsSelection)
                .On(ActionTypes.CharterFlight)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.DirectFlight)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.OperationsExpertSpecialMove)
                    .Goto(ActionStates.Waiting)
                .On(ActionTypes.Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.ShareTypeSelection)
                .On(ActionTypes.Cancel)
                    .Goto(ActionStates.Waiting);

            _actionsStateMachine.In(ActionStates.CharacterSelection)
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

        public event EventHandler<CitySelectingEventArgs> CitySelecting;

        public event EventHandler EventDone;

        public event EventHandler<MoveTypeEventArgs> MoveTypeSelecting;

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

        public Game Game { get; }

        public Character CurrentCharacter { get => Game.CurrentCharacter; }

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

        protected virtual void OnCitySelecting(CitySelectingEventArgs e)
        {
            CitySelecting?.Invoke(this, e);
        }

        protected virtual void OnEventDone(EventArgs e)
        {
            EventDone?.Invoke(this, e);
        }

        protected virtual void OnMoveTypeSelecting(MoveTypeEventArgs e)
        {
            MoveTypeSelecting?.Invoke(this, e);
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

        private void EnableDestinationCities()
        {
            Task.Run(() =>
            {
                foreach (var city in CurrentCharacter.GetPossibleDestinationCities(Game.WorldMap.Cities.Values))
                {
                    city.IsSelectable = true;
                }
            });
        }

        private void MoveDone()
        {
            Task.Run(() =>
            {
                foreach (var city in Game.WorldMap.Cities.Values)
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

            //OnCardsSelecting(new CardsSelectingEventArgs(CurrentCharacter.Cards, action, "Select card of a destination city"));
        }

    
    }

    public class MoveData
    {
        public PlayerCard Card { get; set; }
        public MapCity City { get; set; }
        public IMoveCardAction MoveAction { get; set; }
    }
}