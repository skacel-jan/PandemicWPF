using Pandemic.Cards;
using Pandemic.GameLogic.Actions;
using Pandemic.ViewModels;
using System;
using System.Collections.Generic;

namespace Pandemic
{
    public class SelectActionWrapper<T> : ISelectAction<T>
    {
        public SelectActionWrapper(ISelectAction<T> selectAction, Action callback)
        {
            SelectAction = selectAction ?? throw new ArgumentNullException(nameof(selectAction));
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public IEnumerable<T> Items => SelectAction.Items;
        public ISelectAction<T> SelectAction { get; }
        public Action Callback { get; }

        public string Text => SelectAction.Text;

        public bool CanExecute(object param)
        {
            return SelectAction.CanExecute(param);
        }

        public void Execute(object param)
        {
            Callback();
            SelectAction.Execute(param);            
        }
    }

    public class MultiSelectActionWrapper<T> : IMultiSelectAction<T>
    {
        public MultiSelectActionWrapper(IMultiSelectAction<T> selectAction, Action callback)
        {
            SelectAction = selectAction ?? throw new ArgumentNullException(nameof(selectAction));
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public T Items => SelectAction.Items;
        public IMultiSelectAction<T> SelectAction { get; }
        public Action Callback { get; }

        public string Text => SelectAction.Text;

        public bool CanExecute(object param)
        {
            return SelectAction.CanExecute(param);
        }

        public void Execute(object param)
        {
            SelectAction.Execute(param);
            Callback();
        }

    }

    public class SelectionService
    {
        public SelectionService(WorldMap worldMap)
        {
            WorldMap = worldMap ?? throw new ArgumentNullException(nameof(worldMap));
        }

        public event EventHandler<ViewModelEventArgs> Selecting;

        public event EventHandler SelectionFinished;

        public WorldMap WorldMap { get; }

        public void Select(ISelectAction<Card> selectAction)
        {
            var wrapper = new SelectActionWrapper<Card>(selectAction, ActionFinishedCallback);

            var viewModel = new CardSelectionViewModel(wrapper);
            Selecting?.Invoke(this, new ViewModelEventArgs(viewModel));
        }

        private void ActionFinishedCallback()
        {
            OnSelectionFinished(EventArgs.Empty);
        }

        public void Select(IMultiSelectAction<IEnumerable<PlayerCard>> selectAction)
        {
            var wrapper = new MultiSelectActionWrapper<IEnumerable<PlayerCard>>(selectAction, ActionFinishedCallback);

            var viewModel = new CardsSelectionViewModel(selectAction);

            Selecting?.Invoke(this, new ViewModelEventArgs(viewModel));
        }

        public void Select(ISelectAction<Character> selectAction)
        {
            var wrapper = new SelectActionWrapper<Character>(selectAction, ActionFinishedCallback);

            var viewModel = new CharacterSelectionViewModel(wrapper);
            Selecting?.Invoke(this, new ViewModelEventArgs(viewModel));
        }

        public void Select(ISelectAction<MapCity> selectAction)
        {
            WorldMap.SelectCity(selectAction);
        }

        protected void OnSelectionFinished(EventArgs e)
        {
            SelectionFinished?.Invoke(this, e);
        }
    }
}