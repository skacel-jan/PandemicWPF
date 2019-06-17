namespace Pandemic.GameLogic.Actions
{
    internal class CharacterActionFinishedEffect : IEffect
    {
        private Game _game;

        public CharacterActionFinishedEffect(Game game)
        {
            _game = game;
        }

        public void Execute()
        {
            _game.Actions--;
        }
    }
}