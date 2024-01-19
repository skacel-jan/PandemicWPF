using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Game.Pandemic.GameLogic.Cards
{
    public abstract class Card : ObservableObject, IEquatable<Card>
    {
        protected Card(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public abstract int SortCode { get; }

        public bool Equals(Card other)
        {
            return other is not null && Name.Equals(other.Name, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            //avoid double casting
            return obj is Card other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
