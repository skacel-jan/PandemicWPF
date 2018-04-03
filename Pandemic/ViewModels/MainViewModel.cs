using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Pandemic.Characters;
using Pandemic.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => Set(ref _currentViewModel, value);
        }

        public MainViewModel()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MainMenuViewModel>();
            SimpleIoc.Default.Register<BoardViewModel>();

            SimpleIoc.Default.Register<DiseaseFactory>();
            SimpleIoc.Default.Register(() => SimpleIoc.Default.GetInstance<DiseaseFactory>().GetDiseases());
            SimpleIoc.Default.Register<WorldMapFactory>();
            SimpleIoc.Default.Register(() => SimpleIoc.Default.GetInstance<WorldMapFactory>().GetWorldMap());
            SimpleIoc.Default.Register<IInfectionDeckFactory, InfectionDeckFactory>();
            SimpleIoc.Default.Register<IEventCardFactory, EventCardFactory>();
            SimpleIoc.Default.Register<IGameData, GameData>();
            SimpleIoc.Default.Register<IEnumerable<City>>(() => SimpleIoc.Default.GetInstance<WorldMapFactory>().GetCities());
            SimpleIoc.Default.Register<PlayerDeck>();
            SimpleIoc.Default.Register<SpecialActions>();
            SimpleIoc.Default.Register<TurnStateMachine>();
            SimpleIoc.Default.Register<ActionStateMachine>();
            SimpleIoc.Default.Register<IEnumerable<Character>>(() =>
            {
                return new List<Character>(
                    new Character[]
                    {
                        new Medic()
                        {
                            CurrentMapCity = SimpleIoc.Default.GetInstance<WorldMapFactory>().GetWorldMap().Cities[City.Atlanta]
                        },
                        new OperationsExpert()
                        {
                            CurrentMapCity = SimpleIoc.Default.GetInstance<WorldMapFactory>().GetWorldMap().Cities[City.Atlanta]
                        }
                    });
            });
            SimpleIoc.Default.Register(() => new Queue<Character>(SimpleIoc.Default.GetInstance<IEnumerable<Character>>()));

            SimpleIoc.Default.Register<Board>();

            MessengerInstance.Register<NavigateToViewModelMessage>(this, NavigateTo);


            CurrentViewModel = SimpleIoc.Default.GetInstance<MainMenuViewModel>();
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

        public void SetGameView()
        {
            CurrentViewModel = SimpleIoc.Default.GetInstance<BoardViewModel>();
        }
    }
}
