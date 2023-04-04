using Schneider.Sweeper.Engine;
using Schneider.Sweeper.Map;

namespace Sweeper.Tests
{
    public class PlayerTests
    {
        private Mock<IMap> mapMock;
        private Mock<ILifecycle> lifecycleMock;

        [SetUp]
        public void Setup()
        {
            mapMock = new Moq.Mock<IMap>();
            mapMock.Setup(m => m.Height).Returns(8);
            mapMock.Setup(m => m.Width).Returns(8);
            mapMock.Setup(m => m[It.IsAny<uint>(), It.IsAny<uint>()]).Returns(FieldValue.Empty);
            mapMock.Setup(m => m.Visit(It.IsAny<uint>(), It.IsAny<uint>())).Returns(FieldValue.Empty);

            lifecycleMock = new Moq.Mock<ILifecycle>();
            lifecycleMock.Setup(l => l.CurrentState).Returns(LifecycleState.InGame);
        }

        [Test]
        public void WhenPlayerIsCreatedWithInvalidParameters_ThenProperExceptionIsThrown()
        {
            Assert.Throws<IndexOutOfRangeException>(() => new Player(mapMock.Object, lifecycleMock.Object, 3, 10));
        }

        [Test]
        public void WhenPlayerStartsOnMine_ThenHisLifeCountDoesntChange()
        {
            mapMock.Setup(m => m.Visit(0, 0)).Returns(FieldValue.Mine);
            var player = new Player(mapMock.Object, lifecycleMock.Object, 3, 0);
            Assert.That(player.LifeCount, Is.EqualTo(3));
        }

        [Test]
        public void WhenPlayerMoves_ThenHisPositionAndMoveCountChanges()
        {
            var player = new Player(mapMock.Object, lifecycleMock.Object, 3, 0);
            player.Move(Direction.Right);
            player.Move(Direction.Down);
            player.Move(Direction.Down);
            Assert.That(player.X, Is.EqualTo(1));
            Assert.That(player.Y, Is.EqualTo(2));
            Assert.That(player.MoveCount, Is.EqualTo(3));
        }

        [Test]
        public void WhenPlayerMovesOutsideOfField_ThenHisPositionAndMoveCountDoesntChange()
        {
            var player = new Player(mapMock.Object, lifecycleMock.Object, 3, 0);
            player.Move(Direction.Right);
            player.Move(Direction.Up);
            player.Move(Direction.Up);
            Assert.That(player.X, Is.EqualTo(1));
            Assert.That(player.Y, Is.EqualTo(0));
            Assert.That(player.MoveCount, Is.EqualTo(1));
        }

        [Test]
        public void WhenPlayerHitsMine_ThenHisLifeCountDecreases()
        {
            mapMock.Setup(m => m.Visit(1, 1)).Returns(FieldValue.Mine);
            var player = new Player(mapMock.Object, lifecycleMock.Object, 3, 0);
            player.Move(Direction.Right);
            player.Move(Direction.Down);
            Assert.That(player.LifeCount, Is.EqualTo(2));
        }

        [Test]
        public void WhenPlayerDrainsHisLives_ThenTheDeathEventIsRaised()
        {
            mapMock.Setup(m => m.Visit(1, 0)).Returns(FieldValue.Mine);
            mapMock.Setup(m => m.Visit(2, 0)).Returns(FieldValue.Mine);
            mapMock.Setup(m => m.Visit(3, 0)).Returns(FieldValue.Mine);

            var player = new Player(mapMock.Object, lifecycleMock.Object, 3, 0);
            
            player.Move(Direction.Right);
            player.Move(Direction.Right);
            player.Move(Direction.Right);

            lifecycleMock.Verify(l => l.Die(), Times.Once());
        }

        [Test]
        public void WhenPlayerHitsOppositeSideOfBoard_ThenTheSuccessEventIsRaised()
        {
            var player = new Player(mapMock.Object, lifecycleMock.Object, 1, 0);
            
            for (int i = 0; i < 8; ++i)
            {
                player.Move(Direction.Right);
            }

            lifecycleMock.Verify(l => l.Succeed(), Times.Once());
        }
    }
}
