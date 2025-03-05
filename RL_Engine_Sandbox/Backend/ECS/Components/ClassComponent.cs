using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class ClassComponent(ClassComponent.ClassType classType) : IComponents
{
    public enum ClassType
    {
        Warrior,
        Mage,
        Rogue
    }

    public ClassType Class { get; set; } = classType;
}