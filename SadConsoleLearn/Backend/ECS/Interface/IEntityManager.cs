namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IEntityManager
{
    public void AddEntity(Entity.Entity entity);
    public void RemoveEntity(long id);
    public Entity.Entity GetById(long id);
    public IEnumerable<Entity.Entity> GetAllEntities();

}