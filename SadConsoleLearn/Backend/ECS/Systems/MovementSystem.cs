using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using Direction = RL_Engine_Sandbox.Backend.ECS.Event.Direction;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

public class MovementSystem : IMovementSystem
{
    private readonly IEntityManager _entityManager;
    private readonly IComponentManager _componentManager;
    private readonly IEventManager _eventManager;

    public MovementSystem(IEntityManager entityManager, IComponentManager componentManager, IEventManager eventManager) {
        _entityManager = entityManager;
        _componentManager = componentManager;
        _eventManager = eventManager;

        eventManager.Subscribe<MoveActionEvent>(OnMoveEvent);
    }
    public void OnMoveEvent(MoveActionEvent moveEvent) {
        var entity = moveEvent.EntityId;
            var position = _componentManager.GetComponent<PositionComponent>(entity);    
            
            if (position == null) {
                return;
            }
            switch (moveEvent.MoveDirection)
            {
                case Direction.Up:
                    position.X -= 1;
                    break;
                case Direction.Down:
                    position.Y += 1;
                    break;
                case Direction.Left:
                    position.X -= 1;
                    break;
                case Direction.Right:
                    position.X += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    public void ProcessMovement() {
        // throw new NotImplementedException();
    }
}




