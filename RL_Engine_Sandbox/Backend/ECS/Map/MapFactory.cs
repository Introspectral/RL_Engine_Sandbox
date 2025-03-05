using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Map;

public class MapFactory(int width, int height) : IMapFactory
{
    public IMap CreateMap(int width, int height)
    {
        return new Map(width, height);
        
    }
}