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
                var configuration = ConfigurationParser.Parse(args);
                if (configuration.ShowHelp)
                {
                    ConfigurationParser.PrintHelp();
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