using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class PlayerControlledComponent : IComponents
{
    public bool IsPlayer { get; set; }
    
    public PlayerControlledComponent(bool isPlayer)
    {
        IsPlayer = isPlayer;
    }

}