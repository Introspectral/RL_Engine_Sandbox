using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class HealthComponent(int maxHealth, int currentHealth) : IComponents
{
    public int MaxHealth { get; set; } = maxHealth;
    public int CurrentHealth { get; set; } = currentHealth;
}