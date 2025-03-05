using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class MoveActionEvent(long entityId, Direction direction) : IEvent
{
    public long EntityId { get; } = entityId;
    public Direction MoveDirection { get; } = direction;
}