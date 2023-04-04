using Schneider.Sweeper.Configuration;

namespace Sweeper.Tests
{
    internal class ConfigurationParserTests
    {
        [Test]
        public void WhenArgumentsArePassed_ThenProperConfigurationIsCreated()
        {
            var configuration = ConfigurationParser.Parse(new string[] { "-v", "random", "-m", "1", "-w", "5", "-g", "3" });
            Assert.IsNotNull(configuration);
            Assert.That(configuration.Map, Is.EqualTo(Configuration.MapVariant.Random));
            Assert.That(configuration.RandomMapMinesCount, Is.EqualTo(1));
            Assert.That(configuration.RandomMapWidth, Is.EqualTo(5));
            Assert.That(configuration.RandomMapHeight, Is.EqualTo(3));
        }

        [Test]
        public void WhenArgumentsArePassed_ThenProperConfigurationIsCreated2()
        {
            var configuration = ConfigurationParser.Parse(new string[] { "-v", "file", "-f", "filepath.txt", "-l", "42"});
            Assert.IsNotNull(configuration);
            Assert.That(configuration.Map, Is.EqualTo(Configuration.MapVariant.File));
            Assert.That(configuration.FilePath, Is.EqualTo("filepath.txt"));
            Assert.That(configuration.LifeCount, Is.EqualTo(42));
        }

        [Test]
        public void WhenInvalidArtumentsArePassed_ThenExceptionIsThrown()
        {
            Assert.Throws<FormatException>(() => ConfigurationParser.Parse(new string[] { "-l", "invalid" }));
        }

        [Test]
        public void WhenNoArgumentsArePassed_ThenDefaultConfigurationIsCreated()
        {
            var configuration = ConfigurationParser.Parse(new string[] { });
            Assert.IsNotNull(configuration);
        }
    }
}
