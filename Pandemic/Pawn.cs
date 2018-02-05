using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic
{
    public class Pawn : ObservableObject
    {
        public Color Color { get; private set; }

        private SolidColorBrush _colorBrush;
        public SolidColorBrush ColorBrush
        {
            get { return _colorBrush; }
            private set { Set(ref _colorBrush, value); }
        }

        public Pawn(Color color)
        {
            this.Color = color;
            this.ColorBrush = new SolidColorBrush(color);
        }
    }
}
