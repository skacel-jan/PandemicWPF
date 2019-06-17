using Pandemic.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{    
    public class BuildAction : CharacterAction
    {
        private MapCity _cityToDestroy;

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
            if (_cityToDestroy != null)
            {
                Effects.Add(new DestroyResearchStationEffect(_cityToDestroy, Game));
            }

            Effects.Add(new BuildResearchStationEffect(Character.CurrentMapCity, Game));
            Effects.Add(new DiscardPlayerCardEffect(Character.CityCards.First(x => x.City == Character.CurrentMapCity.City), Game.PlayerDiscardPile));
        }

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            _cityToDestroy = null;
            if (Game.ResearchStationsPile == 0)
            {
                yield return new CitySelection(SetSelectionCallback((MapCity c) => _cityToDestroy = c), Game.WorldMap.Cities, "Select city");
            }
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
            base.AddEffects();
            Effects.Remove(Effects.First(x => x is DiscardPlayerCardEffect));
        }
    }
}