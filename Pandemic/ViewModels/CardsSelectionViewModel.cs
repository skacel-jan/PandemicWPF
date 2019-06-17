using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.Cards;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.ViewModels
{
    public class CardsSelectionViewModel<T> : ViewModelBase where T: Card
    {
        private Action<IEnumerable<T>> _selectAction;
        private readonly Func<IEnumerable<T>, bool> _predicate;

        public IEnumerable<T> Items { get; }
        public RelayCommand<IEnumerable<T>> SelectedCommand { get; }

        public CardsSelectionViewModel(Action<IEnumerable<T>> selectAction, IEnumerable<T> items,
            Func<IEnumerable<T>, bool> predicate)
        {
            _selectAction = selectAction;
            Items = items;
            _predicate = predicate;
            SelectedCommand = new RelayCommand<IEnumerable<T>>((card) =>
            {
                if (_predicate(card))
                {
                    _selectAction.Invoke(card);
                }
            });
        }
    }

    public class CardSelectionViewModel : ViewModelBase
    {
        private Action<Card> _selectAction;
        private readonly Func<Card, bool> _predicate;

        public IEnumerable<Card> Items { get; }
        public RelayCommand<Card> SelectedCommand { get; }

        public CardSelectionViewModel(Action<Card> selectAction, IEnumerable<Card> items, Func<Card, bool> predicate)
        {
            _selectAction = selectAction;
            Items = items;
            _predicate = predicate;
            SelectedCommand = new RelayCommand<Card>((card) =>
            {
                if (_predicate(card))
                {
                    _selectAction.Invoke(card);
                }                
            });
        }
    }

    public class CardsViewModel : ViewModelBase
    {
        public CardsViewModel(IEnumerable<Card> items)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public IEnumerable<Card> Items { get; private set; }
    }

    public class InfectionCardsViewModel : ViewModelBase
    {
        public InfectionCardsViewModel(IEnumerable<Card> items)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public IEnumerable<Card> Items { get; private set; }
    }

    public class PlayerCardsViewModel : ViewModelBase
    {
        public PlayerCardsViewModel(IEnumerable<Card> items)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public IEnumerable<Card> Items { get; private set; }
    }
}