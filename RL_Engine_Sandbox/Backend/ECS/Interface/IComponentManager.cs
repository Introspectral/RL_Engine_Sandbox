namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IComponentManager
{
    public void AddComponent(long entityId, IComponents component);
    public void RemoveComponent(long entityId, IComponents component);
    public void RemoveAllComponents(long entityId);
    public T? GetComponent<T>(long entityId) where T : class, IComponents;
    public bool HasComponent<T>(long entityId) where T : class, IComponents;
}