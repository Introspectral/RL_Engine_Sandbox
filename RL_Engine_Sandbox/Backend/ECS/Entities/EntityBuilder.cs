using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Entities;

public class EntityBuilder(IEntityManager entityManager, IComponentManager componentManager)
    : IEntityBuilder
{
    private readonly Entity _entity = new();

    public EntityBuilder WithPosition(int x, int y)
    {
        componentManager.AddComponent(_entity.Id, new PositionComponent(x, y));
        return this;
    }

    public EntityBuilder WithRendering(char glyph, Color foreground, Color background)
    {
        componentManager.AddComponent(_entity.Id,
            new RenderingComponent(new ColoredGlyph(foreground, background, glyph)));
        return this;
    }

    public EntityBuilder WithPlayerControlled()
    {
        componentManager.AddComponent(_entity.Id, new PlayerControlledComponent(true));
        return this;
    }

    public EntityBuilder WithStats(StatsComponent stats)
    {
        componentManager.AddComponent(_entity.Id, stats);
        return this;
    }

    public EntityBuilder WithInventory()
    {
        var inventory = new InventoryComponent();
        componentManager.AddComponent(_entity.Id, inventory);
        return this;
    }

    public EntityBuilder WithName(string name)
    {
        componentManager.AddComponent(_entity.Id, new NameComponent(name));
        return this;
    }

    public EntityBuilder WithClass(ClassComponent.ClassType classType)
    {
        componentManager.AddComponent(_entity.Id, new ClassComponent(classType));
        return this;
    }

    public EntityBuilder WithFov(FovComponent fov)
    {
        componentManager.AddComponent(_entity.Id, fov);
        return this;
    }

    public EntityBuilder WithHealth(HealthComponent health)
    {
        componentManager.AddComponent(_entity.Id, health);
        return this;
    }

    public EntityBuilder WithItem(ItemComponent item)
    {
        componentManager.AddComponent(_entity.Id, item);
        return this;
    }

    public EntityBuilder WithMapEntity(MapEntityComponent mapEntity)
    {
        componentManager.AddComponent(_entity.Id, mapEntity);
        return this;
    }

    public EntityBuilder WithStairs(StairsComponent stairsComponent)
    {
        componentManager.AddComponent(_entity.Id, stairsComponent);
        return this;
    }

    public Entity Build()
    {
        entityManager.AddEntity(_entity);

        return _entity;
    }
}