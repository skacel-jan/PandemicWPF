using System.Collections.Generic;
using System.Linq;
using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class GovernmentGrantAction : EventAction
    {
        private MapCity _city;

        public GovernmentGrantAction(EventCard card, Game game) : base(card, game)
        {
            
        }

        protected override void AddEffects()
        {
            base.AddEffects();
            Effects.Add(new UnselectAllCitiesEffect(Game.WorldMap));
            Effects.Add(new BuildResearchStationEffect(_city, Game));
        }

        protected override void Initialize()
        {
            AddSelectionState(0,
                new CitySelection(SetSelectionCallback((MapCity c) => _city = c),
                                                           Game.WorldMap.Cities.Where(x => !x.HasResearchStation), 
                                                           "Select city")
               );
        }
    }
}