namespace Pandemic.GameLogic.Actions
{
    public class OneQuietNightAction : EventAction
    {
        protected override void Execute()
        {
            Game.Infection.Actual = 0;

            FinishAction();
        }
    }
}