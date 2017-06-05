using PandemicLegacy.Decks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PandemicLegacy.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public ICommand CityCommand { get; set; }

        public Board Board { get; private set; }

        public int BoardInfectionPosition => Board.InfectionPosition;

        public Character CurrentCharacter { get; private set; }

        public GameViewModel()
        {
            var mapFactory = new WorldMapFactory();
            var cities = mapFactory.GetCities();

            Board = new Board(mapFactory.BuildMap(), new InfectionDeck(cities), new PlayerDeck(cities));

            CityCommand = new RelayCommand<MapCity>(param => CityButtonClicked(param), param => IsCityEnabled(param));
        }

        private bool IsCityEnabled(MapCity city)
        {
            return true;
        }

        private void CityButtonClicked(MapCity city)
        {
            var mapCity = city;
        }

        private void MainButtonClick()
        {
            Board.RaiseInfection();
            OnPropertyChanged(nameof(BoardInfectionPosition));
        }
    }
}
