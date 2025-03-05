using RL_Engine_Sandbox.Backend.ECS.Entities;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IEntityManager
{
    public void AddEntity(Entity entity);
    public void RemoveEntity(long id);
    public Entity GetById(long id);
    public IEnumerable<Entity> GetAllEntities();
}