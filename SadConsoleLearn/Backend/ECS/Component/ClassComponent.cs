using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class ClassComponent : IComponents
{
    public enum ClassType {
        Warrior,
        Mage,
        Rogue
    }
    public ClassType Class { get; set; }
    public ClassComponent(ClassType classType) {
        Class = classType;
    }
    
}