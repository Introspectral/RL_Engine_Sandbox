using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event
{
    public enum Direction { Up, Down, Left, Right } 
    public class MoveActionEvent : IEvent
    {
        public long EntityId { get; }
        public Direction MoveDirection { get; }
        public MoveActionEvent(long entityId, Direction direction)
        {
            MoveDirection = direction;
            EntityId = entityId;
        }
    }
}