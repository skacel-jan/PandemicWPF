using System;
using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.CharacterActions
{
    public abstract class CharacterAction : GameActionBase
    {
        public Character Character { get; }

        protected CharacterAction(Character character, Game game) : base(game)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
        }
        protected override void AddEffects()
        {
            Effects.Add(new CharacterActionFinishedEffect(Game));
        }
    }
}