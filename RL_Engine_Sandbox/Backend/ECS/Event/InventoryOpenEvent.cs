using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public class InventoryOpenEvent : IEvent
{
    public long EntityId { get; set; }

    public InventoryOpenEvent(long entityId)
    {
        EntityId = entityId;
    }
}