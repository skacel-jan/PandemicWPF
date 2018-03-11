using Appccelerate.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using ActionType = System.String;

namespace Pandemic
{
    public class ActionStateMachine
    {
        private PassiveStateMachine<ActionStates, ActionType> _passiveStateMachine;

        public ActionStateMachine(IGameData gameData)
        {
            GameData = gameData ?? throw new ArgumentNullException(nameof(gameData));

            _passiveStateMachine = new PassiveStateMachine<ActionStates, ActionType>();

            _passiveStateMachine.In(ActionStates.Waiting)
                .On(Treat)
                    .If(() => CurrentCharacter.DiseasesToTreat() > 1)
                        .Goto(ActionStates.DiseaseSelection).Execute(ChooseDisease)
                    .If(() => CurrentCharacter.DiseasesToTreat() == 1)
                        .Execute(TreatDisease)
                .On(Build)
                    .If(() => CurrentCharacter.CanBuildResearchStation() && GameData.ResearchStationsPile > 0)
                        .Execute(BuildStructure)
                    .If(() => CurrentCharacter.CanBuildResearchStation() && GameData.ResearchStationsPile == 0)
                        .Goto(ActionStates.CitySelection)
                .On(Discover)
                    .If(() => CurrentCharacter.MostCardsColorCount > CurrentCharacter.CardsForCure)
                        .Goto(ActionStates.CardsSelection)
                    .Otherwise()
                        .Execute(() => DiscoverCure(CurrentCharacter.Cards.Where(card => card.City.Color == CurrentCharacter.MostCardsColor)))
                .On(Share)
                    .Goto(ActionStates.ShareTypeSelection);

            _passiveStateMachine.In(ActionStates.CitySelection)
                .ExecuteOnEntry(SelectCity)
                .On(Build)
                    .Goto(ActionStates.Waiting).Execute<MapCity>(DestroyAndBuildStructure);

            _passiveStateMachine.In(ActionStates.CardsSelection)
                .ExecuteOnEntry(SelectCards)
                .On(Discover)
                    .Goto(ActionStates.Waiting).Execute<IEnumerable<CityCard>>(DiscoverCure);

            _passiveStateMachine.In(ActionStates.ShareTypeSelection)
                .ExecuteOnEntry(SelectShareType)
                .On(ShareTake)                    
                    .Goto(ActionStates.CharacterSelection).Execute<MapCity>(DestroyAndBuildStructure)
                .On(ShareGive)
                    .Goto(ActionStates.Waiting).Execute<MapCity>(DestroyAndBuildStructure);
        }

        private void SelectShareType()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<CardsSelectingEventArgs> CardsSelecting;

        public event EventHandler<InfoTextEventArgs> CitySelecting;

        public event EventHandler<CureDiscoveredEventArgs> CureDiscovered;

        public event EventHandler DiseaseSelecting;

        public event EventHandler<TreatDiseaseEventArgs> DiseaseTreated;

        public event EventHandler<StructureBuiltEventArgs> ResearchStationBuilt;

        public enum ActionStates
        {
            Waiting,
            DiseaseSelection,
            CitySelection,
            CardsSelection,
            CharacterSelection,
            ShareTypeSelection
        }

        public Character CurrentCharacter { get; set; }

        public IGameData GameData { get; }

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

        protected void OnCardsSelecting(CardsSelectingEventArgs e)
        {
            CardsSelecting?.Invoke(this, e);
        }

        protected void OnCitySelecting(InfoTextEventArgs e)
        {
            CitySelecting?.Invoke(this, e);
        }

        protected void OnCureDiscovered(CureDiscoveredEventArgs e)
        {
            CureDiscovered?.Invoke(this, e);
        }

        protected void OnDiseaseSeleting(EventArgs e)
        {
            DiseaseSelecting?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDiseaseTreated(TreatDiseaseEventArgs e)
        {
            DiseaseTreated?.Invoke(this, e);
        }

        protected void OnResearchStationBuild(StructureBuiltEventArgs e)
        {
            ResearchStationBuilt?.Invoke(this, e);
        }

        private void BuildStructure()
        {
            PlayerCard card = CurrentCharacter.BuildResearhStation();
            GameData.ResearchStationsPile--;
            OnResearchStationBuild(new StructureBuiltEventArgs(card));
        }

        private void DestroyAndBuildStructure(MapCity mapCity)
        {
            if (mapCity.HasResearchStation)
            {
                mapCity.HasResearchStation = false;
                BuildStructure();
            }
            else
            {
                _passiveStateMachine.Fire(Build);
            }
        }

        private void DiscoverCure(IEnumerable<CityCard> cards)
        {
            OnCureDiscovered(new CureDiscoveredEventArgs(CurrentCharacter.MostCardsColor, cards));
        }

        private void ChooseDisease()
        {
            OnDiseaseSeleting(EventArgs.Empty);
        }

        private void SelectCards()
        {
            OnCardsSelecting(new CardsSelectingEventArgs(CurrentCharacter.CardsForCure, string.Format("Select {0} cards for cure", CurrentCharacter.CardsForCure)));
        }

        private void SelectCity()
        {
            OnCitySelecting(new InfoTextEventArgs(string.Format("Cannot build another research station. One must be destroyed.{0}Please select city where research station is built.", Environment.NewLine)));
        }

        private void TreatDisease()
        {
            foreach (var infection in CurrentCharacter.CurrentMapCity.Infections)
            {
                if (infection.Value > 0)
                {
                    int removedCubes = CurrentCharacter.TreatDisease(infection.Key);
                    OnDiseaseTreated(new TreatDiseaseEventArgs(removedCubes, infection.Key));
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
        public static readonly ActionType ShareGive = "ShareGive";
        public static readonly ActionType ShareTake = "ShareTake";
        public static readonly ActionType ShuttleFlight = "ShuttleFlight";
        public static readonly ActionType Treat = "Treat";

        #endregion "Action types"
    }
}