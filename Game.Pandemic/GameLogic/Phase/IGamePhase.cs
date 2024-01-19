namespace Game.Pandemic.GameLogic.Phase
{
    public interface IGamePhase
    {
        void Start();

        void End();

        void Continue();

        Game Game { get; }
    }
}