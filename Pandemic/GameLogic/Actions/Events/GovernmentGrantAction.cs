using System.Linq;
using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class GovernmentGrantAction : EventAction
    {
        public GovernmentGrantAction(EventCard card, Game game) : base(card, game)
        {
        }

        public override void Execute()
        {
            Game.SetInfo("Select city");
            Game.SelectionService.Select(new SelectAction<MapCity>(SelectCity,
                Game.WorldMap.Cities.Where(x => !x.HasResearchStation), "Select city"));
        }

        private void SelectCity(MapCity mapCity)
        {
            mapCity.HasResearchStation = true;

            FinishAction();
        }

        protected override void FinishAction()
        {
            foreach (var mapCity in Game.WorldMap.Cities)
            {
                mapCity.IsSelectable = false;
            }

            base.FinishAction();
        }
    }
}