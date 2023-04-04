namespace Schneider.Sweeper.Engine
{
    public interface ILifecycle
    {
        LifecycleState CurrentState { get; }
        event EventHandler<LifecycleState>? StateChanged;

        void StartGame();
        void Die();
        void Succeed();
    }
}
