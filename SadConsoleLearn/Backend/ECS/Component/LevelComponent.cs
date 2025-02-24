using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class LevelComponent : IComponents
{
    public int Level { get; set; }
    public int Experience { get; set; }
    
    public LevelComponent(int level, int experience) {
        Level = level;
        Experience = experience;
    }
}