using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class CollisionComponent : IComponents
{
    public bool IsCollidable { get; set; }
    
    public CollisionComponent(bool isCollidable){
        IsCollidable = isCollidable;
    }
}