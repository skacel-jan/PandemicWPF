using GalaSoft.MvvmLight;
using System;

namespace Pandemic.Cards
{
    public abstract class Card : ObservableObject, IComparable, IComparable<Card>, IEquatable<Card>
    {
        protected Card(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public static int Compare(Card left, Card right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }
            if (left is null)
            {
                return -1;
            }
            return left.CompareTo(right);
        }

        public static bool operator !=(Card left, Card right)
        {
            return !(left == right);
        }

        public static bool operator <(Card left, Card right)
        {
            return (Compare(left, right) < 0);
        }

        public static bool operator ==(Card left, Card right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator >(Card left, Card right)
        {
            return (Compare(left, right) > 0);
        }

        public int CompareTo(Card other)
        {
            if (other is null)
            {
                return 1;
            }

            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() != GetType())
                return 1;
            return CompareTo(obj as Card);
        }

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
            return CompareTo(other) == 0;
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