using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic
{
    public class CitySelectionService
    {
        public CitySelectionService(IEnumerable<MapCity> cities)
        {
            Cities = cities ?? throw new ArgumentNullException(nameof(cities));

            foreach (var city in Cities)
            {
                city.CitySelected += (s, e) =>
                {
                    CitySelectedAction?.Invoke(s as MapCity);
                    CitySelectedAction = null;
                    OnCitySelected(s, e);
                };
            }
        }

        public IEnumerable<MapCity> Cities { get; }

        private Action<MapCity> CitySelectedAction;

        public event EventHandler<CitySelectingEventArgs> CitySelecting;
        public event EventHandler CitySelected;

        public void SelectCity(IEnumerable<MapCity> cities, Action<MapCity> action, string text)
        {
            Task.Run(() =>
            {
                foreach (var city in cities)
                {
                    city.IsSelectable = true;
                }
            });

            CitySelectedAction = action;
            OnCitySelecting(new CitySelectingEventArgs(text));
        }

        protected virtual void OnCitySelecting(CitySelectingEventArgs e)
        {
            CitySelecting?.Invoke(this, e);
        }

        protected virtual void OnCitySelected(object sender, EventArgs e)
        {
            CitySelected?.Invoke(sender, e);
        }
    }
}
