using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.ViewModels
{
    public class TextViewModel : ViewModelBase
    {
        public string Text { get; set; }

        public TextViewModel(string text)
        {
            Text = text;
        }
    }
}
