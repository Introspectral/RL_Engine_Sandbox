using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Map;

public class MapFactory : IMapFactory
{
    public int Width { get; set; }
    public int Height { get; set; }

    public MapFactory(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public IMap CreateMap()
    {
        return new Map(Width, Height);
    }
}



