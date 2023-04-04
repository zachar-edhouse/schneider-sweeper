using Schneider.Sweeper.Input;
using Schneider.Sweeper.Map;
using Schneider.Sweeper.Visualization;

namespace Schneider.Sweeper.Engine
{
    internal class Game
    {
        private bool gameFinished;
        private readonly ILifecycle lifecycle;
        private readonly IPlayer player;
        private readonly IDisplay display;
        private readonly IInput input;

        public Game(Configuration.Configuration? config = null, IGameElementsFactory? gameElementsFactory = null)
        {
            gameElementsFactory ??= new GameElementsFactory();
            config ??= new Configuration.Configuration();

            gameFinished = false;

            var mapData = GetMapData(config);
            var startPosition = (uint)mapData.GetLength(0) / 2;
            var map = gameElementsFactory.CreateMap(mapData, 0, startPosition);

            lifecycle = gameElementsFactory.CreateLifecycle();
            lifecycle.StateChanged += LifecycleStateChanged;

            player = gameElementsFactory.CreatePlayer(map, lifecycle, config.LifeCount, startPosition);

            display = gameElementsFactory.CreateDisplay(map, player, lifecycle);
            display.Draw();

            input = gameElementsFactory.CreateInput();
            input.OnKeyPressed += InputOnKeyPressed;
            input.OnDirectionInput += InputOnDirectionInput;
            input.OnEscapePressed += InputOnEscapePressed;
        }

        public void Run()
        {
            while (!gameFinished)
            {
                input.ReadNext();
            }
        }

        private FieldValue[,] GetMapData(Configuration.Configuration config)
        {
            FieldValue[,] data;
            if (config.Map == Configuration.Configuration.MapVariant.Random)
            {
                data = MapHelper.CreateRandom(config.RandomMapWidth, config.RandomMapHeight, config.RandomMapMinesCount);
            }
            else
            {
                data = MapHelper.LoadFromFile(config.FilePath);
            }
            return data;
        }

        private void LifecycleStateChanged(object? sender, LifecycleState e)
        {
            display.Draw();
        }

        private void InputOnKeyPressed(object? sender, EventArgs e)
        {
            if (lifecycle.CurrentState == LifecycleState.Initializing)
            {
                lifecycle.StartGame();
                display.Draw();
            }
            else if (lifecycle.CurrentState == LifecycleState.Death
                || lifecycle.CurrentState == LifecycleState.Success)
            {
                gameFinished = true;
            }
        }

        private void InputOnDirectionInput(object? sender, Direction direction)
        {
            if (lifecycle.CurrentState != LifecycleState.InGame)
            {
                return;
            }

            player.Move(direction);
            display.Draw();
        }

        private void InputOnEscapePressed(object? sender, EventArgs e)
        {
            gameFinished = true;
        }
    }
}
