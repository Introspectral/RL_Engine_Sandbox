using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities.Actors;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public class StatChangeEvent : IEvent
{
    long EntityId;
    EntityInfo EntityInfo;
    
    public StatChangeEvent(EntityInfo entityInfo, long entityId)
    {
        EntityId = entityId;
        EntityInfo = entityInfo;
        EntityInfo.Attack = entityInfo.Attack;
        EntityInfo.Defense = entityInfo.Defense;
        EntityInfo.Speed = entityInfo.Speed;
        EntityInfo.Health = entityInfo.Health;
        EntityInfo.MaxHealth = entityInfo.MaxHealth;
        EntityInfo.Speed = entityInfo.Speed;
        EntityInfo.Name = entityInfo.Name;
        EntityInfo.Class = entityInfo.Class;
        EntityInfo.Level = entityInfo.Level;
        EntityInfo.Experience = entityInfo.Experience;
        
    }
}