using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class BuildAction : CharacterAction
    {
        public BuildAction(Character character) : base(character)
        {
        }

        public override string Name => ActionTypes.Build;

        public override bool CanExecute(Game game)
        {
            return !Character.CurrentMapCity.HasResearchStation && Character.HasCityCard(Character.CurrentMapCity.City);
        }

        protected override void Execute()
        {
            if (Game.ResearchStationsPile == 0)
            {
                Game.SelectionService.Select(new SelectAction<MapCity>(SetCity, Game.WorldMap.Cities.Where(c => c.HasResearchStation),
                    "Select city with research station to destroy"));
            }
            else
            {
                BuildStation(Character.CurrentMapCity);
            }
        }

        private void SetCity(MapCity mapCity)
        {
            DestroyStation(mapCity);
            BuildStation(Character.CurrentMapCity);
        }

        private void BuildStation(MapCity mapCity)
        {
            mapCity.HasResearchStation = true;
            Character.RemoveCard(mapCity.City);

            Game.ResearchStationsPile--;

            FinishAction();
        }

        private void DestroyStation(MapCity city)
        {
            city.HasResearchStation = false;
            Game.ResearchStationsPile++;
        }
    }

    public class OperationsExpertBuildAction : BuildAction
    {
        public OperationsExpertBuildAction(Character character) : base(character)
        {
        }

        public override bool CanExecute(Game game)
        {
            return !Character.CurrentMapCity.HasResearchStation;
        }

        protected override void Execute()
        {
            Character.CurrentMapCity.HasResearchStation = true;
            Game.ResearchStationsPile--;

            FinishAction();
        }
    }

    public class Build : INext
    {
        private Queue<IAction> actions;
        private IGamePhase _phase;

        public Character Character { get; }

        public Game Game => _phase.Game;

        public Build(Character character)
        {
            actions = new Queue<IAction>();
            actions.Enqueue(new DestroyCityAction(this));

            Character = character;
        }

        public void Execute(IGamePhase phase)
        {            
            _phase = phase;
            Next();
        }

        public void Next()
        {
            if (actions.Count > 0)
            {
                var action = actions.Dequeue();
                if (action.CanExecute(null))
                {
                    action.Execute(null);
                }
                else
                {
                    Next();
                }
            }
            else
            {
                DoAction();
                _phase.End();
            }        
        }

        private void DoAction()
        {
            Character.CurrentMapCity.HasResearchStation = true;
            Character.RemoveCard(Character.CurrentMapCity.City);

            _phase.Game.ResearchStationsPile--;
        }
    }

    public class DestroyCityAction : IAction
    {
        private MapCity _mapCity;

        public DestroyCityAction(INext buildAction)
        {
            BuildAction = buildAction;
        }

        public INext BuildAction { get; }

        public bool CanExecute(object param)
        {
            var mapCity = (MapCity)param;
            return BuildAction.Game.ResearchStationsPile == 0;
        }

        public void Execute(object param)
        {
            BuildAction.Game.SelectionService.Select(new SelectAction<MapCity>(DestroyCity,
                BuildAction.Game.WorldMap.Cities.Where(c => c.HasResearchStation),
                   "Select city with research station to destroy"));
        }

        private void DestroyCity(MapCity mapCity)
        {
            _mapCity = mapCity;
            _mapCity.HasResearchStation = false;
            BuildAction.Next();
        }

        public void Undo()
        {
            _mapCity.HasResearchStation = true;
        }
    }
}