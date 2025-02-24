using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class StatsComponent : IComponents {
    // This will be a component for stats
    // would likely need to be split into multiple components in a bigger game
    
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    
    public StatsComponent(int attack, int defense, int speed, int health, int maxHealth)
    {
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Health = health;
        MaxHealth = maxHealth;
    }
}