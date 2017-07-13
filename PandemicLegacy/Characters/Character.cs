﻿using GalaSoft.MvvmLight;
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
            this.MapCity = toCity;
        }

        public virtual bool CanDirectFlight(MapCity toCity)
        {
            return Player.HasCityCard(toCity.City);
        }

        public virtual void DirectFlight(MapCity toCity)
        {
            this.MapCity = toCity;
            this.Player.RemoveCardWithCity(toCity.City);

        }

        public virtual bool CanCharterFlight()
        {
            return HasCardOfCurrentCity();
        }

        public virtual void CharterFlight(MapCity toCity)
        {

            this.MapCity = toCity;
            this.Player.RemoveCardWithCity(this.MapCity.City);

        }

        public virtual bool CanShuttleFlight(MapCity toCity)
        {
            return this.MapCity.HasResearchStation && toCity.HasResearchStation;
        }

        public virtual void ShuttleFlight(MapCity toCity)
        {
            this.MapCity = toCity;
        }

        public virtual bool CanBuildResearchStation()
        {
            return HasCardOfCurrentCity();
        }

        public virtual void BuildResearhStation()
        {

            this.MapCity.HasResearchStation = true;
            this.Player.RemoveCardWithCity(this.MapCity.City);

        }

        public virtual void TreatDisease(Disease disease)
        {
            this.MapCity.ChangeInfection(disease.Color, -1);
        }

        public virtual bool CanDiscoverCure(Disease disease)
        {
            return this.MapCity.HasResearchStation && this.Player.SameColorCards(disease) >= STANDARD_CARDS_FOR_CURE;
        }

        public virtual void DiscoverCure(Disease disease)
        {

            disease.KnownCure = true;

        }

        public virtual bool CanShareKnowledge(PlayerCard card, Character character)
        {
            return this.MapCity == character.MapCity && this.MapCity.City == card.City;
        }

        public virtual void ShareKnowledgeGive(PlayerCard card, Character character)
        {

            this.Player.RemoveCard(card);
            character.Player.AddCard(card);

        }

        public virtual void ShareKnowledgeTake(PlayerCard card, Character character)
        {

            character.Player.RemoveCard(card);
            this.Player.AddCard(card);

        }

        protected virtual bool HasCardOfCurrentCity()
        {
            return this.Player.HasCityCard(this.MapCity.City);
        }
    }
}
