using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public class PickUpEvent : IEvent
{
    public long EntityId;


    public PickUpEvent(long entityId)
    {
        EntityId = entityId;
    }
}