using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using Direction = RL_Engine_Sandbox.Backend.ECS.Event.Direction;

namespace RL_Engine_Sandbox.Backend.ECS.Systems.Core;

public class MovementSystem : IMovementSystem
{
    private readonly IEntityManager _entityManager;
    private readonly IComponentManager _componentManager;
    private readonly IEventManager _eventManager;
    private readonly ICollisionSystem _collisionSystem;

    public MovementSystem(IEntityManager entityManager, IComponentManager componentManager, IEventManager eventManager, ICollisionSystem collisionSystem) {
        _entityManager = entityManager;
        _componentManager = componentManager;
        _eventManager = eventManager;
        _collisionSystem = collisionSystem;
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
        if (position == null) { return; }

        // Calculate the new position based on the move direction
        int newX = position.X;
        int newY = position.Y;
        switch (moveEvent.MoveDirection)
        {
            case Direction.Up:
                newY -= 1;
                break;
            case Direction.Down:
                newY += 1;
                break;
            case Direction.Left:
                newX -= 1;
                break;
            case Direction.Right:
                newX += 1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // Check if the new position is walkable
        if (_collisionSystem.CollisionCheck(entity, newX, newY))
        {
            // Update the position if the new tile is walkable
            position.X = newX;
            position.Y = newY;
        }
    }
}




