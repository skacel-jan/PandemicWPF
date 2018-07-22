using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private ICommand _confirmSelectedEvent;
        private EventCard _selectedEventCard;

        public event EventHandler<EventSelectedEventArgs> EventSelected;

        public EventsViewModel(IEnumerable<EventCard> eventCards, Game game)
        {
            EventCards = eventCards ?? throw new ArgumentNullException(nameof(eventCards));
            Game = game ?? throw new ArgumentNullException(nameof(game));
            SelectedEventCard = EventCards.FirstOrDefault();
        }

        public ICommand ConfirmSelectedEventCommand => _confirmSelectedEvent ?? (_confirmSelectedEvent =
            new RelayCommand(OnEventSelected, () => SelectedEventCard != null));
        public IEnumerable<EventCard> EventCards { get; }

        public Game Game { get; }
        public EventCard SelectedEventCard { get => _selectedEventCard; set => Set(ref _selectedEventCard, value); }

        private void OnEventSelected()
        {
            EventSelected?.Invoke(this, new EventSelectedEventArgs(SelectedEventCard));
        }
    }

    public class EventSelectedEventArgs : EventArgs
    {
        public EventCard EventCard { get; }

        public EventSelectedEventArgs(EventCard eventCard)
        {
            EventCard = eventCard ?? throw new ArgumentNullException(nameof(eventCard));
        }
    }
}