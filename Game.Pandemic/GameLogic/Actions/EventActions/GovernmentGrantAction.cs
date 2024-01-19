using System.Linq;
using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Actions.Selections;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Cards;

namespace Game.Pandemic.GameLogic.Actions.EventActions
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
                new CitySelection(SelectionCallback((MapCity c) => _city = c),
                                                           Game.WorldMap.Cities.Where(x => !x.HasResearchStation), 
                                                           "Select city")
               );
        }
    }
}