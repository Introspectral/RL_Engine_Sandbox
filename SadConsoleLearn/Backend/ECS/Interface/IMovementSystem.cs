using RL_Engine_Sandbox.Backend.ECS.Event;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IMovementSystem
{
    public void OnMoveEvent(MoveActionEvent moveActionEvent);
    public void ProcessMovement();
}