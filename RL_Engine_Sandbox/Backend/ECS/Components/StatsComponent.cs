using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class StatsComponent(int attack, int defense, int speed) : IComponents
{
    // This will be a component for stats
    // would likely need to be split into multiple components in a bigger game


    public int Attack { get; set; } = attack;
    public int Defense { get; set; } = defense;
    public int Speed { get; set; } = speed;
}