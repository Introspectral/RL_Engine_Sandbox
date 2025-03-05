using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public class CollisionEvent(long entityA, long entityB) : IEvent
{
    public long EntityA { get; set; } = entityA;
    public long EntityB { get; set; } = entityB;
}