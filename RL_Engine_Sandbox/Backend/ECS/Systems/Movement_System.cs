using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using Direction = RL_Engine_Sandbox.Backend.ECS.Event.Direction;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

public class Movement_System : IMovementSystem
{
    private readonly ICollisionSystem _collisionSystem;
    private readonly IComponentManager _componentManager;
    private readonly IEntityManager _entityManager;
    private readonly IEventManager _eventManager;

    public Movement_System(
        IEntityManager entityManager,
        IComponentManager componentManager,
        IEventManager eventManager,
        ICollisionSystem collisionSystem
    )
    {
        _entityManager = entityManager;
        _componentManager = componentManager;
        _eventManager = eventManager;
        _collisionSystem = collisionSystem;
        // Subscribe to events
        eventManager.Subscribe<MoveActionEvent>(OnMoveEvent);
    }

    public void OnMoveEvent(MoveActionEvent moveActionEvent)
    {
        ProcessMovement(moveActionEvent);
    }

    public void ProcessMovement(MoveActionEvent moveEvent)
    {
        var entity = moveEvent.EntityId;
        var position = _componentManager.GetComponent<PositionComponent>(entity);
        if (position == null) return;

        // Calculate the new position based on the move direction
        var newX = position.X;
        var newY = position.Y;
        switch (moveEvent.MoveDirection)
        {
            case Direction.Up: newY -= 1; break;
            case Direction.Down: newY += 1; break;
            case Direction.Left: newX -= 1; break;
            case Direction.Right: newX += 1; break;
            default: throw new ArgumentOutOfRangeException();
        }

        // Move the entity first, even if the tile is occupied
        MoveToNewPosition(newX, newY, position);

        // Now check if an entity is on the new position
        var targetEntity = _collisionSystem.GetEntityAtPosition(newX, newY, entity);

        if (targetEntity == null) return; // ✅ No entity = exit

        if (IsItem(targetEntity.Id)) IfEntityIsItem(targetEntity);

        if (IsMapEntity(targetEntity.Id)) IfEntityIsMapEntity(targetEntity);
    }

    private void IfEntityIsMapEntity(Entity targetEntity)
    {
        var entityName = _componentManager.GetComponent<NameComponent>(targetEntity.Id)?.Name;
        _eventManager.Publish(new MessageEvent($"You see a {entityName} here!", Color.Aqua));
    }


    private void MoveToNewPosition(int newX, int newY, PositionComponent position)
    {
        if (!_collisionSystem.IsTileWalkable(newX, newY)) return;
        position.X = newX;
        position.Y = newY;
    }

    private void IfEntityIsItem(Entity targetEntity)
    {
        var itemName = _componentManager.GetComponent<NameComponent>(targetEntity.Id)?.Name;
        _eventManager.Publish(new MessageEvent($"You see a {itemName}!", Color.Green));
    }

    // Helper methods
    private bool IsPlayer(long entityId)
    {
        return _componentManager.GetComponent<PlayerControlledComponent>(entityId) != null;
    }

    private bool IsItem(long entity)
    {
        return _componentManager.GetComponent<ItemComponent>(entity) != null;
    }

    private bool IsMapEntity(long entity)
    {
        return _componentManager.GetComponent<MapEntityComponent>(entity) != null;
    }
}