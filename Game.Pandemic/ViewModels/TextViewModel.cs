using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Game.Pandemic.ViewModels
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
            set => SetProperty(ref _commandText, value);
        }

        public ICommand ContinueCommand { get; }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
    }
}