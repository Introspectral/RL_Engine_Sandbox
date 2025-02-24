using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class NameComponent : IComponents
{
    public string Name { get; set; }
    public NameComponent(string name){
        Name = name;
    }
}