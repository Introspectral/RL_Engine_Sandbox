using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities.Actors;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Entities;

public class EntityFactory(IEntityManager entityManager, IComponentManager componentManager)
    : IEntityFactory
{
    public EntityInfo EntityInfo;

    // Create Actor

    #region Actors

    public (Entity, EntityInfo) CreatePlayer()
    {
        EntityInfo = new EntityInfo();
        var playerClass = SetClass();
        var playerStats = SetStats(playerClass);
        var playerLevel = SetLevel();
        var playerHealth = SetHealth(playerClass);
        var playerInventory = new InventoryComponent();
        
        var entityBuilder = new EntityBuilder(entityManager, componentManager);
        var player = entityBuilder
            .WithPlayerControlled()
            .WithClass(playerClass)
            .WithName("Player")
            .WithStats(playerStats)
            .WithHealth(playerHealth)
            .WithPosition(12, 12)
            .WithRendering('@', Color.White, Color.Black)
            .WithInventory()
            .WithFov(new FovComponent(5))
            .Build();

        var playerInfo = new EntityInfo
        {
            Id = player.Id,
            Name = "Player",
            Class = playerClass,
            Level = playerLevel.Level,
            Experience = playerLevel.Experience,
            Attack = playerStats.Attack,
            Defense = playerStats.Defense,
            Speed = playerStats.Speed,
            Health = playerHealth.CurrentHealth,
            MaxHealth = playerHealth.MaxHealth,
            Gold = 0,
            Inventory = playerInventory,
        };
        return (player, playerInfo);
    }

    private StatsComponent SetStats(ClassComponent.ClassType playerClass)
    {
        var attack = playerClass switch
        {
            ClassComponent.ClassType.Warrior => 10,
            ClassComponent.ClassType.Mage => 5,
            ClassComponent.ClassType.Rogue => 7,
            _ => 0
        };
        var defense = playerClass switch
        {
            ClassComponent.ClassType.Warrior => 5,
            ClassComponent.ClassType.Mage => 3,
            ClassComponent.ClassType.Rogue => 2,
            _ => 0
        };
        var speed = playerClass switch
        {
            ClassComponent.ClassType.Warrior => 5,
            ClassComponent.ClassType.Mage => 7,
            ClassComponent.ClassType.Rogue => 10,
            _ => 0
        };

        return new StatsComponent(attack, defense, speed);
    }

    private ClassComponent.ClassType SetClass()
    {
        return ClassComponent.ClassType.Mage;
    }

    private LevelComponent SetLevel()
    {
        return new LevelComponent(1, 0);
    }

    private HealthComponent SetHealth(ClassComponent.ClassType playerClass)
    {
        var health = playerClass switch
        {
            ClassComponent.ClassType.Warrior => 25,
            ClassComponent.ClassType.Mage => 13,
            ClassComponent.ClassType.Rogue => 18,
            _ => 0
        };
        var maxHealth = health;
        return new HealthComponent(health, maxHealth);
    }

    #endregion

    // Create Items

    #region Items

    public Entity CreateSword()
    {
        var entityBuilder = new EntityBuilder(entityManager, componentManager);
        var sword = entityBuilder
            .WithPosition(15, 15)
            .WithName("Sword")
            .WithItem(new ItemComponent())
            .WithRendering('/', Color.White, Color.Black)
            .Build();

        return sword;
    }

    public Entity CreateShield()
    {
        var entityBuilder = new EntityBuilder(entityManager, componentManager);
        var shield = entityBuilder
            .WithPosition(16, 15)
            .WithName("Shield")
            .WithItem(new ItemComponent())
            .WithRendering('0', Color.DarkOrange, Color.Black)
            .Build();
 
        return  shield;
    }

    public Entity CreatePotion()
    {
        var entityBuilder = new EntityBuilder(entityManager, componentManager);
        var potion = entityBuilder
            .WithPosition(17, 15)
            .WithName("Health Potion")
            .WithItem(new ItemComponent())
            .WithRendering('!', Color.Crimson, Color.Black)
            .Build();

        return  potion;
    }


    #endregion

    // Create MapEntities

    #region MapEntities

    public Entity CreateStairUp()
    {
        var stairsUp = new EntityBuilder(entityManager, componentManager)
            .WithPosition(12, 25)
            .WithName("Stairs Up")
            .WithMapEntity(new MapEntityComponent())
            .WithRendering('>', Color.White, Color.Black)
            .WithStairs(new StairsComponent(StairDirection.Up))  // <--- add it
            .Build();
        return stairsUp;
    }

    public Entity CreateStairDown()
    {
        var entityBuilder = new EntityBuilder(entityManager, componentManager);
        var stairsDown = entityBuilder
            .WithPosition(27, 16)
            .WithName("Stairs Down")
            .WithMapEntity(new MapEntityComponent())
            .WithRendering('<', Color.White, Color.Black)
            .WithStairs(new StairsComponent(StairDirection.Down))  // <--- add it
            .Build();
        return stairsDown;
    }

    #endregion
}