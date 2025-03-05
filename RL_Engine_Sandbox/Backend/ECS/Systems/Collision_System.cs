using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

public class Collision_System(
    IComponentManager componentManager,
    IEventManager eventManager,
    IEntityManager entityManager, IMapManager mapManager)
    : ICollisionSystem
{

    
    public void CollisionCheck(long entityId, int x, int y)
    {
        var targetEntity = mapManager.CurrentMap.Entities
            .FirstOrDefault(e =>
                e.Id != entityId && // Ignore self
                componentManager.GetComponent<PositionComponent>(e.Id)?.X == x &&
                componentManager.GetComponent<PositionComponent>(e.Id)?.Y == y);

        if (targetEntity != null) eventManager.Publish(new CollisionEvent(entityId, targetEntity.Id));
    }

    public Entity? GetEntityAtPosition(int x, int y, long ignoringEntityId)
    {
        return mapManager.CurrentMap.Entities
            .FirstOrDefault(e =>
                e.Id != ignoringEntityId && // ✅ Ignore the entity checking for collisions
                componentManager.GetComponent<PositionComponent>(e.Id)?.X == x &&
                componentManager.GetComponent<PositionComponent>(e.Id)?.Y == y);
    }

    public bool IsTileOccupied(int x, int y, long ignoreEntityId)
    {
        foreach (var entity in mapManager.CurrentMap.Entities)
        {
            if (entity.Id == ignoreEntityId) continue; // ✅ Ignore the entity we're checking movement for

            var position = componentManager.GetComponent<PositionComponent>(entity.Id);
            if (position != null && position.X == x &&
                position.Y == y) return true; // There is another entity at this location
        }

        return false; // No entity is here
    }

    public bool IsTileWalkable(int x, int y)
    {
        return mapManager.IsTileWalkable(x, y);
    }
}