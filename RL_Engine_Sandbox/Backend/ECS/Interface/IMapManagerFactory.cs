using RL_Engine_Sandbox.Backend.ECS.Entities;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IMapManagerFactory
{
    IMapManager CreateMapManager(IMap map,  IMapFactory factory);
}

