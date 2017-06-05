using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy
{
    public abstract class Character
    {
        private const int STANDARD_ACTIONS_COUNT = 4;
        private const int STANDARD_CARDS_FOR_CURE = 5;

        public Player Player { get; set; }
        public Pawn Pawn { get; set; }
        public virtual int ActionsCount { get { return STANDARD_ACTIONS_COUNT; } }

        public virtual bool CanDriveOrFerry(MapCity toCity)
        {
            return Pawn.MapCity.IsCityConnected(toCity);
        }

        public virtual void DriveOrFerry(MapCity toCity)
        {
            if (CanDriveOrFerry(toCity))
                this.Pawn.MapCity = toCity;
        }

        public virtual bool CanDirectFlight(MapCity toCity)
        {
            return Player.HasCityCard(toCity.City);
        }

        public virtual void DirectFlight(MapCity toCity)
        {
            if (CanDirectFlight(toCity))
            {
                this.Pawn.MapCity = toCity;
                this.Player.RemoveCardWithCity(toCity.City);
            }
        }

        public virtual bool CanCharterFlight()
        {
            return HasCardOfCurrentCity();
        }

        public virtual void CharterFlight(MapCity toCity)
        {
            if (CanCharterFlight())
            {
                this.Pawn.MapCity = toCity;
                this.Player.RemoveCardWithCity(this.Pawn.MapCity.City);
            }
        }

        public virtual bool CanShuttleFlight(MapCity toCity)
        {
            return this.Pawn.MapCity.HasReaserchStation == toCity.HasReaserchStation;
        }

        public virtual void ShuttleFlight(MapCity toCity)
        {
            this.Pawn.MapCity = toCity;
        }

        public virtual bool CanBuildResearchStation()
        {
            return HasCardOfCurrentCity();
        }

        public virtual void BuildResearhStation()
        {
            if (CanBuildResearchStation())
            {
                this.Pawn.MapCity.HasReaserchStation = true;
                this.Player.RemoveCardWithCity(this.Pawn.MapCity.City);
            }
        }

        public virtual void TreatDisease(Disease disease)
        {
            this.Pawn.MapCity.ChangeInfection(disease.Color, -1);
        }

        public virtual bool CanDiscoverCure(Disease disease)
        {
            return this.Pawn.MapCity.HasReaserchStation && this.Player.SameColorCards(disease) >= STANDARD_CARDS_FOR_CURE;
        }

        public virtual void DiscoverCure(Disease disease)
        {
            if (CanDiscoverCure(disease))
            {
                disease.KnownCure = true;
            }                
        }

        public virtual bool CanShareKnowledge(PlayerCard card, Character character)
        {
            return this.Pawn.MapCity == character.Pawn.MapCity && this.Pawn.MapCity.City == card.City;
        }

        public virtual void ShareKnowledgeGive(PlayerCard card, Character character)
        {
            if (CanShareKnowledge(card, character))
            {
                this.Player.RemoveCard(card);
                character.Player.AddCard(card);
            }
        }

        public virtual void ShareKnowledgeTake(PlayerCard card, Character character)
        {
            if (CanShareKnowledge(card, character))
            {
                character.Player.RemoveCard(card);
                this.Player.AddCard(card);
            }
        }

        protected virtual bool HasCardOfCurrentCity()
        {
            return this.Player.HasCityCard(this.Pawn.MapCity.City);
        }
    }
}
