﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.Cards
{
    public class PlayerCard : CityCard, IComparable
    {
        public PlayerCard(City city) : base(city)
        {
        }
    }
}
