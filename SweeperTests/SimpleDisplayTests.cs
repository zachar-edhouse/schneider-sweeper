using Schneider.Sweeper.Visualization;
using Schneider.Sweeper.Engine;
using Schneider.Sweeper.Map;

namespace Sweeper.Tests
{
    internal class SimpleDisplayTests
    {
        [Test]
        public void WhenDrawCalled_ThenContentIsDrawnProperly()
        {
            Mocks.CachedOutput output = new Mocks.CachedOutput();
            var map = CreateMapMock();
            var player = CreatePlayerMock(1);
            var lifecycleMock = new Mock<ILifecycle>();
            lifecycleMock.Setup(l => l.CurrentState).Returns(LifecycleState.InGame);
            
            var display = new SimpleDisplay(output, map, player, lifecycleMock.Object);
            display.Draw();

            Assert.That(output.Data, Is.EqualTo(
@".XX
..X

Player:
    Position: [0, 1]
    Life count: 1
    Move count: 2
"));
        }

        [Test]
        public void WhenPlayerHasNoLivesLeft_ThenGameOverIsPrinted()
        {
            Mocks.CachedOutput output = new Mocks.CachedOutput();
            var map = CreateMapMock();
            var player = CreatePlayerMock(0);
            var lifecycleMock = new Mock<ILifecycle>();
            lifecycleMock.Setup(l => l.CurrentState).Returns(LifecycleState.Death);

            var display = new SimpleDisplay(output, map, player, lifecycleMock.Object);
            display.Draw();

            bool gameOverIsPresent = output.Data.Contains("GAME OVER");
            Assert.True(gameOverIsPresent, "'GAME OVER' is not part of the output.");
        }

        [Test]
        public void WhenPlayerFinishesGame_ThenGameFinishedIsPrinted()
        {
            Mocks.CachedOutput output = new Mocks.CachedOutput();
            var map = CreateMapMock();
            var player = CreatePlayerMock(0);
            var lifecycleMock = new Mock<ILifecycle>();
            lifecycleMock.Setup(l => l.CurrentState).Returns(LifecycleState.Success);

            var display = new SimpleDisplay(output, map, player, lifecycleMock.Object);
            display.Draw();

            bool gameFinishedIsPresent = output.Data.Contains("GAME FINISHED");
            Assert.True(gameFinishedIsPresent, "'GAME FINISHED' is not part of the output.");
        }

        private IMap CreateMapMock()
        {
            Moq.Mock<IMap> mapMock = new Moq.Mock<IMap>();
            mapMock.Setup(m => m.Width).Returns(3);
            mapMock.Setup(m => m.Height).Returns(2);
            mapMock.Setup(m => m[0, 0]).Returns(FieldValue.Empty);
            mapMock.Setup(m => m[0, 1]).Returns(FieldValue.Empty);
            mapMock.Setup(m => m[1, 0]).Returns(FieldValue.Mine);
            mapMock.Setup(m => m[1, 1]).Returns(FieldValue.Empty);
            mapMock.Setup(m => m[2, 0]).Returns(FieldValue.Mine);
            mapMock.Setup(m => m[2, 1]).Returns(FieldValue.Mine);
            return mapMock.Object;
        }

        private IPlayer CreatePlayerMock(uint lifeCount)
        {
            Moq.Mock<IPlayer> playerMock = new Moq.Mock<IPlayer>();
            playerMock.Setup(p => p.X).Returns(0);
            playerMock.Setup(p => p.Y).Returns(1);
            playerMock.Setup(p => p.LifeCount).Returns(lifeCount);
            playerMock.Setup(p => p.MoveCount).Returns(2);
            return playerMock.Object;
        }
    }
}
