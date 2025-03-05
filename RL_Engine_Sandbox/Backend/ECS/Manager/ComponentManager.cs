using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Manager;

public class ComponentManager : IComponentManager
{
    private readonly Dictionary<long, List<IComponents>> _entityComponents = new();

    public void AddComponent(long entityId, IComponents component)
    {
        if (!_entityComponents.ContainsKey(entityId))
            _entityComponents[entityId] = new List<IComponents>();

        _entityComponents[entityId].Add(component);
    }

    public void RemoveComponent(long entityId, IComponents component)
    {
        _entityComponents[entityId]?.Remove(component);
    }

    public void RemoveAllComponents(long entityId)
    {
        _entityComponents.Remove(entityId);
    }

    public T? GetComponent<T>(long entityId) where T : class, IComponents
    {
        return _entityComponents.TryGetValue(entityId, out var components)
            ? components.OfType<T>().FirstOrDefault()
            : null;
    }
    public bool HasComponent<T>(long entityId) where T : class, IComponents
    {
        return _entityComponents.TryGetValue(entityId, out var components) && components.OfType<T>().Any();
    }

}