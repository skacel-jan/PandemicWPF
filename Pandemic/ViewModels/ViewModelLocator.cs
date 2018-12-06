﻿using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Pandemic.Cards;
using Pandemic.Characters;
using Pandemic.GameLogic;
using Pandemic.ViewModels.Dialogs;
using System.Threading.Tasks;

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
            SimpleIoc.Default.Register<GameSetingsViewModel>();
            SimpleIoc.Default.Register<BoardViewModel>();

            SimpleIoc.Default.Register<DiseaseFactory>();

            if (IsInDesignMode)
            {
                SimpleIoc.Default.Register<IWorldMapFactory, MockWorldMapFactory>();
                if (!SimpleIoc.Default.IsRegistered<WorldMap>())
                {
                    SimpleIoc.Default.Register<WorldMap>(() =>
                                        SimpleIoc.Default.GetInstance<IWorldMapFactory>().CreateWorldMap(SimpleIoc.Default.GetInstance<DiseaseFactory>().GetDiseases()));

                }
            }
            else
            {
                SimpleIoc.Default.Register<IWorldMapFactory, XmlWorldMapFactory>();
            }

            SimpleIoc.Default.Register<EventCardFactory>();

            SimpleIoc.Default.Register<GameFactory>();

            SimpleIoc.Default.Register<CharacterFactory>();
            SimpleIoc.Default.Register<CharacterActionsFactory>();

            SimpleIoc.Default.Register<GameSettings>();

            SimpleIoc.Default.Register<IDialogService, WindowDialogService>();
            SimpleIoc.Default.Register<SelectionService>();

            SimpleIoc.Default.Register<WorldMapViewModel>();


            MessengerInstance.Register<NavigateToViewModelMessage>(this, NavigateTo);

            CurrentViewModel = SimpleIoc.Default.GetInstance<MainMenuViewModel>();
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => Set(ref _currentViewModel, value);
        }

        public EventsViewModel EventsViewModel => new EventsViewModel(SimpleIoc.Default.GetInstance<EventCardFactory>().GetEventCards(), SimpleIoc.Default.GetInstance<Game>());

        public BoardViewModel BoardViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstanceWithoutCaching<BoardViewModel>();
            }
        }

        public MainMenuViewModel MainMenuViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainMenuViewModel>();
            }
        }

        public GameSetingsViewModel GameSetingsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GameSetingsViewModel>();
            }
        }

        public WorldMapViewModel WorldMapViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WorldMapViewModel>();
            }
        }

        private void NavigateTo(NavigateToViewModelMessage message)
        {
            switch (message.NavigateTo)
            {
                case MessageTokens.StartNewGame:
                    CurrentViewModel = BoardViewModel;
                    break;

                case MessageTokens.MainMenu:
                    CurrentViewModel = MainMenuViewModel;
                    break;

                case MessageTokens.NewGameSettings:
                    CurrentViewModel = GameSetingsViewModel;
                    break;

                case MessageTokens.LoadGame:
                    Task.Run(async () =>
                    {
                        var vm = BoardViewModel;
                        await vm.Game.Load();
                        CurrentViewModel = vm;
                    });

                    break;
            }
        }
    }
}