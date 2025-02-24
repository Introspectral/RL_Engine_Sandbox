using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Manager;

public class EntityManager : IEntityManager
{
    private List<Entity.Entity> _entities = new();

    public void AddEntity(Entity.Entity entity)
    {
        _entities.Add(entity);
    }

    public void RemoveEntity(long id)
    {
        _entities.RemoveAll(e => e.Id == id);
    }

    public Entity.Entity GetById(long id)
    {
        return _entities.Find(e => e.Id == id) ?? throw new InvalidOperationException("Entity not found");
    }

    public IEnumerable<Entity.Entity> GetAllEntities()
    {
        return _entities;
    }
}

