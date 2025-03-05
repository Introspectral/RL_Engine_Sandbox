using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class LevelComponent(int level, int experience) : IComponents
{
    public int Level { get; set; } = level;
    public int Experience { get; set; } = experience;
}