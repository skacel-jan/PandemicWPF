using System;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Phase;
using Game.Pandemic.ViewModels;

namespace Game.Pandemic.GameLogic
{
    public class OutbreakEventArgs : EventArgs
    {
        public OutbreakEventArgs(City city)
        {
            City = city;
        }

        public City City { get; }
    }

    public enum ShareType
    {
        Give,
        Take
    }

    public class GamePhaseChangedEventArgs : EventArgs
    {
        public GamePhaseChangedEventArgs(IGamePhase previousPhase, IGamePhase actualPhase)
        {
            PreviousPhase = previousPhase;
            ActualPhase = actualPhase ?? throw new ArgumentNullException(nameof(actualPhase));
        }

        public IGamePhase PreviousPhase { get; }
        public IGamePhase ActualPhase { get; }
    }

    public class ViewModelEventArgs : EventArgs
    {
        public ViewModelEventArgs(ViewModelBase viewModel)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        public ViewModelBase ViewModel { get; }
    }
}