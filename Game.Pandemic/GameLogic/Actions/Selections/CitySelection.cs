using System;
using System.Collections.Generic;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Services;

namespace Game.Pandemic.GameLogic.Actions.Selections
{
    public class CitySelection : Selection
    {
        private readonly Action<MapCity> _selectCityCallback;
        private readonly IEnumerable<MapCity> _cities;

        public CitySelection(Action<MapCity> setCity, IEnumerable<MapCity> cities, string infoText)
        {
            _selectCityCallback = setCity;
            _cities = cities;
            InfoText = infoText;
        }

        public override void Execute(SelectionService service)
        {
            service.SelectCity(_selectCityCallback, _cities, InfoText);
        }
    }
}