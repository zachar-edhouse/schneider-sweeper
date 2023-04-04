using Schneider.Sweeper.Input;
using Schneider.Sweeper.Map;
using Schneider.Sweeper.Visualization;

namespace Schneider.Sweeper.Engine
{
    internal class GameElementsFactory : IGameElementsFactory
    {
        public IDisplay CreateDisplay(IMap map, IPlayer player, ILifecycle lifecycle, IRawOutput? rawOutput = null)
        {
            return new SimpleDisplay(map, player, lifecycle, rawOutput);
        }

        public IInput CreateInput(IRawInput? input = null)
        {
            return new Input.Input(input);
        }

        public ILifecycle CreateLifecycle()
        {
            return new LifecycleStateMachine();
        }

        public IMap CreateMap(FieldValue[,] data, uint initialX, uint initialY)
        {
            return new Map.Map(data, initialX, initialY);
        }

        public IPlayer CreatePlayer(IMap map, ILifecycle lifecycle, uint lifeCount, uint startY)
        {
            return new Player(map, lifecycle, lifeCount, startY);
        }
    }
}
