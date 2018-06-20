using System.Linq;
using System.Threading.Tasks;

namespace Pandemic.GameLogic.Actions
{
    public class GovernmentGrantAction : EventAction
    {
        protected override void Execute()
        {
            _game.SelectCity(_game.WorldMap.Cities.Values.Where(x => !x.HasResearchStation), SetCity, "Select city");
        }

        private void SetCity(MapCity city)
        {
            city.HasResearchStation = true;

            Task.Run(() =>
            {
                foreach (var mapCity in _game.WorldMap.Cities.Values)
                {
                    mapCity.IsSelectable = false;
                }
            });

            FinishAction();
        }
    }
}