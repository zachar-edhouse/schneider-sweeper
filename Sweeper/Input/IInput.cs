using Schneider.Sweeper.Engine;

namespace Schneider.Sweeper.Input
{
    public interface IInput
    {
        public event EventHandler<Direction>? OnDirectionInput;
        public event EventHandler<ConsoleKeyInfo>? OnKeyPressed;

        void ReadNext();
    }
}
