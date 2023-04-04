using Schneider.Sweeper.Input;
using Schneider.Sweeper.Map;
using Schneider.Sweeper.Visualization;

namespace Schneider.Sweeper.Engine
{
    public interface IGameElementsFactory
    {
        IMap CreateMap(FieldValue[,] data, uint initialX, uint initialY);
        ILifecycle CreateLifecycle();
        IPlayer CreatePlayer(IMap map, ILifecycle lifecycle, uint lifeCount, uint startY);
        IDisplay CreateDisplay(IOutput output, IMap map, IPlayer player, ILifecycle lifecycle);
        IInput CreateInput(IRawInput? input = null);
    }
}
