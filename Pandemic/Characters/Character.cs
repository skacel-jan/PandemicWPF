using GalaSoft.MvvmLight;
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
            Cards = new ObservableCollection<PlayerCard>();
            Cards.CollectionChanged += Cards_CollectionChanged;

            MoveFactory = new MoveStrategy(this);
        }

        public ShareKnowledgeBehaviour ShareKnowledgeBehaviour { get; set; }
        public BuildBehaviour BuildBehaviour { get; set; }

        public event EventHandler<CardDiscardedEventArgs> CardDiscarded;

        public virtual int ActionsCount { get => STANDARD_ACTIONS_COUNT; }
        public ObservableCollection<PlayerCard> Cards { get; private set; }
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

        public MoveStrategy MoveFactory { get; set; }
        public abstract string Role { get; }

        public abstract IEnumerable<string> RoleDescription { get; }

        public void AddCard(PlayerCard card)
        {
            Cards.Add(card);
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
            foreach (var action in MoveFactory.GetPossibleMoves())
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
            return Cards.Count(x => x.City.Color == diseaseColor);
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

        public virtual IEnumerable<IMoveAction> GetPossibleMoveTypes(MapCity destinationCity)
        {
            return MoveFactory.GetPossibleMoves();
        }

        public bool HasCityCard(City city)
        {
            return Cards.Any(card => card.City == city);
        }

        public virtual bool Move(string moveType, MapCity city)
        {
            return MoveFactory.GetMoveAction(moveType, null).Move(city);
        }

        public virtual bool Move(string moveType, MapCity city, PlayerCard card)
        {
            return MoveFactory.GetMoveAction(moveType, card).Move(city);
        }


        public virtual void RegisterSpecialActions(SpecialActions actions)
        { }

        public PlayerCard RemoveCard(PlayerCard card)
        {
            Cards.Remove(card);
            OnCardDiscarded(new CardDiscardedEventArgs(card));
            return card;
        }

        public PlayerCard RemoveCard(City city)
        {
            var card = Cards.Single(c => c.City == city);
            return RemoveCard(card as PlayerCard);
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
                _mostCardsColor = Cards.GroupBy(x => x.City.Color).OrderByDescending(gb => gb.Count()).Select(y => y.Key).First();
                _mostCardsColorCount = ColorCardsCount(_mostCardsColor);
            }
            else
            {
                _mostCardsColor = DiseaseColor.Black;
            }
        }

        private void OnCardDiscarded(CardDiscardedEventArgs e)
        {
            CardDiscarded?.Invoke(this, e);
        }
    }
}