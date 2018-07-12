using System.Linq;
using System.Threading.Tasks;

namespace Pandemic.GameLogic.Actions
{
    public class GovernmentGrantAction : EventAction
    {
        protected override void Execute()
        {
            Game.SelectCity(Game.WorldMap.Cities.Values.Where(x => !x.HasResearchStation), SetCity, "Select city");
        }

        private void SetCity(MapCity city)
        {
            city.HasResearchStation = true;            

            FinishAction();
        }

        protected override void FinishAction()
        {
            foreach (var mapCity in Game.WorldMap.Cities.Values)
            {
                mapCity.IsSelectable = false;
            }

            base.FinishAction();
        }

    }
}