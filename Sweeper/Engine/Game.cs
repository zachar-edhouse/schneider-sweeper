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

            FieldValue[,] data;
            if (config.Map == Configuration.Configuration.MapVariant.Random)
            {
                data = MapHelper.CreateRandom(config.RandomMapWidth, config.RandomMapHeight, config.RandomMapMinesCount);
            }
            else
            {
                data = MapHelper.LoadFromFile(config.FilePath);
            }
            uint startPosition = (uint)data.GetLength(0) / 2;

            var map = gameElementsFactory.CreateMap(data, 0, startPosition);

            lifecycle = gameElementsFactory.CreateLifecycle();
            lifecycle.StateChanged += LifecycleStateChanged;

            player = gameElementsFactory.CreatePlayer(map, lifecycle, config.LifeCount, startPosition);

            display = gameElementsFactory.CreateDisplay(new ConsoleOutput(), map, player, lifecycle);
            display.Draw();

            input = gameElementsFactory.CreateInput();
            input.OnKeyPressed += InputOnKeyPressed;
            input.OnDirectionInput += ProcessDirectionInput;
        }

        public void Run()
        {
            while (!gameFinished)
            {
                input.ReadNext();
            }
        }

        private void InputOnKeyPressed(object? sender, ConsoleKeyInfo e)
        {
            if (e.Key == ConsoleKey.Escape)
            {
                gameFinished = true;
            }
            else if (lifecycle.CurrentState == LifecycleState.Initializing)
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

        private void LifecycleStateChanged(object? sender, LifecycleState e)
        {
            display.Draw();
        }

        private void ProcessDirectionInput(object? sender, Direction direction)
        {
            if (lifecycle.CurrentState != LifecycleState.InGame)
            {
                return;
            }

            player.Move(direction);
            display.Draw();
        }
    }
}
