using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PandemicLegacy.Characters;
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
    public class GameViewModel : ViewModelBase
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
            Board.InfectionDeck.Shuffle();
            Board.PlayerDeck.Shuffle();

            CityCommand = new RelayCommand<MapCity>(param => CityButtonClicked(param), param => IsCityEnabled(param));

            CurrentCharacter = new Medic() { Player = new Player() };
            PlayerCard card = Board.DrawCard();
            if (card != null)
            {
                CurrentCharacter.Player.AddCard(card);
            }

            card = Board.DrawCard();
            if (card != null)
            {
                CurrentCharacter.Player.AddCard(card);
            }
        }

        private bool IsCityEnabled(MapCity city)
        {
            return true;
        }

        private void CityButtonClicked(MapCity city)
        {
            var mapCity = city;

            PlayerCard card = Board.DrawCard();
            if (card != null)
            {
                CurrentCharacter.Player.AddCard(card);
            }
        }
    }
}
