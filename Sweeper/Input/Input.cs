using Schneider.Sweeper.Engine;

namespace Schneider.Sweeper.Input
{
    internal class Input : IInput
    {
        private IRawInput input;

        public event EventHandler<Direction>? OnDirectionInput;
        public event EventHandler? OnKeyPressed;
        public event EventHandler? OnEscapePressed;

        public Input(IRawInput? input = null)
        {
            this.input = input ?? new ConsoleInput();
        }

        public void ReadNext()
        {
            var key = input.ReadKey();
            OnKeyPressed?.Invoke(this, new EventArgs());

            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    OnEscapePressed?.Invoke(this, new EventArgs());
                    break;
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
