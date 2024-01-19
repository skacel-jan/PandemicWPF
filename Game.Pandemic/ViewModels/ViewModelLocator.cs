using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Game.Pandemic.GameLogic;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Characters;
using Game.Pandemic.GameLogic.Decks;
using Game.Pandemic.GameLogic.Services;
using Game.Pandemic.ViewModels.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Pandemic.ViewModels
{
    public class ViewModelLocator : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private readonly Ioc _simpleIoc;

        public ViewModelLocator()
        {
            _simpleIoc = Ioc.Default;

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ViewModelLocator>();
            serviceCollection.AddSingleton<MainMenuViewModel>();
            serviceCollection.AddSingleton<GameSetingsViewModel>();
            serviceCollection.AddSingleton<BoardViewModel>();

            serviceCollection.AddSingleton<DiseaseFactory>();

            if (Application.Current is not App)
            {
                serviceCollection.AddSingleton<IWorldMapFactory, MockWorldMapFactory>();

            }
            else
            {
                serviceCollection.AddSingleton<IWorldMapFactory, XmlWorldMapFactory>();
            }

            serviceCollection.AddSingleton<WorldMap>(s =>
                                    s.GetRequiredService<IWorldMapFactory>()
                                    .CreateWorldMap(s.GetRequiredService<DiseaseFactory>().GetDiseases()));



            serviceCollection.AddSingleton<DeckFactory>();

            serviceCollection.AddSingleton<GameFactory>();

            serviceCollection.AddSingleton<CharacterFactory>();
            serviceCollection.AddSingleton<CharacterActionsFactory>();

            serviceCollection.AddSingleton<GameSettings>();

            serviceCollection.AddSingleton<IDialogService, WindowDialogService>();
            serviceCollection.AddSingleton<SelectionService>();

            serviceCollection.AddSingleton<WorldMapViewModel>();

            _simpleIoc.ConfigureServices(serviceCollection.BuildServiceProvider());


            Messenger.Register<ViewModelLocator, NavigateToViewModelMessage, string>(this, MessageTokens.GameChannel, NavigateTo);

            CurrentViewModel = _simpleIoc.GetRequiredService<MainMenuViewModel>();
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public EventsViewModel EventsViewModel => new EventsViewModel(_simpleIoc.GetRequiredService<DeckFactory>().GetEventCards());

        public BoardViewModel BoardViewModel
        {
            get
            {
                return _simpleIoc.GetRequiredService<BoardViewModel>();
            }
        }

        public MainMenuViewModel MainMenuViewModel
        {
            get
            {
                return _simpleIoc.GetRequiredService<MainMenuViewModel>();
            }
        }

        public GameSetingsViewModel GameSettingsViewModel
        {
            get
            {
                return _simpleIoc.GetRequiredService<GameSetingsViewModel>();
            }
        }

        public WorldMapViewModel WorldMapViewModel
        {
            get
            {
                return _simpleIoc.GetRequiredService<WorldMapViewModel>();
            }
        }

        private void NavigateTo(ViewModelLocator recipient, NavigateToViewModelMessage message)
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
                    CurrentViewModel = GameSettingsViewModel;
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