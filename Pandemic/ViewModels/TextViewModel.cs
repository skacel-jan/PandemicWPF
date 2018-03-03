using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class TextViewModel : ViewModelBase
    {
        private string _commandText;
        private ICommand _continueCommand;
        private string _text;

        public TextViewModel(string text)
        {
            Text = text;
        }

        public TextViewModel(string text, ICommand continueCommand, string commandText) : this(text)
        {
            ContinueCommand = continueCommand ?? throw new ArgumentNullException(nameof(continueCommand));
            CommandText = commandText ?? throw new ArgumentNullException(nameof(commandText));
        }

        public bool ButtonVisible => !string.IsNullOrEmpty(CommandText);

        public string CommandText
        {
            get => _commandText;
            set => Set(ref _commandText, value);
        }

        public ICommand ContinueCommand
        {
            get => _continueCommand;
            set => Set(ref _continueCommand, value);
        }

        public string Text
        {
            get => _text;
            set => Set(ref _text, value);
        }
    }
}