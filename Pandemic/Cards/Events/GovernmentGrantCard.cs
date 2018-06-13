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

        public override void PlayEvent(Game game)
        {
            var action = new Action<MapCity>((MapCity mapCity) =>
            {
                mapCity.HasResearchStation = true;

                Task.Run(() =>
                {
                    foreach (var city in CitySelectionService.Cities)
                    {
                        city.IsSelectable = false;
                    }
                });

                OnEventFinished(EventArgs.Empty, game);
            });

            CitySelectionService.SelectCity(CitySelectionService.Cities.Where(x => !x.HasResearchStation), action, "Select city");
        }
    }
}