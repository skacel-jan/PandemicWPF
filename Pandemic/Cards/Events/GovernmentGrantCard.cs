using Pandemic.GameLogic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pandemic.Cards
{
    public class GovernmentGrantCard : EventCard
    {
        public GovernmentGrantCard(CitySelectionService citySelectionService) : base("Government Grant")
        {
            CitySelectionService = citySelectionService ?? throw new ArgumentNullException(nameof(citySelectionService));
        }

        public CitySelectionService CitySelectionService { get; }

        public override void PlayEvent()
        {
            foreach (var city in CitySelectionService.Cities.Where(x => !x.HasResearchStation))
            {
                city.IsSelectable = true;
            }

            var action = new Action<MapCity>((MapCity mapCity) =>
            {
                mapCity.HasResearchStation = true;
                Character.RemoveCard(this);

                Task.Run(() =>
                {
                    foreach (var city in CitySelectionService.Cities)
                    {
                        city.IsSelectable = false;
                    }
                });

                OnEventFinished(EventArgs.Empty);
            });

            CitySelectionService.SelectCity("Select city", action);
        }
    }
}