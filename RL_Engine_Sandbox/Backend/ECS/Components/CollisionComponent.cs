using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class CollisionComponent(bool isCollidable) : IComponents
{
    public bool IsCollidable { get; set; } = isCollidable;
}