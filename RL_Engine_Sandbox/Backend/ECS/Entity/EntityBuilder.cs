using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Entity;

public class EntityBuilder : IEntityBuilder
{
    private readonly IEntityManager _entityManager;
    private readonly IComponentManager _componentManager;
    private readonly Entity _entity;
    
    public EntityBuilder(IEntityManager entityManager, IComponentManager componentManager) 
    {
        _entityManager = entityManager;
        _componentManager = componentManager;
        _entity = new Entity();
    }
    public EntityBuilder WithPosition(int x, int y)
    {
        _componentManager.AddComponent(_entity.Id, new PositionComponent(x, y));
        return this;
    }
    
    public EntityBuilder WithRendering(char glyph, Color foreground, Color background)
    {
        _componentManager.AddComponent(_entity.Id, new RenderingComponent(new ColoredGlyph(foreground, background, glyph)));
        return this;
    }
    public EntityBuilder WithPlayerControlled()
    {
        _componentManager.AddComponent(_entity.Id, new PlayerControlledComponent(true));
        return this;
    }
    
    public EntityBuilder WithStats(StatsComponent stats)
    {
        _componentManager.AddComponent(_entity.Id, stats);
        return this;
    }
    
    public EntityBuilder WithInventory()
    {
        _componentManager.AddComponent(_entity.Id, new InventoryComponent(new Dictionary<string, Item.Item>()));
        return this;
    }
    public EntityBuilder WithName(string name)
    {
        _componentManager.AddComponent(_entity.Id, new NameComponent(name));
        return this;
    }

    public EntityBuilder WithClass(ClassComponent.ClassType classType)
    {
        _componentManager.AddComponent(_entity.Id, new ClassComponent(classType));
        return this;
    }
    
    public Entity Build()
    {
        _entityManager.AddEntity(_entity);
        
        return _entity;
    }
    
}