using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public class GameStateChangedEvent : IEvent
{
    public GameState NewState { get; }

    public GameStateChangedEvent(GameState newState)
    {
        NewState = newState;
    }
}
