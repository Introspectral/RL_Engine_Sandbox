namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IEventManager {
    void Subscribe<T>(Action<T> action) where T : IEvent;
    public void SubscribeOnce<T>(Action<T> action, bool resubscribe = false) where T : IEvent;
    void Unsubscribe<T>(Action<T> action) where T : IEvent;
    void Publish<T>(T eventData) where T : IEvent;
    public void ProcessEvents(){}
    
}