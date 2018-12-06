using Pandemic.Cards;
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
            CityCard cityCard = Character.CityCards.FirstOrDefault(c => c.City.Equals(Character.CurrentMapCity.City));

            Character.RemoveCard(cityCard);
            Game.AddCardToPlayerDiscardPile(cityCard);
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
}