using GalaSoft.MvvmLight;
using System;

namespace Pandemic.Cards
{
    public abstract class Card : ObservableObject, IEquatable<Card>
    {
        protected Card(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public bool Equals(Card other)
        {
            if (other is null)
            {
                return false;
            }

            return Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            Card other = obj as Card; //avoid double casting
            if (other is null)
            {
                return false;
            }
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}