using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Manager;

namespace RL_Engine_Sandbox.Backend.ECS.Map;

// This class is responsible for creating a MapManager object, since the MapManager class depends on 
// values only known at runtime.
public class MapManagerFactory : IMapManagerFactory
{
    public IMapManager CreateMapManager(IMap map, IMapFactory factory)
    {
        return new MapManager(factory, 15 );
    }
}