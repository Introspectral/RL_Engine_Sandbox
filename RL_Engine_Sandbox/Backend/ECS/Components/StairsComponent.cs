using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class StairsComponent : IComponents
{
    public StairDirection Direction { get; }
    public StairsComponent(StairDirection direction)
    {
        Direction = direction;
    }
}
