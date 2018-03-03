using Appccelerate.StateMachine;
using System;
using ActionType = System.String;

namespace Pandemic
{
    public class ActionStateMachine
    {
        private PassiveStateMachine<ActionStates, ActionType> _passiveStateMachine;

        public ActionStateMachine()
        {
            _passiveStateMachine = new PassiveStateMachine<ActionStates, ActionType>();

            _passiveStateMachine.In(ActionStates.Waiting)
                .On(Treat)
                    .If(() => CurrentCharacter.DiseasesToTreat() > 1).Goto(ActionStates.ChooseDisease).Execute(OnChooseDisease)
                    .If(() => CurrentCharacter.DiseasesToTreat() == 1).Execute(TreatDisease);
        }

        public event EventHandler<TreatDiseaseEventArgs> AfterTreatDisease;

        public event EventHandler ChooseDisease;

        public enum ActionStates
        {
            Waiting,
            ChooseDisease,
            ChooseCity,
            ChooseCard,
            ChooseCharacter
        }

        public Character CurrentCharacter { get; set; }

        public void DoAction(ActionType actionType)
        {
            _passiveStateMachine.Fire(actionType);
        }

        public void DoAction(ActionType actionType, object parameter)
        {
            _passiveStateMachine.Fire(actionType, parameter);
        }

        public void Start()
        {
            _passiveStateMachine.Initialize(ActionStates.Waiting);
            _passiveStateMachine.Start();
        }

        public void Stop()
        {
            _passiveStateMachine.Stop();
        }

        protected void OnAfterTreatDisease(TreatDiseaseEventArgs eventArgs)
        {
            AfterTreatDisease?.Invoke(this, eventArgs);
        }

        private void OnChooseDisease()
        {
            ChooseDisease?.Invoke(this, EventArgs.Empty);
        }

        private void TreatDisease()
        {
            foreach (var infection in CurrentCharacter.CurrentMapCity.Infections)
            {
                if (infection.Value > 0)
                {
                    int removedCubes = CurrentCharacter.TreatDisease(infection.Key);
                    OnAfterTreatDisease(new TreatDiseaseEventArgs(removedCubes, infection.Key));
                    break;
                }
            }
        }

        #region "Action types"

        public static readonly ActionType Build = "Build";
        public static readonly ActionType DirectFlight = "DirectFlight";
        public static readonly ActionType Discover = "Discover";
        public static readonly ActionType DriveOrFrerry = "DriveOrFrerry";
        public static readonly ActionType CharterFlight = "CharterFlight";
        public static readonly ActionType Share = "Share";
        public static readonly ActionType ShuttleFlight = "ShuttleFlight";
        public static readonly ActionType Treat = "Treat";

        #endregion "Action types"
    }

    public class TreatDiseaseEventArgs
    {
        public TreatDiseaseEventArgs(int cubesCount, DiseaseColor diseaseColor)
        {
            CubesCount = cubesCount;
            DiseaseColor = diseaseColor;
        }

        public int CubesCount { get; }
        public DiseaseColor DiseaseColor { get; }
    }
}