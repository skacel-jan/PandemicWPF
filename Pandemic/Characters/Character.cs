﻿using GalaSoft.MvvmLight;
using Pandemic.Cards;
using Pandemic.GameLogic;
using Pandemic.GameLogic.Actions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace Pandemic
{
    public abstract class Character : ObservableObject
    {
        private const int STANDARD_ACTIONS_COUNT = 4;
        private const int STANDARD_CARDS_FOR_CURE = 5;
        private const int STANDARD_CARDS_LIMIT = 7;

        private MapCity _currentMapCity;
        private bool _isActive;

        protected Character()
        {
            Cards = new ObservableCollection<Card>();
            Cards.CollectionChanged += Cards_CollectionChanged;

            Actions = new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
            {
                { new MoveAction(this) },
                { new TreatAction(this) },
                { new BuildAction(this) },
                { new ShareKnowledgeAction(this) },
                { new DiscoverCureAction(this) }                
            };
        }

        public IDictionary<string, IGameAction> Actions { get; }
        public virtual int ActionsCount { get => STANDARD_ACTIONS_COUNT; }

        public ObservableCollection<Card> Cards { get; }
        public virtual int CardsForCure { get => STANDARD_CARDS_FOR_CURE; }
        public virtual int CardsLimit { get => STANDARD_CARDS_LIMIT; }
        public IEnumerable<PlayerCard> CityCards { get => Cards.OfType<PlayerCard>(); }

        public abstract Color Color { get; }

        public MapCity CurrentMapCity
        {
            get { return _currentMapCity; }
            set
            {
                _currentMapCity?.Characters.Remove(this);
                Set(ref _currentMapCity, value);
                _currentMapCity.Characters.Add(this);
            }
        }

        public bool HasMoreCardsThenLimit
        {
            get => Cards.Count > CardsLimit;
        }

        public bool IsActive
        {
            get => _isActive;
            set => Set(ref _isActive, value);
        }

        public DiseaseColor MostCardsColor { get; private set; }

        public int MostCardsColorCount { get; private set; }

        public abstract string Role { get; }

        public abstract IEnumerable<string> RoleDescription { get; }

        public void AddCard(Card card)
        {
            Cards.Add(card);
            if (card is EventCard eventCard)
            {
                eventCard.Character = this;
            }
        }

        public virtual bool CanPreventInfection(MapCity city, DiseaseColor color)
        {
            return false;
        }

        public int ColorCardsCount(DiseaseColor diseaseColor)
        {
            return CityCards.Count(x => x.City.Color == diseaseColor);
        }

        public bool HasCityCard(City city)
        {
            return CityCards.Any(card => card.City == city);
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        public void RemoveCard(City city)
        {
            var card = CityCards.Single(c => c.City == city);
            RemoveCard(card);
        }

        public virtual int TreatDisease(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.TreatDisease(diseaseColor);
        }

        protected virtual bool HasCardOfCurrentCity()
        {
            return HasCityCard(CurrentMapCity.City);
        }

        private void Cards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Cards.Count > 0)
            {
                MostCardsColor = CityCards.GroupBy(x => x.City.Color).OrderByDescending(gb => gb.Count()).Select(y => y.Key).FirstOrDefault();
                MostCardsColorCount = ColorCardsCount(MostCardsColor);
            }
            else
            {
                MostCardsColor = default;
            }
        }
    }
}