﻿using System;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.Effects
{
    public class MoveCharacterToCityEffect : IEffect
    {
        private readonly Character _character;
        private readonly MapCity _city;

        public MoveCharacterToCityEffect(Character character, MapCity city)
        {
            _character = character ?? throw new ArgumentNullException(nameof(character));
            _city = city ?? throw new ArgumentNullException(nameof(city));
        }

        public void Execute()
        {
            _character.CurrentMapCity = _city;
        }
    }
}