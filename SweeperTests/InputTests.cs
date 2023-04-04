using Schneider.Sweeper.Input;
using Moq;
using Schneider.Sweeper.Engine;

namespace Sweeper.Tests
{
    public class InputTests
    {
        [Test]
        public void WhenDirectionKeyIsRead_ThenProperEventIsCalled()
        {
            Mock<IRawInput> rawInputMock = new Mock<IRawInput>();
            var sequence = new MockSequence();

            rawInputMock.SetupSequence(ri => ri.ReadKey())
                .Returns(new ConsoleKeyInfo(' ', ConsoleKey.LeftArrow, false, false, false))
                .Returns(new ConsoleKeyInfo(' ', ConsoleKey.RightArrow, false, false, false))
                .Returns(new ConsoleKeyInfo(' ', ConsoleKey.DownArrow, false, false, false))
                .Returns(new ConsoleKeyInfo(' ', ConsoleKey.UpArrow, false, false, false));

            var input = new Input(rawInputMock.Object);
            List<Direction> directions = new List<Direction>();
            input.OnDirectionInput += (sender, dir) => directions.Add(dir);

            for (int i = 0; i < 4; ++i)
            {
                input.ReadNext();
            }

            var expectedDirections = new List<Direction>() { Direction.Left, Direction.Right, Direction.Down, Direction.Up };
            Assert.That(directions.Count, Is.EqualTo(4));
            Assert.That(directions, Is.EquivalentTo(expectedDirections));
        }

        [Test]
        public void WhenAnyKeyIsRead_ThenProperEventIsRaised()
        {
            Mock<IRawInput> rawInputMock = new Mock<IRawInput>();
            rawInputMock.SetupSequence(ri => ri.ReadKey())
                .Returns(new ConsoleKeyInfo(' ', ConsoleKey.Escape, false, false, false))
                .Returns(new ConsoleKeyInfo(' ', ConsoleKey.LeftArrow, false, false, false))
                .Returns(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false));

            var input = new Input(rawInputMock.Object);
            int counter = 0;
            input.OnKeyPressed += (sender, arg) => counter++;

            for (int i = 0; i < 3; ++i)
            {
                input.ReadNext();
            }

            Assert.That(counter, Is.EqualTo(3));
        }
    }
}
