using Schneider.Sweeper.Input;
using Schneider.Sweeper.Map;
using Schneider.Sweeper.Visualization;

namespace Schneider.Sweeper.Engine
{
    internal class GameElementsFactory : IGameElementsFactory
    {
        public IDisplay CreateDisplay(IOutput output, IMap map, IPlayer player, ILifecycle lifecycle)
        {
            return new SimpleDisplay(output, map, player, lifecycle);
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
