using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public abstract class Card : ObservableObject
    {
        public string Name { get; private set; }

        public Card(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
