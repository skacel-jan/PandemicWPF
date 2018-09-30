using GalaSoft.MvvmLight;

namespace Pandemic.Helpers
{
    public class SelectedItemWrapper<T> : ObservableObject
    {
        private bool _isSelected = false;

        public SelectedItemWrapper(T item)
        {
            Item = item;
        }

        /// <summary>
        /// Sets and gets the _isSelected property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected; set => Set(ref _isSelected, value);
        }

        public T Item { get; }
    }
}