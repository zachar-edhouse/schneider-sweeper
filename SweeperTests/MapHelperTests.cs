using Schneider.Sweeper.Map;

namespace Sweeper.Tests
{
    internal class MapHelperTests
    {
        [Test]
        public void WhenLoadFromFileIsCalled_ThenProperMapIsCreated()
        {
            string fileName = "temporary1.txt";
            File.WriteAllText(fileName,
@"  XX 
X   X");
            try
            {
                var data = MapHelper.LoadFromFile(fileName);
                Assert.IsNotNull(data);
                Assert.That(data.GetLength(1), Is.EqualTo(5));
                Assert.That(data.GetLength(0), Is.EqualTo(2));
                Assert.That(data[0, 0], Is.EqualTo(FieldValue.Empty));
                Assert.That(data[0, 2], Is.EqualTo(FieldValue.Mine));
                Assert.That(data[1, 4], Is.EqualTo(FieldValue.Mine));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Test]
        public void WhenFileContainsInvalidData_ThenProperExceptionIsThrown()
        {
            string fileName = "temporary2.txt";
            File.WriteAllText(fileName,
@"a XX 
X   X");
            try
            {
                Assert.Throws<InvalidDataException>(() => MapHelper.LoadFromFile(fileName));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Test]
        public void WhenFileContainsLinesWithDifferentLength_ThenProperExceptionIsThrown()
        {
            string fileName = "temporary3.txt";
            File.WriteAllText(fileName,
@"  XX    
X   X");
            try
            {
                Assert.Throws<InvalidDataException>(() => MapHelper.LoadFromFile(fileName));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Test]
        public void WhenFileDoesntExist_ThenProperExceptionIsThrown()
        {
            string fileName = "temporary4.txt";
            try
            {
                Assert.Throws<FileNotFoundException>(() => MapHelper.LoadFromFile(fileName));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Test]
        public void WhenFileContainsNoData_ThenProperExceptionIsThrown()
        {
            string fileName = "temporary5.txt";
            File.WriteAllText(fileName, "");
            try
            {
                Assert.Throws<InvalidDataException>(() => MapHelper.LoadFromFile(fileName));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Test]
        public void WhenRandomMapIsCreated_ThenValidMapIsReturned()
        {
            var data = MapHelper.CreateRandom(8, 8, 10);
            Assert.That(data.GetLength(1), Is.EqualTo(8));
            Assert.That(data.GetLength(0), Is.EqualTo(8));
            int minesCount = 0;
            for (uint y = 0; y < data.GetLength(0); y++)
            {
                for (uint x = 0; x < data.GetLength(1); x++)
                {
                    if (data[x, y] == FieldValue.Mine)
                    {
                        minesCount++;
                    }
                }
            }
            Assert.That(minesCount, Is.LessThanOrEqualTo(10));
            Assert.That(minesCount, Is.GreaterThan(0));
        }
    }
}
