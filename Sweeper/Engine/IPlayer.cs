namespace Schneider.Sweeper.Engine
{
    public interface IPlayer
    {
        uint MoveCount { get; }
        uint LifeCount { get; }
        uint X { get; }
        uint Y { get; }

        void Move(Direction direction);
    }
}
