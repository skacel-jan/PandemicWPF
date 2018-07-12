using System;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class BuildAction : CharacterAction
    {
        public override string Name => ActionTypes.Build;

        public BuildAction(Character character) : base(character)
        {
        }

        public override bool CanExecute(Game game)
        {
            return !Character.CurrentMapCity.HasResearchStation && Character.HasCityCard(Character.CurrentMapCity.City);
        }

        protected override void Execute()
        {
            if (Game.ResearchStationsPile > 0)
            {
                BuildStation();
            }
            else
            {
                Game.SelectCity(Game.WorldMap.Cities.Values.Where(c => c.HasResearchStation), DestroyStation, "Select city with research station");
            }
        }

        private void DestroyStation(MapCity city)
        {
            city.HasResearchStation = false;
            Game.ResearchStationsPile++;

            BuildStation();
        }

        private void BuildStation()
        {
            Character.CurrentMapCity.HasResearchStation = true;
            Character.RemoveCard(Character.CurrentMapCity.City);

            Game.ResearchStationsPile--;

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
            Game.ResearchStationsPile--;

            FinishAction();
        }
    }
}