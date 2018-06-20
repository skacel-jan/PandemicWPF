namespace Pandemic.GameLogic.Actions
{
    public class OneQuietNightAction : EventAction
    {
        protected override void Execute()
        {
            _game.Infection.Actual = 0;

            FinishAction();
        }
    }
}