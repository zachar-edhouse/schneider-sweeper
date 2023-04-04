namespace Schneider.Sweeper.Input
{
    internal class ConsoleInput : IRawInput
    {
        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
    }
}
