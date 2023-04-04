namespace Schneider.Sweeper.Visualization
{
    internal class ConsoleOutput : IRawOutput
    {
        private ConsoleColor foregroundBackup;
        private ConsoleColor backgroundBackup;

        public void Clear()
        {
            Console.Clear();
        }

        public void Write(object? value = null)
        {
            Console.Write(value);
        }

        public void WriteLine(object? value = null)
        {
            Console.WriteLine(value);
        }

        public void EnableHighlighting()
        {
            foregroundBackup = Console.ForegroundColor;
            backgroundBackup = Console.BackgroundColor;
            Console.ForegroundColor = backgroundBackup;
            Console.BackgroundColor = foregroundBackup;
        }

        public void DisableHighlighting()
        {
            Console.ForegroundColor = foregroundBackup;
            Console.BackgroundColor = backgroundBackup;
        }
    }
}
