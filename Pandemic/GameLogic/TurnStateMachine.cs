using Appccelerate.StateMachine;
using GalaSoft.MvvmLight;
using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic
{
    public class TurnStateMachine : ObservableObject
    {
        private int _actions;
        private PassiveStateMachine<TurnStates, TurnEvents> _phaseStateMachine;

        public TurnStateMachine(CircularCollection<Character> _characters, Board board)
        {
            Characters = _characters;
            Board = board;

            foreach (var character in Characters)
            {
                DrawPlayerCards(6 - Characters.Count, character);
            }

            Board.PlayerDeck.AddEpidemicCards(5);

            _phaseStateMachine = new PassiveStateMachine<TurnStates, TurnEvents>();

            _phaseStateMachine.In(TurnStates.StartOfTurn)
                .ExecuteOnEntry(ExecuteStartTurnEvent)
                .On(TurnEvents.Next).Goto(TurnStates.ActionPhase);

            _phaseStateMachine.In(TurnStates.ActionPhase)
                .ExecuteOnExit(() => OnActionPhaseEnded(EventArgs.Empty))
                .On(TurnEvents.Next)
                    .If(() => Actions == 0).Goto(TurnStates.DrawingPhase)
                    .Otherwise().Execute(ExecuteActionEvent);

            _phaseStateMachine.In(TurnStates.DrawingPhase)
                .On(TurnEvents.Next)
                    .If(() => Draws == 0).Goto(TurnStates.InfectionPhase)
                        .Execute(() => OnDrawingPhaseEnded(EventArgs.Empty))
                    .Otherwise().Execute(ExecuteDrawEvent)
                .On(TurnEvents.GameOver).Goto(TurnStates.GameLost);

            _phaseStateMachine.In(TurnStates.InfectionPhase)
                .ExecuteOnExit(() => OnInfectionPhaseEnded(EventArgs.Empty))
                .On(TurnEvents.Next)
                    .If(() => Infections == 0).Goto(TurnStates.EndOfTurn)
                    .Otherwise().Execute(ExecuteInfectionEvent);

            _phaseStateMachine.In(TurnStates.EndOfTurn)
                .ExecuteOnExit(ExecuteEndOfTurn)
                .On(TurnEvents.Next).Goto(TurnStates.StartOfTurn);

            _phaseStateMachine.In(TurnStates.GameLost)
                .ExecuteOnEntry(() => GameLost?.Invoke(this, new GenericEventArgs<string>("Game over: No more cards")));
        }

        public event EventHandler ActionDone;

        public event EventHandler ActionPhaseEnded;

        public event EventHandler Discarding;

        public event EventHandler DrawDone;

        public event EventHandler DrawingPhaseEnded;

        public event EventHandler<GenericEventArgs<string>> GameLost;

        public event EventHandler<InfectionEventArgs> InfectionDone;

        public event EventHandler InfectionPhaseEnded;

        public event EventHandler<OutbreakEventArgs> Outbreak;

        public event EventHandler TurnStarted;

        public enum TurnEvents
        {
            Next,
            GameOver
        }

        public enum TurnStates
        {
            StartOfTurn,
            ActionPhase,
            DrawingPhase,
            InfectionPhase,
            EndOfTurn,
            GameWon,
            GameLost
        }

        public int Actions
        {
            get => _actions;
            set => Set(ref _actions, value);
        }

        public Board Board { get; }

        public int Draws { get; private set; }

        public CircularCollection<Character> Characters { get; set; }

        public int Infections { get; private set; }

        public bool CanRaiseInfection(MapCity city, DiseaseColor color)
        {
            bool result = true;

            foreach (var character in Characters)
            {
                result = result && !character.CanPreventInfection(city, color);
            }

            return result;
        }

        public void DoAction()
        {
            _phaseStateMachine.Fire(TurnEvents.Next);
        }

        public void GameOver()
        {
            _phaseStateMachine.Fire(TurnEvents.GameOver);
        }

        public bool IsActionPhase() => Actions > 0;

        public void Start()
        {
            _phaseStateMachine.Initialize(TurnStates.StartOfTurn);
            _phaseStateMachine.Start();
        }

        public void Stop()
        {
            _phaseStateMachine.Stop();
        }

        protected void OnActionDone(EventArgs e)
        {
            ActionDone?.Invoke(this, e);
        }

        protected void OnActionPhaseEnded(EventArgs e)
        {
            ActionPhaseEnded?.Invoke(this, e);
        }

        protected void OnDrawDone(EventArgs e)
        {
            DrawDone?.Invoke(this, e);
        }

        protected void OnDrawingPhaseEnded(EventArgs e)
        {
            DrawingPhaseEnded?.Invoke(this, e);
        }

        protected void OnEpidemic(EventArgs empty)
        {
            //ActionViewModel = new TextViewModel("Epidemic");
        }

        protected void OnInfectionDone(InfectionEventArgs e)
        {
            InfectionDone?.Invoke(this, e);
        }

        protected void OnInfectionPhaseEnded(EventArgs e)
        {
            InfectionPhaseEnded?.Invoke(this, e);
        }

        protected void OnTurnStarted(EventArgs e)
        {
            TurnStarted?.Invoke(this, e);
        }

        private void DoEpidemicActions()
        {
            Board.RaiseInfectionPosition();
            InfectionCard card = Board.DrawInfectionBottomCard();
            if (CanRaiseInfection(Board.WorldMap.Cities[card.City.Name], card.City.Color))
            {
                bool isOutbreak = Board.RaiseInfection(card.City, card.City.Color);
                if (isOutbreak)
                {
                    DoOutbreak(card.City, card.City.Color);
                }
            }

            Board.ShuffleInfectionDiscardPileBack();

            OnEpidemic(EventArgs.Empty);
        }

        private void DoOutbreak(City city, DiseaseColor diseaseColor)
        {
            var citiesToOutbreak = new Queue<City>(1);
            var alreadyOutbreakedCities = new List<City>();
            citiesToOutbreak.Enqueue(city);

            OnOutbreak(new OutbreakEventArgs(city));

            while (citiesToOutbreak.Count > 0)
            {
                var outbreakCity = citiesToOutbreak.Dequeue();
                alreadyOutbreakedCities.Add(outbreakCity);

                foreach (var connectedCity in Board.WorldMap.Cities[outbreakCity.Name].ConnectedCities)
                {
                    if (CanRaiseInfection(connectedCity, diseaseColor))
                    {
                        bool isOutbreak = Board.RaiseInfection(connectedCity.City, diseaseColor);
                        if (Board.CheckCubesPile(city.Color))
                        {
                            GameOver();
                        }

                        if (isOutbreak && !alreadyOutbreakedCities.Contains(connectedCity.City) && !citiesToOutbreak.Contains(connectedCity.City))
                        {
                            citiesToOutbreak.Enqueue(connectedCity.City);
                            OnOutbreak(new OutbreakEventArgs(connectedCity.City));
                        }
                    }
                }
            }
        }

        private bool DrawPlayerCards(int count, Character character)
        {
            bool isGameOver = false;
            foreach (var i in Enumerable.Range(0, count))
            {
                Card card = Board.DrawPlayerCard();

                if (card == null)
                {
                    isGameOver = true;
                }

                if (card is PlayerCard)
                {
                    character.AddCard(card);
                }
                else if (card is EventCard)
                {
                    character.AddCard(card);
                }
                else if (card is EpidemicCard epidemicCard)
                {
                    DoEpidemicActions();
                }
            }
            return isGameOver;
        }

        private void ExecuteActionEvent()
        {
            Actions--;
            OnActionDone(new GenericEventArgs<int>(Actions));
            if (Actions == 0)
            {
                _phaseStateMachine.Fire(TurnEvents.Next);
            }
        }

        private void ExecuteDiscardEvent()
        {
            Discarding?.Invoke(this, EventArgs.Empty);
        }

        private void ExecuteDrawEvent()
        {
            if (DrawPlayerCards(1, Characters.Current))
            {
                GameOver();
            }

            Draws -= 1;
            _phaseStateMachine.Fire(TurnEvents.Next);
        }

        private void ExecuteEndOfTurn()
        {
            Characters.Current.IsActive = false;

            Characters.Next();
        }

        private void ExecuteInfectionEvent()
        { 
            Infections--;
            InfectionCard card = Board.DrawInfectionCard();
            if (Board.CheckCubesPile(card.City.Color))
            {
                GameOver();
            }
            else
            {
                if (CanRaiseInfection(Board.WorldMap.Cities[card.City.Name], card.City.Color))
                {
                    var isOutbreak = Board.RaiseInfection(card.City, card.City.Color);
                    if (isOutbreak)
                    {
                        DoOutbreak(card.City, card.City.Color);
                    }
                }
            }

            if (Infections == 0)
            {
                _phaseStateMachine.Fire(TurnEvents.Next);
            }

            OnInfectionDone(new InfectionEventArgs(card.City));
        }

        private void ExecuteStartTurnEvent()
        {
            Characters.Current.IsActive = true;
            Actions = Characters.Current.ActionsCount;
            Draws = 2;
            Infections = Board.GameData.InfectionRate;

            OnTurnStarted(EventArgs.Empty);
            _phaseStateMachine.Fire(TurnEvents.Next);
        }

        private void OnOutbreak(OutbreakEventArgs e)
        {
            Board.GameData.Outbreaks++;
            Outbreak?.Invoke(this, e);
        }

        public void SkipInfectionPhase()
        {
            Infections = 0;
        }
    }
}