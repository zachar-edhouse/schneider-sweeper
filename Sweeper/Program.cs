using Schneider.Sweeper.Configuration;
using Schneider.Sweeper.Engine;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Sweeper.Tests")]

namespace Schneider.Sweeper
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var parser = new ConfigurationParser();
                var configuration = parser.Parse(args);
                if (configuration.ShowHelp)
                {
                    parser.PrintHelp();
                    return;
                }
                Game game = new Game(configuration);
                game.Run();
            }
            catch (Exception ex)
            {
                // Add logging for bug hunting in future releases
                Console.WriteLine($"Something went wrong. Exception message: {ex}");
            }
        }
    }
}