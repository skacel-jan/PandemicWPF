﻿using System;
using Game.Pandemic.GameLogic.Board;

namespace Game.Pandemic.GameLogic.Cards
{
    public class CityCard : PlayerCard
    {
        public City City { get; private set; }

        public override int SortCode => 2;

        public CityCard(City city) : base(city.Name)
        {
            City = city ?? throw new ArgumentNullException(nameof(city));
        }

        //public int CompareTo(CityCard other)
        //{
        //    if (City == other.City) return 0;
        //    return City.CompareTo(other.City);
        //}

        //public bool Equals(CityCard other)
        //{
        //    return City.Equals(other.City);
        //}

        //int IComparable.CompareTo(object obj)
        //{
        //    if (obj.GetType() != GetType())
        //        return -1;
        //    return CompareTo(obj as CityCard);
        //}
    }
}