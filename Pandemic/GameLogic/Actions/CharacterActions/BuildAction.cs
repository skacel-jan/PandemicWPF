using Pandemic.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{    
    public class BuildAction : CharacterAction
    {
        protected MapCity CityToDestroy { get; private set; }

        public BuildAction(Character character, Game game) : base(character, game)
        {           
        }

        public override string Name => ActionTypes.Build;

        public override bool CanExecute()
        {
            return !Character.CurrentMapCity.HasResearchStation && Character.HasCityCard(Character.CurrentMapCity.City);
        }

        protected override void AddEffects()
        {
            if (CityToDestroy != null)
            {
                Effects.Add(new DestroyResearchStationEffect(CityToDestroy, Game));
            }

            Effects.Add(new BuildResearchStationEffect(Character.CurrentMapCity, Game));
            Effects.Add(new DiscardPlayerCardEffect(Character.CityCards.First(x => x.City == Character.CurrentMapCity.City), Game.PlayerDiscardPile));
        }

        protected override void Initialize()
        {
            CityToDestroy = null;

            AddSelectionState(0,
                (g) => g.ResearchStationsPile == 0,
                new CitySelection(
                    SelectionCallback((MapCity c) => CityToDestroy = c),
                    Game.WorldMap.Cities.Where(c => c.HasResearchStation),
                    "Select city where to destroy research station"));
        }
    }

    public class OperationsExpertBuildAction : BuildAction
    {
        public OperationsExpertBuildAction(Character character, Game game) : base(character, game)
        {
        }

        public override bool CanExecute()
        {
            return !Character.CurrentMapCity.HasResearchStation;
        }

        protected override void AddEffects()
        {
            if (CityToDestroy != null)
            {
                Effects.Add(new DestroyResearchStationEffect(CityToDestroy, Game));
                Effects.Add(new UnselectAllCitiesEffect(Game.WorldMap));
            }

            Effects.Add(new BuildResearchStationEffect(Character.CurrentMapCity, Game));
        }
    }
}