using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public enum StairDirection
{
    Up,
    Down
}

public class UseStairsEvent : IEvent
{
    public long EntityId { get; }
    public StairDirection Direction { get; }

    public UseStairsEvent(long entityId, StairDirection direction)
    {
        EntityId = entityId;
        Direction = direction;
    }
}
