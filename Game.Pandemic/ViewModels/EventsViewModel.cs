using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Game.Pandemic.GameLogic.Cards;

namespace Game.Pandemic.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private ICommand _confirmSelectedEvent;
        private EventCard _selectedEventCard;

        public event EventHandler<EventSelectedEventArgs> EventSelected;

        public EventsViewModel(IEnumerable<EventCard> eventCards)
        {
            EventCards = eventCards?.OrderBy(x => x.Name).ToList() ?? throw new ArgumentNullException(nameof(eventCards));
            SelectedEventCard = EventCards.FirstOrDefault();
        }

        public ICommand ConfirmSelectedEventCommand => _confirmSelectedEvent ?? (_confirmSelectedEvent =
            new RelayCommand(OnEventSelected, () => SelectedEventCard != null));

        public IEnumerable<EventCard> EventCards { get; }

        public EventCard SelectedEventCard
        {
            get => _selectedEventCard;
            set => SetProperty(ref _selectedEventCard, value);
        }

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