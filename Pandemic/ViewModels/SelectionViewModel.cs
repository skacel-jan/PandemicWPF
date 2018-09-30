using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class SelectionViewModel<T> : ViewModelBase
    {
        public IEnumerable<T> Items { get; }
        public Action<T> CallbackAction { get; }
        public ICommand SelectedCommand { get; protected set; }

        public SelectionViewModel(IEnumerable<T> items, Action<T> callbackAction)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            CallbackAction = callbackAction ?? throw new ArgumentNullException(nameof(callbackAction));
            SelectedCommand = new RelayCommand<T>(CallbackAction);
        }
    }
}