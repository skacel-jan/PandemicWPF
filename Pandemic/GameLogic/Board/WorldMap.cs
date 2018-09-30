using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic
{
    public class WorldMap
    {
        private ISelectAction<MapCity> _selectAction;
        private IDictionary<string, MapCity> _cities;

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
                    if (_selectAction != null)
                    {
                        _selectAction.Execute((MapCity)s);
                        OnCitySelected(s, e);
                    }                    
                };

                city.CityDoubleClicked += OnCityDoubleClicked;
            }
        }

        public event EventHandler CityDoubleClicked;

        public event EventHandler CitySelected;

        public event EventHandler<CitySelectingEventArgs> CitySelecting;

        public IEnumerable<MapCity> Cities => _cities.Values;

        public MapCity GetCity(string city)
        {
            return _cities[city];
        }

        public void SelectCity(ISelectAction<MapCity> selectAction)
        {
            foreach (var city in selectAction.Items)
            {
                city.IsSelectable = true;
            }
            _selectAction = selectAction;

            OnCitySelecting(new CitySelectingEventArgs(selectAction.Text));
        }

        protected void OnCityDoubleClicked(object sender, EventArgs e)
        {
            CityDoubleClicked?.Invoke(sender, e);
        }

        protected virtual void OnCitySelected(object sender, EventArgs e)
        {
            CitySelected?.Invoke(sender, e);
        }

        protected virtual void OnCitySelecting(CitySelectingEventArgs e)
        {
            CitySelecting?.Invoke(this, e);
        }
    }
}