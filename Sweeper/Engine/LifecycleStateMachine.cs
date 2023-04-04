namespace Schneider.Sweeper.Engine
{
    internal class LifecycleStateMachine : ILifecycle
    {
        public LifecycleState CurrentState { get; private set; }
        public event EventHandler<LifecycleState>? StateChanged;

        public LifecycleStateMachine()
        {
            CurrentState = LifecycleState.Initializing;
        }

        public void StartGame()
        {
            ChangeStateIfValidOrThrow(LifecycleState.InGame, LifecycleState.Initializing);
        }

        public void Die()
        {
            ChangeStateIfValidOrThrow(LifecycleState.Death, LifecycleState.InGame);
        }

        public void Succeed()
        {
            ChangeStateIfValidOrThrow(LifecycleState.Success, LifecycleState.InGame);
        }

        private void ChangeStateIfValidOrThrow(LifecycleState newState, params LifecycleState[] expectedOldStates)
        {
            if (!expectedOldStates.Contains(CurrentState))
            {
                throw new InvalidOperationException($"Cannot change LifecycleState from '{CurrentState}' to '{newState}'");
            }
            CurrentState = newState;
            StateChanged?.Invoke(this, newState);
        }
    }
}
