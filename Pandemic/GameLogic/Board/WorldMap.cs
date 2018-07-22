using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandemic
{
    public class WorldMap
    {
        private Action<MapCity> CitySelectedAction;

        public WorldMap(IDictionary<string, MapCity> cities)
        {
            Cities = cities ?? throw new ArgumentNullException(nameof(cities));

            foreach (var city in Cities.Values)
            {
                city.CitySelected += (s, e) =>
                {
                    CitySelectedAction?.Invoke(s as MapCity);
                    CitySelectedAction = null;
                    OnCitySelected(s, e);
                };

                city.CityDoubleClicked += City_CityDoubleClicked;
            }
        }

        private void City_CityDoubleClicked(object sender, EventArgs e)
        {
            OnCityDoubleClicked(sender, e);
        }

        public event EventHandler CitySelected;

        public event EventHandler<CitySelectingEventArgs> CitySelecting;

        public event EventHandler CityDoubleClicked;

        public IDictionary<string, MapCity> Cities { get; }

        public MapCity GetCity(string city)
        {
            return Cities[city];
        }

        public void SelectCity(IEnumerable<MapCity> cities, Action<MapCity> action, string text)
        {
            foreach (var city in cities)
            {
                city.IsSelectable = true;
            }

            CitySelectedAction = action;
            OnCitySelecting(new CitySelectingEventArgs(text));
        }

        protected virtual void OnCitySelected(object sender, EventArgs e)
        {
            CitySelected?.Invoke(sender, e);
        }

        protected virtual void OnCitySelecting(CitySelectingEventArgs e)
        {
            CitySelecting?.Invoke(this, e);
        }

        protected virtual void OnCityDoubleClicked(object sender, EventArgs e)
        {
            CityDoubleClicked?.Invoke(sender, e);
        }
    }
}