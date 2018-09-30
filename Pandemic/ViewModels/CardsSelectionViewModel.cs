using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.Cards;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;

namespace Pandemic.ViewModels
{
    public class CardsSelectionViewModel : ViewModelBase
    {
        private ICollection<Card> _selectedCards;
        private IMultiSelectAction<IEnumerable<Card>> _selectAction;

        public IEnumerable<Card> Items { get; private set; }
        public RelayCommand<Card> SelectedCommand { get; }

        public CardsSelectionViewModel(IMultiSelectAction<IEnumerable<Card>> selectAction)
        {
            _selectAction = selectAction;
            Items = _selectAction.Items;
            _selectedCards = new List<Card>();
            SelectedCommand = new RelayCommand<Card>((c) =>
            {
                if (!_selectedCards.Remove(c))
                {
                    _selectedCards.Add(c);
                }
                if (_selectAction.CanExecute(_selectedCards))
                {
                    _selectAction.Execute(_selectedCards);
                }
            });
        }
    }

    public class CardSelectionViewModel : ViewModelBase
    {
        private ISelectAction<Card> _selectAction;

        public IEnumerable<Card> Items { get; }
        public RelayCommand<Card> SelectedCommand { get; }

        public CardSelectionViewModel(ISelectAction<Card> selectAction)
        {
            _selectAction = selectAction;
            Items = _selectAction.Items;
            SelectedCommand = new RelayCommand<Card>((card) =>
            {
                if (_selectAction.CanExecute(card))
                {
                    _selectAction.Execute(card);
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