namespace Schneider.Sweeper.Configuration
{
    internal class Configuration
    {
        public enum MapVariant
        {
            Random,
            File
        }
        public MapVariant Map { get; set; } = MapVariant.Random;
        public string FilePath { get; set; } = "";
        public uint RandomMapWidth { get; set; } = 80;
        public uint RandomMapHeight { get; set; } = 20;
        public uint RandomMapMinesCount { get; set; } = 100;
        public uint LifeCount { get; set; } = 5;

        public bool ShowHelp { get; set; } = false;
    }
}
