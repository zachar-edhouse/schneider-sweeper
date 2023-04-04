using Schneider.Sweeper.Engine;

namespace Schneider.Sweeper.Input
{
    internal class Input : IInput
    {
        public event EventHandler<Direction>? OnDirectionInput;
        public event EventHandler<ConsoleKeyInfo>? OnKeyPressed;

        private IRawInput input;

        public Input(IRawInput? input = null)
        {
            this.input = input ?? new ConsoleInput();
        }

        public void ReadNext()
        {
            var key = input.ReadKey();
            OnKeyPressed?.Invoke(this, key);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    OnDirectionInput?.Invoke(this, Direction.Up);
                    break;
                case ConsoleKey.DownArrow:
                    OnDirectionInput?.Invoke(this, Direction.Down);
                    break;
                case ConsoleKey.LeftArrow:
                    OnDirectionInput?.Invoke(this, Direction.Left);
                    break;
                case ConsoleKey.RightArrow:
                    OnDirectionInput?.Invoke(this, Direction.Right);
                    break;
                default:
                    break;
            }
        }
    }
}
