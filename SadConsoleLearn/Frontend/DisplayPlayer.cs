using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Entity;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Frontend;

public class DisplayPlayer {
    private IEntityManager _entityManager;
    private IComponentManager _componentManager;
    private Console _console;
    private Entity _player;
    
    public DisplayPlayer(IEntityManager entityManager, IComponentManager componentManager, Console console, Entity player) {
        _entityManager = entityManager;
        _componentManager = componentManager;
        _console = console;
        _player = player;
    }
    
    public void Display() {
        var stats = _componentManager.GetComponent<StatsComponent>(_player.Id);
        var name = _componentManager.GetComponent<NameComponent>(_player.Id);
        var health = _componentManager.GetComponent<HealthComponent>(_player.Id).CurrentHealth;
        var maxHealth = _componentManager.GetComponent<HealthComponent>(_player.Id).MaxHealth;
        _console.Print(5, 0, "Character Sheet", Color.White, Color.Black);
        _console.Print(2, 2, $"Name: {name}", Color.White, Color.Black);
        _console.Print(2, 3, $"Health: {health}/{maxHealth}", Color.White, Color.Black);
        _console.Print(2, 4, $"Attack: {stats.Attack}", Color.White, Color.Black);
        _console.Print(2, 5, $"Defense: {stats.Defense}", Color.White, Color.Black);
    }
}