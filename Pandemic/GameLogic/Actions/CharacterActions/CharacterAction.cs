using System;

namespace Pandemic.GameLogic.Actions
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