using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Pandemic.Cards;
using Pandemic.Characters;
using Pandemic.GameLogic;
using Pandemic.ViewModels.Dialogs;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Pandemic.ViewModels
{
    public class ViewModelLocator : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        SimpleIoc _simpleIoc;

        public ViewModelLocator()
        {
            _simpleIoc = SimpleIoc.Default;

            _simpleIoc.Register<ViewModelLocator>();
            _simpleIoc.Register<MainMenuViewModel>();
            _simpleIoc.Register<GameSetingsViewModel>();
            _simpleIoc.Register<BoardViewModel>();

            _simpleIoc.Register<DiseaseFactory>();

            if (!(Application.Current is App))
            {                
                _simpleIoc.Register<IWorldMapFactory, MockWorldMapFactory>();
                if (!_simpleIoc.IsRegistered<WorldMap>())
                {
                    _simpleIoc.Register<WorldMap>(() =>
                                        _simpleIoc.GetInstance<IWorldMapFactory>().CreateWorldMap(_simpleIoc.GetInstance<DiseaseFactory>().GetDiseases()));

                }
            }
            else
            {
                _simpleIoc.Register<IWorldMapFactory, XmlWorldMapFactory>();
            }

            _simpleIoc.Register<EventCardFactory>();

            _simpleIoc.Register<GameFactory>();

            _simpleIoc.Register<CharacterFactory>();
            _simpleIoc.Register<CharacterActionsFactory>();

            _simpleIoc.Register<GameSettings>();

            _simpleIoc.Register<IDialogService, WindowDialogService>();
            _simpleIoc.Register<SelectionService>();

            _simpleIoc.Register<WorldMapViewModel>();


            MessengerInstance.Register<NavigateToViewModelMessage>(this, NavigateTo);

            CurrentViewModel = _simpleIoc.GetInstance<MainMenuViewModel>();
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => Set(ref _currentViewModel, value);
        }

        public EventsViewModel EventsViewModel => new EventsViewModel(_simpleIoc.GetInstance<EventCardFactory>().GetEventCards(), _simpleIoc.GetInstance<Game>());

        public BoardViewModel BoardViewModel
        {
            get
            {
                return _simpleIoc.GetInstanceWithoutCaching<BoardViewModel>();
            }
        }

        public MainMenuViewModel MainMenuViewModel
        {
            get
            {
                return _simpleIoc.GetInstance<MainMenuViewModel>();
            }
        }

        public GameSetingsViewModel GameSetingsViewModel
        {
            get
            {
                return _simpleIoc.GetInstance<GameSetingsViewModel>();
            }
        }

        public WorldMapViewModel WorldMapViewModel
        {
            get
            {
                return _simpleIoc.GetInstance<WorldMapViewModel>();
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