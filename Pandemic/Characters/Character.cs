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
        private DiseaseColor _mostCardsColor;
        private int _mostCardsColorCount;

        protected Character()
        {
            Cards = new ObservableCollection<PlayerCard>();
            Cards.CollectionChanged += Cards_CollectionChanged;
        }

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
            set => Set(ref _isActive, value);
        }

        public DiseaseColor MostCardsColor
        {
            get => _mostCardsColor;
        }

        public int MostCardsColorCount
        {
            get => _mostCardsColorCount;
        }

        public abstract string Role { get; }

        public abstract IEnumerable<string> RoleDescription { get; }

        public void AddCard(PlayerCard card)
        {
            Cards.Add(card);
        }

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

        public virtual bool CanDiscoverCure()
        {
            return CanDiscoverCure(MostCardsColor);
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

        public virtual bool CanRaiseInfection(MapCity city, DiseaseColor color)
        {
            return true;
        }

        public virtual bool CanShareKnowledge()
        {
            return (CurrentMapCity.Characters.Count() > 1 && CurrentMapCity.Characters.Any(c => c.HasCityCard(CurrentMapCity.City)));
        }

        public virtual bool CanShuttleFlight(MapCity toCity)
        {
            return CurrentMapCity.HasResearchStation && toCity.HasResearchStation;
        }

        public bool CanTreatDisease()
        {
            return CurrentMapCity.DiseasesToTreat.Count > 0;
        }

        public int ColorCardsCount(DiseaseColor diseaseColor)
        {
            return Cards.Count(x => x.City.Color == diseaseColor);
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

        public bool HasCityCard(City city)
        {
            return Cards.Any(card => card.City == city);
        }

        public virtual PlayerCard CharterFlight(MapCity toCity)
        {
            var card = RemoveCard(CurrentMapCity.City);
            CurrentMapCity = toCity;
            return card;
        }

        public virtual void RegisterSpecialActions(SpecialActions actions)
        { }

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
    }
}