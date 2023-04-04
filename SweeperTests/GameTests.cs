using Schneider.Sweeper.Engine;
using Schneider.Sweeper.Input;
using Schneider.Sweeper.Map;
using Schneider.Sweeper.Visualization;

namespace Sweeper.Tests
{
    public class GameTests
    {
        private Mock<IPlayer> playerMock;
        private Mock<IInput> inputMock;
        private Mock<IDisplay> displayMock;
        private Mock<IMap> mapMock;
        private Mock<ILifecycle> lifecycleMock;
        private Mock<IGameElementsFactory> gameElementsFactoryMock;

        [SetUp]
        public void Setup()
        {
            playerMock = new Mock<IPlayer>();
            playerMock.Setup(p => p.LifeCount).Returns(3);
            inputMock = new Mock<IInput>();
            displayMock = new Mock<IDisplay>();
            mapMock = new Mock<IMap>();
            lifecycleMock = new Mock<ILifecycle>();
            lifecycleMock.Setup(l => l.CurrentState).Returns(LifecycleState.InGame);
            gameElementsFactoryMock = new Mock<IGameElementsFactory>();
            gameElementsFactoryMock.Setup(gef => gef.CreateInput(It.IsAny<IRawInput>())).Returns(inputMock.Object);
            gameElementsFactoryMock.Setup(gef => gef.CreateLifecycle()).Returns(lifecycleMock.Object);
            gameElementsFactoryMock.Setup(ger => ger.CreateMap(It.IsAny<FieldValue[,]>(), It.IsAny<uint>(), It.IsAny<uint>())).Returns(mapMock.Object);
            gameElementsFactoryMock.Setup(gef => gef.CreateDisplay(It.IsAny<IMap>(), It.IsAny<IPlayer>(), It.IsAny<ILifecycle>(), It.IsAny<IRawOutput>())).Returns(displayMock.Object);
            gameElementsFactoryMock.Setup(gef => gef.CreatePlayer(It.IsAny<IMap>(),It.IsAny<ILifecycle>(), It.IsAny<uint>(), It.IsAny<uint>())).Returns(playerMock.Object);
        }

        [Test]
        public void WhenInputIsReceived_ThenMoveShouldHappenAndDisplayShouldRedraw()
        {
            var game = new Game(new Schneider.Sweeper.Configuration.Configuration(), gameElementsFactoryMock.Object);
            inputMock.Raise(i => i.OnDirectionInput += null, inputMock.Object, Direction.Left);
            playerMock.Verify(p => p.Move(Direction.Left), Times.Once);
            displayMock.Verify(d => d.Draw(), Times.Exactly(2)); // first draw is in constructor, second after move
        }

        [Test]
        public void WhenNotInInGameState_ThenMoveIsNotCalled()
        {
            lifecycleMock.Setup(l => l.CurrentState).Returns(LifecycleState.Initializing);
            var game = new Game(new Schneider.Sweeper.Configuration.Configuration(), gameElementsFactoryMock.Object);
            inputMock.Raise(i => i.OnDirectionInput += null, inputMock.Object, Direction.Left);
            playerMock.Verify(p => p.Move(Direction.Left), Times.Never);
        }

        [Test]
        public void WhenEscapeIsReturnedFromInput_ThenGameEnds()
        {
            inputMock.Setup(i => i.ReadNext()).Callback(() => 
                inputMock.Raise(i => i.OnEscapePressed += null, inputMock.Object, new EventArgs()));
            var game = new Game(new Schneider.Sweeper.Configuration.Configuration(), gameElementsFactoryMock.Object);
            game.Run();
        }

        [TestCase(LifecycleState.Success)]
        [TestCase(LifecycleState.Death)]
        public void WhenInSuccessOrDeathStateAndAnyKeyIsPressed_ThenGameEnds(LifecycleState state)
        {
            inputMock.Setup(i => i.ReadNext()).Callback(() =>
                inputMock.Raise(i => i.OnKeyPressed += null, inputMock.Object, new EventArgs()));
            lifecycleMock.Setup(l => l.CurrentState).Returns(state);
            var game = new Game(new Schneider.Sweeper.Configuration.Configuration(), gameElementsFactoryMock.Object);
            game.Run();
        }
    }
}
