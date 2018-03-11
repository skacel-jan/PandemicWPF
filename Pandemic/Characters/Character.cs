using GalaSoft.MvvmLight;
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
        public virtual int ActionsCount { get => STANDARD_ACTIONS_COUNT; }
        public virtual int CardsForCure { get => STANDARD_CARDS_FOR_CURE; }
        public virtual int CardsLimit { get => STANDARD_CARDS_LIMIT; }

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

        public bool IsActive
        {
            get => _isActive;
            set => Set(ref _isActive, value);
        }

        public abstract string Role { get; }

        public abstract IEnumerable<string> RoleDescription { get; }

        public abstract Color Color { get; }

        public virtual PlayerCard BuildResearhStation()
        {
            CurrentMapCity.HasResearchStation = true;
            return RemoveCard(CurrentMapCity.City);
        }

        public virtual bool CanBuildResearchStation()
        {
            return HasCardOfCurrentCity() && !CurrentMapCity.HasResearchStation;
        }

        public virtual bool CanDirectFlight(MapCity toCity)
        {
            return HasCityCard(toCity.City);
        }

        public virtual bool CanDiscoverCure(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.HasResearchStation && ColorCardsCount(diseaseColor) >= CardsForCure;
        }

        public virtual bool CanDriveOrFerry(MapCity toCity)
        {
            return CurrentMapCity.IsCityConnected(toCity);
        }

        public virtual bool CanCharterFlight()
        {
            return HasCardOfCurrentCity();
        }

        public virtual bool CanShareKnowledge(PlayerCard card, Character character)
        {
            return CurrentMapCity == character.CurrentMapCity && CurrentMapCity.City == card.City;
        }

        public virtual bool CanShuttleFlight(MapCity toCity)
        {
            return CurrentMapCity.HasResearchStation && toCity.HasResearchStation;
        }

        public virtual PlayerCard DirectFlight(MapCity toCity)
        {
            CurrentMapCity = toCity;
            return RemoveCard(toCity.City);
        }

        public virtual void DriveOrFerry(MapCity toCity)
        {
            CurrentMapCity = toCity;
        }

        public virtual PlayerCard CharterFlight(MapCity toCity)
        {
            var card = RemoveCard(CurrentMapCity.City);
            CurrentMapCity = toCity;
            return card;
        }

        public virtual void ShareKnowledgeGive(PlayerCard card, Character character)
        {
            RemoveCard(card);
            character.AddCard(card);
        }

        public virtual void ShareKnowledgeTake(PlayerCard card, Character character)
        {
            character.RemoveCard(card);
            AddCard(card);
        }

        public virtual void ShuttleFlight(MapCity toCity)
        {
            CurrentMapCity = toCity;
        }

        public virtual int TreatDisease(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.TreatDisease(diseaseColor);
        }

        internal int DiseasesToTreat()
        {
            int count = 0;
            foreach (var infection in CurrentMapCity.Infections.Values)
            {
                count += infection > 0 ? 1 : 0;
            }

            return count;
        }

        protected virtual bool HasCardOfCurrentCity()
        {
            return HasCityCard(CurrentMapCity.City);
        }

        public ObservableCollection<PlayerCard> Cards { get; private set; }

        private DiseaseColor _mostCardsColor;

        protected Character()
        {
            Cards = new ObservableCollection<PlayerCard>();
            Cards.CollectionChanged += Cards_CollectionChanged;
        }

        public DiseaseColor MostCardsColor
        {
            get => _mostCardsColor;
        }

        private void Cards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Cards.Count > 0)
            {
                _mostCardsColor = Cards.GroupBy(x => x.City.Color).OrderByDescending(gb => gb.Count()).Select(y => y.Key).First();
            }
            else
            {
                _mostCardsColor = DiseaseColor.Black;
            }
        }

        public PlayerCard RemoveCard(PlayerCard card)
        {
            Cards.Remove(card);
            return card;
        }

        public PlayerCard RemoveCard(City city)
        {
            var card = Cards.Single(c => c.City == city);
            return RemoveCard(card as PlayerCard);
        }

        public void AddCard(PlayerCard card)
        {
            Cards.Add(card);
        }

        public int ColorCardsCount(DiseaseColor diseaseColor)
        {
            return Cards.Count(x => x.City.Color == diseaseColor);
        }

        public bool HasCityCard(City city)
        {
            return Cards.Any(card => card.City == city);
        }

        public bool HasMoreCardsThenLimit
        {
            get => Cards.Count > CardsLimit;
        }
    }
}