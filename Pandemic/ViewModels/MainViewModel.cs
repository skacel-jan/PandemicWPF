using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Pandemic.Cards;
using Pandemic.Characters;
using Pandemic.Decks;
using Pandemic.GameLogic;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public MainViewModel()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MainMenuViewModel>();
            SimpleIoc.Default.Register<BoardViewModel>();

            SimpleIoc.Default.Register<DiseaseFactory>();
            SimpleIoc.Default.Register(() => SimpleIoc.Default.GetInstance<DiseaseFactory>().GetDiseases());
            SimpleIoc.Default.Register<IWorldMapFactory, XmlWorldMapFactory>();
            SimpleIoc.Default.Register(() => SimpleIoc.Default.GetInstance<IWorldMapFactory>().WorldMap);
            SimpleIoc.Default.Register<EventCardFactory>();
            SimpleIoc.Default.Register(() => SimpleIoc.Default.GetInstance<EventCardFactory>().GetEventCards());
            SimpleIoc.Default.Register<Infection>();
            SimpleIoc.Default.Register<IEnumerable<City>>(() => SimpleIoc.Default.GetInstance<IWorldMapFactory>().Cities);
            SimpleIoc.Default.Register<IDictionary<string, MapCity>>(() => SimpleIoc.Default.GetInstance<IWorldMapFactory>().MapCities);
            SimpleIoc.Default.Register<PlayerDeck>();
            SimpleIoc.Default.Register(() => new Deck<InfectionCard>(SimpleIoc.Default.GetInstance<WorldMap>()
                .Cities.Values.Select(x => new InfectionCard(x.City))));
            SimpleIoc.Default.Register<IEnumerable<Character>>(() =>
            {
                var startingCity = SimpleIoc.Default.GetInstance<IWorldMapFactory>().MapCities[City.Atlanta];
                var characterFactory = new CharacterFactory(startingCity);
                return new List<Character>(
                    new Character[]
                    {
                        characterFactory.GetCharacter(OperationsExpert.ROLE),
                        characterFactory.GetCharacter(Medic.ROLE),
                        characterFactory.GetCharacter(Researcher.ROLE)
                    });
            });
            SimpleIoc.Default.Register(() => new CircularCollection<Character>(SimpleIoc.Default.GetInstance<IEnumerable<Character>>()));

            SimpleIoc.Default.Register<Game>();

            MessengerInstance.Register<NavigateToViewModelMessage>(this, NavigateTo);

            CurrentViewModel = SimpleIoc.Default.GetInstance<MainMenuViewModel>();
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => Set(ref _currentViewModel, value);
        }

        public void SetGameView()
        {
            CurrentViewModel = SimpleIoc.Default.GetInstance<BoardViewModel>();
        }

        private void NavigateTo(NavigateToViewModelMessage message)
        {
            switch (message.NavigateTo)
            {
                case MessageTokens.StartNewGame:
                    SetGameView();
                    break;
            }
        }
    }
}