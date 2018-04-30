using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pandemic
{
    public abstract class Card : ObservableObject
    {
        public string Name { get; }

        public Card(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public BitmapImage Image { get; protected set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
