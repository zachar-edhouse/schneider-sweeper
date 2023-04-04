namespace Schneider.Sweeper.Visualization
{
    public interface IOutput
    {
        void Write(object? value = null);
        void WriteLine(object? value = null);
        void Clear();
        void EnableHighlighting();
        void DisableHighlighting();
    }
}
