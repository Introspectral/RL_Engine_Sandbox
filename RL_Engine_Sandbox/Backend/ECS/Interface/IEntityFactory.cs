using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Entities.Actors;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IEntityFactory
{
    public (Entity, EntityInfo) CreatePlayer();
    public Entity CreateStairUp();
    public Entity CreateStairDown();

    public Entity CreateSword();
    public Entity CreateShield();
    public Entity CreatePotion();
}