using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

public class CollisionSystem
{
    
    private IComponentManager _componentManager;
    private IMapManager _mapManager;
    private IEventManager _eventManager;
    private IEntityManager _entityManager;

    public CollisionSystem(
        IComponentManager componentManager, 
        IMapManager mapManager, 
        IEventManager eventManager,
        IEntityManager entityManager)
        {
            _componentManager = componentManager;
            _mapManager = mapManager;
            _eventManager = eventManager;
            _entityManager = entityManager;
        }
    
    public bool IsWalkable(int x, int y)
    {
        return _mapManager.IsTileWalkable(x, y);
    }
    public bool CollisionCheck(long entityId)
    {
       var position = _componentManager.GetComponent<PositionComponent>(entityId);
       if (position == null) return false;
       if (!_mapManager.IsTileWalkable(position.X, position.Y)) return true;

       foreach (var entities in _entityManager.GetAllEntities())
       {
           if (entities.Id == entityId) continue;
           var otherPosition = _componentManager.GetComponent<PositionComponent>(entities.Id);
           if (otherPosition == null) continue;
           if (position.X == otherPosition.X && position.Y == otherPosition.Y) return true;
       }
       return false;
   }
}


    
