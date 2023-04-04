namespace Sweeper.Tests.Mocks
{
    public class CachedOutput : Schneider.Sweeper.Visualization.IRawOutput
    {
        public string Data { get; private set; } = "";

        public void Clear()
        {
            Data = "";
        }

        public void EnableHighlighting()
        {
            // do nothing
        }

        public void DisableHighlighting()
        {
            // do nothing
        }

        public void Write(object? value = null)
        {
            Add(value);
        }

        public void WriteLine(object? value = null)
        {
            Add(value);
        }

        private void Add(object? value)
        {
            if (value == null)
                return;
            if (value is string)
                Data += (string)value;
            if (value is char)
                Data += (char)value;
        }
    }
}
