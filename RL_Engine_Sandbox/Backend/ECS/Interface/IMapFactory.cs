namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IMapFactory
{
    IMap CreateMap(int width, int height);
}