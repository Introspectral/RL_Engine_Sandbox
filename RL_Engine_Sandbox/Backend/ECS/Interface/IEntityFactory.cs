using RL_Engine_Sandbox.Backend.ECS.Entity.Actors;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IEntityFactory
{
    public (Entity.Entity, PlayerInfo) CreatePlayer();
}