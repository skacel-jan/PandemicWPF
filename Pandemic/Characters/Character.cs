﻿using GalaSoft.MvvmLight;
using Pandemic.Cards;
using System;
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
        private DiseaseColor _mostCardsColor;
        private int _mostCardsColorCount;

        protected Character()
        {
            Cards = new ObservableCollection<Card>();
            Cards.CollectionChanged += Cards_CollectionChanged;

            MoveStrategy = new MoveStrategy(this);
        }

        public ShareKnowledgeBehaviour ShareKnowledgeBehaviour { get; set; }
        public BuildBehaviour BuildBehaviour { get; set; }

        public event EventHandler<CardDiscardedEventArgs> CardDiscarded;

        public virtual int ActionsCount { get => STANDARD_ACTIONS_COUNT; }

        public IEnumerable<PlayerCard> CityCards { get => Cards.OfType<PlayerCard>(); }

        public ObservableCollection<Card> Cards { get;  }

        public virtual int CardsForCure { get => STANDARD_CARDS_FOR_CURE; }
        public virtual int CardsLimit { get => STANDARD_CARDS_LIMIT; }

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
            set
            {
                Set(ref _isActive, value);
                CurrentMapCity.CharactersChanged();
            }
        }

        public DiseaseColor MostCardsColor
        {
            get => _mostCardsColor;
        }

        public int MostCardsColorCount
        {
            get => _mostCardsColorCount;
        }

        public MoveStrategy MoveStrategy { get; set; }
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

        public virtual bool CanDiscoverCure()
        {
            return CanDiscoverCure(MostCardsColor);
        }

        public virtual bool CanDiscoverCure(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.HasResearchStation && ColorCardsCount(diseaseColor) >= CardsForCure;
        }

        public virtual bool CanMove(MapCity destinationCity)
        {
            foreach (var action in MoveStrategy.GetPossibleMoves())
            {
                if (action.IsPossible(destinationCity))
                {
                    return true;
                }
            }

            foreach (var action in MoveStrategy.GetPossibleCardMoves())
            {
                if (action.IsPossible(destinationCity))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool CanPreventInfection(MapCity city, DiseaseColor color)
        {
            return false;
        }

        public bool CanTreatDisease()
        {
            return CurrentMapCity.DiseasesToTreat.Count > 0;
        }

        public int ColorCardsCount(DiseaseColor diseaseColor)
        {
            return CityCards.Count(x => x.City.Color == diseaseColor);
        }

        public virtual IEnumerable<MapCity> GetPossibleDestinationCities(IEnumerable<MapCity> cities)
        {
            var canCharterFlight = HasCityCard(CurrentMapCity.City);

            foreach (var city in cities)
            {
                if (canCharterFlight)
                {
                    yield return city;
                }
                else if (city != CurrentMapCity)
                {
                    if (CanMove(city))
                    {
                        yield return city;
                    }
                }
            }
        }

        public virtual IEnumerable<IMoveAction> GetPossibleMoves(MapCity destinationCity)
        {
            return MoveStrategy.GetPossibleMoves();
        }

        public virtual IEnumerable<IMoveCardAction> GetPossibleCardMoves(MapCity destinationCity)
        {
            return MoveStrategy.GetPossibleCardMoves();
        }

        public bool HasCityCard(City city)
        {
            return CityCards.Any(card => card.City == city);
        }

        public virtual bool Move(string moveType, MapCity city)
        {
            return MoveStrategy.GetMoveAction(moveType).Move(city);
        }

        public virtual bool Move(string moveType, MapCity city, PlayerCard card)
        {
            return MoveStrategy.GetCardMoveAction(moveType).Move(city, card);
        }

        public virtual void RegisterSpecialActions(SpecialActions actions)
        { }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
            if (card is EventCard eventCard)
            {
                eventCard.Character = null;
            }
            OnCardDiscarded(new CardDiscardedEventArgs(card));
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
                _mostCardsColor = CityCards.GroupBy(x => x.City.Color).OrderByDescending(gb => gb.Count()).Select(y => y.Key).FirstOrDefault();
                _mostCardsColorCount = ColorCardsCount(_mostCardsColor);
            }
            else
            {
                _mostCardsColor = default;
            }
        }

        private void OnCardDiscarded(CardDiscardedEventArgs e)
        {
            CardDiscarded?.Invoke(this, e);
        }
    }
}