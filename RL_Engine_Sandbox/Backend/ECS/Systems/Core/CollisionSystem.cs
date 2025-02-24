using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Systems.Core;

public class CollisionSystem : ICollisionSystem
{
    
    private IComponentManager _componentManager;
    private IEventManager _eventManager;
    private IEntityManager _entityManager;
    private IMapManager _mapManager;

    public CollisionSystem(
        IComponentManager componentManager, 
        IEventManager eventManager,
        IEntityManager entityManager,
        IMapManager mapManager)
        {
            _componentManager = componentManager;
            _eventManager = eventManager;
            _entityManager = entityManager;
            _mapManager = mapManager;
        }

    public bool CollisionCheck(long entityId, int x, int y)
    {
        // This needs to check if the tile is walkable and then communicate that to the movement system
        return _mapManager.IsTileWalkable(x, y);

        // foreach (var entities in _entityManager.GetAllEntities())
        // {
        //     if (entities.Id == entityId) continue;
        //     var otherPosition = _componentManager.GetComponent<PositionComponent>(entities.Id);
        //     if (otherPosition == null) continue;
        //     if (position.X == otherPosition.X && position.Y == otherPosition.Y) return true;
        // }
    }
}



    
