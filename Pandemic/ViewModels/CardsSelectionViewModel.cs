using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.Cards;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.ViewModels
{
    public class CardsSelectionViewModel : ViewModelBase
    {
        private readonly Action<IEnumerable<Card>> _selectAction;
        private readonly Func<IEnumerable<Card>, bool> _predicate;

        public IList<Card> Items { get; }
        public RelayCommand<Card> SelectedCommand { get; }

        private IList<Card> _selectedCards;

        public CardsSelectionViewModel(Action<IEnumerable<Card>> selectAction, IEnumerable<Card> items,
            Func<IEnumerable<Card>, bool> predicate)
        {
            _selectAction = selectAction;
            Items = items.ToList();
            _predicate = predicate;
            _selectedCards = new List<Card>();
            SelectedCommand = new RelayCommand<Card>((card) =>
            {
                if (_selectedCards.Contains(card))
                {
                    _selectedCards.Remove(card);
                }
                else
                {
                    _selectedCards.Add(card);
                }
                if (_predicate(_selectedCards))
                {
                    _selectAction.Invoke(_selectedCards);
                }
            });
        }
    }

    public class CardSelectionViewModel : ViewModelBase
    {
        private readonly Action<Card> _selectAction;
        private readonly Func<Card, bool> _predicate;

        public IList<Card> Items { get; }
        public RelayCommand<Card> SelectedCommand { get; }

        public CardSelectionViewModel(Action<Card> selectAction, IEnumerable<Card> items, Func<Card, bool> predicate)
        {
            _selectAction = selectAction;
            Items = items.OrderBy(x => x.SortRank).ThenBy(x=> x.Name).ToList();
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