using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class PlayerControlledComponent(bool isPlayer) : IComponents
{
    public bool IsPlayer { get; set; } = isPlayer;
}