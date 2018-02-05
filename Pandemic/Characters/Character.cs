using GalaSoft.MvvmLight;

namespace Pandemic
{
    public abstract class Character : ObservableObject
    {
        private const int STANDARD_ACTIONS_COUNT = 4;
        private const int STANDARD_CARDS_FOR_CURE = 5;

        private MapCity _currentMapCity;
        private bool _isActive;
        private Player _player;
        public virtual int ActionsCount { get => STANDARD_ACTIONS_COUNT; }
        public virtual int CardsForCure { get => STANDARD_CARDS_FOR_CURE; }

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

        public Player Player
        {
            get { return _player; }
            set
            {
                Set(ref _player, value);
            }
        }

        public abstract string Role { get; }

        public virtual PlayerCard BuildResearhStation()
        {
            CurrentMapCity.HasResearchStation = true;
            return Player.RemoveCard(CurrentMapCity.City);
        }

        public virtual bool CanBuildResearchStation()
        {
            return HasCardOfCurrentCity() && !CurrentMapCity.HasResearchStation;
        }

        public virtual bool CanDirectFlight(MapCity toCity)
        {
            return Player.HasCityCard(toCity.City);
        }

        public virtual bool CanDiscoverCure(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.HasResearchStation && Player.ColorCardsCount(diseaseColor) >= CardsForCure;
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
            return Player.RemoveCard(toCity.City);
        }

        public virtual void DriveOrFerry(MapCity toCity)
        {
            CurrentMapCity = toCity;
        }

        public virtual PlayerCard CharterFlight(MapCity toCity)
        {
            var card = Player.RemoveCard(CurrentMapCity.City);
            CurrentMapCity = toCity;
            return card;
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

        public virtual void ShuttleFlight(MapCity toCity)
        {
            CurrentMapCity = toCity;
        }

        public virtual int TreatDisease(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.TreatDisease(diseaseColor);
        }

        internal bool CanTreatDisease()
        {
            return CurrentMapCity.BlackInfection > 0 || CurrentMapCity.BlueInfection > 0 || CurrentMapCity.RedInfection > 0 || CurrentMapCity.YellowInfection > 0;
        }

        protected virtual bool HasCardOfCurrentCity()
        {
            return Player.HasCityCard(CurrentMapCity.City);
        }
    }
}