using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Entity.Actors;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Entity;

public class EntityFactory
{
    private readonly IEntityManager _entityManager;
    private readonly IComponentManager _componentManager;

    public PlayerInfo PlayerInfo;

    public EntityFactory(IEntityManager entityManager, IComponentManager componentManager)
    {
        _entityManager = entityManager;
        _componentManager = componentManager;
    }
    // Create Actor
    #region Actors

    public (Entity, PlayerInfo) CreatePlayer()
    {
        PlayerInfo = new PlayerInfo();
        var playerClass = SetClass();
        var playerStats = SetStats(playerClass);
        var playerLevel = SetLevel();

        
        var entityBuilder = new EntityBuilder(_entityManager, _componentManager);
        
        Entity player = entityBuilder
            .WithPlayerControlled()
            .WithClass(playerClass)
            .WithName("Player")
            .WithStats(playerStats)
            .WithPosition(12, 12)
            .WithRendering('@', Color.White, Color.Black)
            .WithInventory()
            .Build();

        var playerInfo = new PlayerInfo
        {
            Name = "Player",
            Class = playerClass,
            Level = playerLevel.Level,
            Experience = playerLevel.Experience,
            Attack = playerStats.Attack,
            Defense = playerStats.Defense,
            Speed = playerStats.Speed,
            Health = playerStats.Health,
            MaxHealth = playerStats.MaxHealth,
            Gold = 0
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
        var health = playerClass switch
        {
            ClassComponent.ClassType.Warrior => 25,
            ClassComponent.ClassType.Mage => 13,
            ClassComponent.ClassType.Rogue => 18,
            _ => 0
        };
        var maxHealth = health;
        return new StatsComponent(attack, defense, speed, health, maxHealth);
    }

    private ClassComponent.ClassType SetClass()
    {
        return ClassComponent.ClassType.Warrior;
    }

    private LevelComponent SetLevel()
    {
        return new LevelComponent(1, 0);
    }

    #endregion

    // Create Items
    #region Items



    #endregion

    // Create MapEntities
    #region MapEntities

    public Entity CreateStairUp()
    {
        var entityBuilder = new EntityBuilder(_entityManager, _componentManager);
        Entity stairsUp = entityBuilder
            .WithPosition(12, 25)
            .WithRendering('>', Color.White, Color.Black)
            .Build();
        return stairsUp;
    }
    public Entity CreateStairDown()
    {
        var entityBuilder = new EntityBuilder(_entityManager, _componentManager);
        Entity stairsDown = entityBuilder
            .WithPosition(27, 16)
            .WithRendering('<', Color.White, Color.Black)
            .Build();
        return stairsDown;
    }
    #endregion
}