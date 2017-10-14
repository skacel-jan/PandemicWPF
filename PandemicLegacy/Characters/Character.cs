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

        private Player _player;
        public Player Player
        {
            get { return _player; }
            set
            {
                Set(ref _player, value);
            }
        }

        private MapCity _mapCity;
        public MapCity MapCity
        {
            get { return _mapCity; }
            set
            {
                _mapCity?.Pawns.Remove(Player.Pawn);
                Set(ref _mapCity, value);
                _mapCity.Pawns.Add(Player.Pawn);
            }
        }

        public virtual int ActionsCount { get { return STANDARD_ACTIONS_COUNT; } }

        public virtual bool CanDriveOrFerry(MapCity toCity)
        {
            return MapCity.IsCityConnected(toCity);
        }

        public virtual void DriveOrFerry(MapCity toCity)
        {
            MapCity = toCity;
        }

        public virtual bool CanDirectFlight(MapCity toCity)
        {
            return Player.HasCityCard(toCity.City);
        }

        public virtual void DirectFlight(MapCity toCity)
        {
            MapCity = toCity;
            Player.RemoveCardWithCity(toCity.City);
        }

        public virtual bool CanCharterFlight()
        {
            return HasCardOfCurrentCity();
        }

        public virtual void CharterFlight(MapCity toCity)
        {
            Player.RemoveCardWithCity(MapCity.City);
            MapCity = toCity;
        }

        public virtual bool CanShuttleFlight(MapCity toCity)
        {
            return MapCity.HasResearchStation && toCity.HasResearchStation;
        }

        public virtual void ShuttleFlight(MapCity toCity)
        {
            MapCity = toCity;
        }

        public virtual bool CanBuildResearchStation()
        {
            return HasCardOfCurrentCity() && !MapCity.HasResearchStation;
        }

        public virtual void BuildResearhStation()
        {
            MapCity.HasResearchStation = true;
            Player.RemoveCardWithCity(MapCity.City);
        }

        internal bool CanTreatDisease()
        {
            return MapCity.BlackInfection > 0 || MapCity.BlueInfection > 0 || MapCity.RedInfection > 0 || MapCity.YellowInfection > 0;
        }

        public virtual void TreatDisease(Disease disease)
        {
            MapCity.ChangeInfection(disease.Color, -1);
        }

        public virtual bool CanDiscoverCure(Disease disease)
        {
            return MapCity.HasResearchStation && Player.SameColorCards(disease) >= STANDARD_CARDS_FOR_CURE;
        }

        public virtual void DiscoverCure(Disease disease)
        {

            disease.KnownCure = true;
        }

        public virtual bool CanShareKnowledge(PlayerCard card, Character character)
        {
            return MapCity == character.MapCity && MapCity.City == card.City;
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
            return Player.HasCityCard(MapCity.City);
        }
    }
}
