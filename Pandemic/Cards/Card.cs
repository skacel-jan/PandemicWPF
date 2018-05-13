using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pandemic.Cards
{
    public abstract class Card : ObservableObject, IComparable, IComparable<Card>, IEquatable<Card>
    {
        public string Name { get; }

        public Card(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(Card other)
        {
            if (Name == other.Name) return 0;
            return Name.CompareTo(other.Name);
        }

        public bool Equals(Card other)
        {
            return Name.Equals(other.Name);
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() != GetType())
                return 1;
            return CompareTo(obj as Card);
        }
    }
}
