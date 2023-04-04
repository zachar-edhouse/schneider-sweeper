using Schneider.Sweeper.Map;

namespace Sweeper.Tests
{
    public class MapTests
    {
        private FieldValue[,] data = new FieldValue[0, 0];

        [SetUp]
        public void Setup()
        {
            string fileName = "map_tests_data.txt";
            File.WriteAllText(fileName,
@"    XX
X   X 
X   XX");
            try
            {
                data = MapHelper.LoadFromFile(fileName);
                Assert.IsNotNull(data);
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Test]
        public void WhenMapIsLoaded_ThenItInitializedProperly()
        {
            var map = new Map(data, 2, 1);
            Assert.That(map.Width, Is.EqualTo(6));
            Assert.That(map.Height, Is.EqualTo(3));
        }

        [Test]
        public void WhenFieldIsVisitedAndItContainsEmpty_ThenAllNeighboursAreVisitedRecursivellyUntilFirstNonEmptyField()
        {
            var map = new Map(data, 2, 1);
            Assert.That(map[0, 0], Is.EqualTo(FieldValue.Hidden));
            Assert.That(map[1, 0], Is.EqualTo(FieldValue.One));
            Assert.That(map[2, 0], Is.EqualTo(FieldValue.Empty));
            Assert.That(map[3, 0], Is.EqualTo(FieldValue.Two));
            Assert.That(map[1, 1], Is.EqualTo(FieldValue.Two));
            Assert.That(map[2, 1], Is.EqualTo(FieldValue.Empty));
            Assert.That(map[3, 1], Is.EqualTo(FieldValue.Three));
            Assert.That(map[1, 2], Is.EqualTo(FieldValue.Two));
            Assert.That(map[2, 2], Is.EqualTo(FieldValue.Empty));
            Assert.That(map[3, 2], Is.EqualTo(FieldValue.Two));
        }

        [Test]
        public void WhenFieldWithNumberIsVisited_ThenOnlyThatFieldIsUncovered()
        {
            var map = new Map(data, 0, 0);
            Assert.That(map[0, 0], Is.EqualTo(FieldValue.One));
            Assert.That(map[1, 0], Is.EqualTo(FieldValue.Hidden));
            Assert.That(map[0, 1], Is.EqualTo(FieldValue.Hidden));
            Assert.That(map[1, 1], Is.EqualTo(FieldValue.Hidden));
        }

        [Test]
        public void WhenFieldWithMineIsVisited_ThenOnlyThatFieldIsUncovered()
        {
            var map = new Map(data, 5, 2);
            Assert.That(map[5, 2], Is.EqualTo(FieldValue.Mine));
            Assert.That(map[4, 2], Is.EqualTo(FieldValue.Hidden));
            Assert.That(map[5, 1], Is.EqualTo(FieldValue.Hidden));
            Assert.That(map[4, 1], Is.EqualTo(FieldValue.Hidden));
        }
    }
}
