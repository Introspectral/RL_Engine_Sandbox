using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Manager;

public class GameStateManager : IGameStateManager
{
    private GameState _currentState = GameState.Run;
    private readonly IEventManager _eventManager;

    public GameStateManager(IEventManager eventManager)
    {
        _eventManager = eventManager;
        _eventManager.Subscribe<GameStateChangedEvent>(OnGameStateChanged);
    }

    public GameState GetCurrentState() => _currentState;

    private void OnGameStateChanged(GameStateChangedEvent gameStateChangedEvent)
    {
        _currentState = gameStateChangedEvent.NewState;
    }
}
