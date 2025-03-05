using SadConsole.Input;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IInputHandler
{
    public void ProcessInput(Keyboard keyboard, GameState currentState);
}