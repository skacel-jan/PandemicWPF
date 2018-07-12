using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class TextViewModel : ViewModelBase
    {
        private string _commandText;
        private string _text;

        public TextViewModel(string text)
        {
            Text = text;
        }

        public TextViewModel(string text, Action callback, string commandText) : this(text)
        {
            ContinueCommand = new RelayCommand(callback);
            CommandText = commandText ?? throw new ArgumentNullException(nameof(commandText));
        }

        //public bool BackButtonVisible => false;

        public ICommand BackCommand { get; }

        public bool ButtonVisible => !string.IsNullOrEmpty(CommandText);

        public string CommandText
        {
            get => _commandText;
            set => Set(ref _commandText, value);
        }

        public ICommand ContinueCommand { get; }

        public string Text
        {
            get => _text;
            set => Set(ref _text, value);
        }
    }
}