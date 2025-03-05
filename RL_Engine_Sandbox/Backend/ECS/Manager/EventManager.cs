using System.Collections.Concurrent;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Manager;

public class EventManager : IEventManager
{
    private readonly Queue<IEvent> _eventQueue = new();
    private readonly ConcurrentDictionary<Type, List<Delegate>> _subscribers = new();

    public void Subscribe<T>(Action<T> action) where T : IEvent
    {
        var eventType = typeof(T);
        if (!_subscribers.ContainsKey(eventType)) _subscribers[eventType] = [];
        _subscribers[eventType].Add(action);
    }

    public void Unsubscribe<T>(Action<T> action) where T : IEvent
    {
        var eventType = typeof(T);
        if (!_subscribers.ContainsKey(eventType)) return;
        _subscribers[eventType].Remove(action);

        if (_subscribers[eventType].Count == 0) _subscribers.TryRemove(eventType, out _);
    }

    // A potential one-time subscription method that automatically unsubscribes after the first event
    // This is useful, for example when you want to subscribe to a "game over" event that should only happen once,
    // or when you want to subscribe to a "player hit" event that should only happen once, but
    // you want to resubscribe after the event has been processed so that it can happen again.
    public void SubscribeOnce<T>(Action<T> action, bool resubscribe = false) where T : IEvent
    {
        Action<T> wrapper = null;
        wrapper = e =>
        {
            action(e); // Execute the action
            Unsubscribe(wrapper); // Remove after execution

            if (resubscribe) Subscribe(wrapper); // Resubscribe only if requested
        };
        Subscribe(wrapper);
    }


    public void Publish<T>(T eventData) where T : IEvent
    {
        _eventQueue.Enqueue(eventData);
    }

    // Process all events in the queue at the end of the loop cycle
    // This is useful because it allows you to publish events during event processing and 
    // not have them be processed until the next loop cycle.
    // Examples of this are when you want to publish an event that triggers
    // an entity to move. 
    public void ProcessEvents()
    {
        while (_eventQueue.Count > 0)
        {
            var eventData = _eventQueue.Dequeue();
            var eventType = eventData.GetType();
            if (!_subscribers.ContainsKey(eventType)) continue;
            foreach (var action in _subscribers[eventType].OfType<Delegate>()) action.DynamicInvoke(eventData);
        }
    }
}