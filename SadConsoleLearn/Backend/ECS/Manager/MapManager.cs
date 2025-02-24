using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Manager;

public class MapManager : IMapManager
{
    private readonly IMap _map;
    private readonly IComponentManager _componentManager;
    
    public MapManager(IMap map, IComponentManager componentManager)
    {
        _map = map;
        _componentManager = componentManager;
    }
    public bool IsTileWalkable(int x, int y)
    {
        // for now, just return true
        return true;
    }
}