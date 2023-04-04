using Mono.Options;

namespace Schneider.Sweeper.Configuration
{
    internal static class ConfigurationParser
    {
        public static Configuration Parse(string[] args)
        {
            var config = new Configuration();

            var optionSet = new OptionSet();
            optionSet.Add("h|help", "Prints help", option => config.ShowHelp = true);
            optionSet.Add("v=|map_variant=", "(optional) Specifies the map source. Supported values are 'random' or 'file'.",
                option => ParseOptional(option, value => config.Map = value == "random" ? Configuration.MapVariant.Random : Configuration.MapVariant.File));
            optionSet.Add("f=|file=", "(optional) File path of the file to be loaded. Valid only when '-m file' is set.",
                option => ParseOptional(option, value => config.FilePath = value));
            optionSet.Add("w=|width=", "(optional) Width of the random map. Valid only when '-m random' is set.",
                option => ParseOptional(option, value => config.RandomMapWidth = uint.Parse(value)));
            optionSet.Add("g=|height=", "(optional) Height of the random map. Valid only when '-m random' is set.",
                option => ParseOptional(option, value => config.RandomMapHeight = uint.Parse(value)));
            optionSet.Add("m=|mines=", "(optional) Mines count in the random map. Valid only when '-m random' is set.",
                option => ParseOptional(option, value => config.RandomMapMinesCount = uint.Parse(value)));
            optionSet.Add("l=|lives=", "(optional) The number of lifes.",
                option => ParseOptional(option, value => config.LifeCount = uint.Parse(value)));

            optionSet.Parse(args.ToList());

            return config;
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("SchneiderSweeper [-v MAP_VARIANT] [-f FILEPATH] [-w WIDTH] [-g HEIGHT] [-m MINES_COUNT] [-l LIVES] [-h]");
        }

        private static void ParseOptional(string option, Action<string> action)
        {
            if (option != null)
            {
                action(option);
            }
        }
    }
}
