using System;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class BuildAction : GameAction
    {
        public BuildAction(Character character) : base(character)
        {
        }

        public override bool CanExecute(Game game)
        {
            return !Character.CurrentMapCity.HasResearchStation && Character.HasCityCard(Character.CurrentMapCity.City);
        }

        protected override void Execute()
        {
            if (_game.ResearchStationsPile > 0)
            {
                BuildStation();
            }
            else
            {
                _game.SelectCity(_game.WorldMap.Cities.Values.Where(c => c.HasResearchStation), DestroyStation, "Select city with research station");
            }
        }

        private void DestroyStation(MapCity city)
        {
            city.HasResearchStation = false;
            _game.ResearchStationsPile++;

            BuildStation();
        }

        private void BuildStation()
        {
            Character.CurrentMapCity.HasResearchStation = true;
            Character.RemoveCard(Character.CurrentMapCity.City);

            _game.ResearchStationsPile--;

            FinishAction();
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
            _game.ResearchStationsPile--;

            FinishAction();
        }
    }
}