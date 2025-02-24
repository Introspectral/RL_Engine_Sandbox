using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class HealthComponent : IComponents
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    
    public HealthComponent(int maxHealth, int currentHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
    }

    
}