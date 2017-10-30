using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy
{
    public abstract class Character : ObservableObject
    {
        private const int STANDARD_ACTIONS_COUNT = 4;
        private const int STANDARD_CARDS_FOR_CURE = 5;

        public virtual int ActionsCount { get => STANDARD_ACTIONS_COUNT; }
        public virtual int CardsForCure { get => STANDARD_CARDS_FOR_CURE; } 

        private Player _player;
        public Player Player
        {
            get { return _player; }
            set
            {
                Set(ref _player, value);
            }
        }

        private MapCity _currentMapCity;
        public MapCity CurrentMapCity
        {
            get { return _currentMapCity; }
            set
            {
                _currentMapCity?.Pawns.Remove(Player.Pawn);
                Set(ref _currentMapCity, value);
                _currentMapCity.Pawns.Add(Player.Pawn);
            }
        }

        public virtual bool CanDriveOrFerry(MapCity toCity)
        {
            return CurrentMapCity.IsCityConnected(toCity);
        }

        public virtual void DriveOrFerry(MapCity toCity)
        {
            CurrentMapCity = toCity;
        }

        public virtual bool CanDirectFlight(MapCity toCity)
        {
            return Player.HasCityCard(toCity.City);
        }

        public virtual PlayerCard DirectFlight(MapCity toCity)
        {
            CurrentMapCity = toCity;
            return Player.RemoveCard(toCity.City);
        }

        public virtual bool CanCharterFlight()
        {
            return HasCardOfCurrentCity();
        }

        public virtual PlayerCard CharterFlight(MapCity toCity)
        {
            var card = Player.RemoveCard(CurrentMapCity.City);
            CurrentMapCity = toCity;
            return card;
        }

        public virtual bool CanShuttleFlight(MapCity toCity)
        {
            return CurrentMapCity.HasResearchStation && toCity.HasResearchStation;
        }

        public virtual void ShuttleFlight(MapCity toCity)
        {
            CurrentMapCity = toCity;
        }

        public virtual bool CanBuildResearchStation()
        {
            return HasCardOfCurrentCity() && !CurrentMapCity.HasResearchStation;
        }

        public virtual PlayerCard BuildResearhStation()
        {
            CurrentMapCity.HasResearchStation = true;
            return Player.RemoveCard(CurrentMapCity.City);
        }

        internal bool CanTreatDisease()
        {
            return CurrentMapCity.BlackInfection > 0 || CurrentMapCity.BlueInfection > 0 || CurrentMapCity.RedInfection > 0 || CurrentMapCity.YellowInfection > 0;
        }

        public virtual int TreatDisease(Disease disease)
        {
            CurrentMapCity.ChangeInfection(disease.Color, -1);
            return 1;
        }

        public virtual bool CanDiscoverCure(Disease disease)
        {
            return CurrentMapCity.HasResearchStation && Player.SameColorCards(disease) >= CardsForCure;
        }

        public virtual void DiscoverCure(Disease disease)
        {
            disease.IsCured = true;
        }

        public virtual bool CanShareKnowledge(PlayerCard card, Character character)
        {
            return CurrentMapCity == character.CurrentMapCity && CurrentMapCity.City == card.City;
        }

        public virtual void ShareKnowledgeGive(PlayerCard card, Character character)
        {

            Player.RemoveCard(card);
            character.Player.AddCard(card);
        }

        public virtual void ShareKnowledgeTake(PlayerCard card, Character character)
        {

            character.Player.RemoveCard(card);
            Player.AddCard(card);
        }

        protected virtual bool HasCardOfCurrentCity()
        {
            return Player.HasCityCard(CurrentMapCity.City);
        }
    }
}
