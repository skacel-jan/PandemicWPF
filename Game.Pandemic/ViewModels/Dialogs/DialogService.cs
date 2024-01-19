using System.Windows;
using Game.Pandemic.Views.Dialogs;

namespace Game.Pandemic.ViewModels.Dialogs
{
    public interface IDialogService
    {
        bool? ShowDialog(string title, object datacontext);
    }

    public class WindowDialogService : IDialogService
    {
        public bool? ShowDialog(string title, object datacontext)
        {
            var win = new WindowDialog
            {
                Title = title,
                DataContext = datacontext,
                Owner = Application.Current.MainWindow
            };

            return win.ShowDialog();
        }
    }
}