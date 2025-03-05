using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public class RemoveEntityEvent : IEvent
{
    public RemoveEntityEvent(long entityId)
    {
        EntityId = entityId;
    }

    public long EntityId { get; set; }
}