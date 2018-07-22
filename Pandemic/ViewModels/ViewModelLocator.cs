using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Pandemic.Cards;
using Pandemic.GameLogic;
using System.Collections.Generic;

namespace Pandemic.ViewModels
{
    public class ViewModelLocator : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ViewModelLocator>();
            SimpleIoc.Default.Register<MainMenuViewModel>();
            SimpleIoc.Default.Register<BoardViewModel>();

            SimpleIoc.Default.Register<DiseaseFactory>();

            if (IsInDesignMode)
            {
                SimpleIoc.Default.Register<IWorldMapFactory, MockWorldMapFactory>();
            }
            else
            {
                SimpleIoc.Default.Register<IWorldMapFactory, XmlWorldMapFactory>();
            }

            SimpleIoc.Default.Register<EventCardFactory>();

            if (!SimpleIoc.Default.IsRegistered<Game>())
            {
                SimpleIoc.Default.Register<GameFactory>();
                SimpleIoc.Default.Register<Game>(() => SimpleIoc.Default.GetInstanceWithoutCaching<GameFactory>().CreateGame());
            }

            MessengerInstance.Register<NavigateToViewModelMessage>(this, NavigateTo);

            CurrentViewModel = SimpleIoc.Default.GetInstance<MainMenuViewModel>();
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => Set(ref _currentViewModel, value);
        }

        public EventsViewModel EventsViewModel => new EventsViewModel(SimpleIoc.Default.GetInstance<EventCardFactory>().GetEventCards(), SimpleIoc.Default.GetInstance<Game>());

        public BoardViewModel Game
        {
            get
            {
                return SimpleIoc.Default.GetInstanceWithoutCaching<BoardViewModel>();
            }
        }

        public MainMenuViewModel MainMenu
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainMenuViewModel>();
            }
        }

        private void NavigateTo(NavigateToViewModelMessage message)
        {
            switch (message.NavigateTo)
            {
                case MessageTokens.StartNewGame:
                    CurrentViewModel = SimpleIoc.Default.GetInstanceWithoutCaching<BoardViewModel>();
                    break;

                case MessageTokens.EndGame:
                    CurrentViewModel = SimpleIoc.Default.GetInstance<MainMenuViewModel>();
                    break;
            }
        }
    }
}