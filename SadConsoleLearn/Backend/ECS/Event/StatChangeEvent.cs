using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public class StatChangeEvent : IEvent
{
    public StatsComponent StatsComponent;
    public long EntityId;

    public StatChangeEvent(long entityId, StatsComponent statsComponent)
    {
        EntityId = entityId;
        StatsComponent = statsComponent;
        StatsComponent.Attack = statsComponent.Attack;
        StatsComponent.Defense = statsComponent.Defense;
        StatsComponent.Speed = statsComponent.Speed;
    }
}