using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic
{
    public class WorldMap
    {
        private IDictionary<string, MapCity> _cities;
        private Action<MapCity> _selectionCallback;

        public WorldMap(IEnumerable<MapCity> cities)
        {
            if (cities == null)
            {
                throw new ArgumentNullException(nameof(cities));
            }

            _cities = cities.ToDictionary(c => c.City.Name, C => C);

            foreach (var city in Cities)
            {
                city.CitySelected += (s, e) =>
                {
                    if (_selectionCallback != null)
                    {
                        _selectionCallback.Invoke((MapCity)s);
                        OnCitySelected(s, e);
                    }                    
                };

                city.CityDoubleClicked += OnCityDoubleClicked;
            }
        }

        public event EventHandler CityDoubleClicked;

        public event EventHandler CitySelected;

        public IEnumerable<MapCity> Cities => _cities.Values;

        public MapCity this[string city] => _cities[city];

        public void SelectCity(Action<MapCity> selectionCallback)
        {
            _selectionCallback = selectionCallback;
        }

        protected void OnCityDoubleClicked(object sender, EventArgs e)
        {
            CityDoubleClicked?.Invoke(sender, e);
        }

        protected virtual void OnCitySelected(object sender, EventArgs e)
        {
            CitySelected?.Invoke(sender, e);
        }
    }
}