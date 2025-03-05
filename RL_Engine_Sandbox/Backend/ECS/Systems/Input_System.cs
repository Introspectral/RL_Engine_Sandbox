using System.Text;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using SadConsole.Input;
using Direction = RL_Engine_Sandbox.Backend.ECS.Event.Direction;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

internal class Input_System(IEventManager eventManager, long playerId) : IInputHandler
{
    public void ProcessInput(Keyboard keyboard, GameState currentState)
    {
        foreach (var keyInfo in keyboard.KeysPressed)
        {
            char c = keyInfo.Character;
            if (c != '\0') // \0 means "no character"
            {
                // Convert to lowercase for uniform handling of 'g' or 'G'
                char lower = char.ToLower(c);

                if (c == '<' && currentState == GameState.Run)
                {
                    eventManager.Publish(new UseStairsEvent(playerId, StairDirection.Up));
                }
                else if (c == '>' && currentState == GameState.Run)
                {
                    eventManager.Publish(new UseStairsEvent(playerId, StairDirection.Down));
                }
            }
        }
        if (currentState == GameState.Run)
        {
            if (keyboard.IsKeyPressed(Keys.Up)) eventManager.Publish(new MoveActionEvent(playerId, Direction.Up));
            if (keyboard.IsKeyPressed(Keys.Down)) eventManager.Publish(new MoveActionEvent(playerId, Direction.Down));
            if (keyboard.IsKeyPressed(Keys.Left)) eventManager.Publish(new MoveActionEvent(playerId, Direction.Left));
            if (keyboard.IsKeyPressed(Keys.Right)) eventManager.Publish(new MoveActionEvent(playerId, Direction.Right));
            // PickUp
            if (keyboard.IsKeyPressed(Keys.G)) eventManager.Publish(new PickUpEvent(playerId));
            // Stairs
            // if (keyboard.IsKeyPressed(Keys.OemBackslash) && keyboard.IsKeyDown(Keys.LeftShift))
            //     eventManager.Publish(new UseStairsEvent(playerId, StairDirection.Up));
            // if (keyboard.IsKeyPressed(Keys.OemBackslash)  && !keyboard.IsKeyDown(Keys.LeftShift))
            //     eventManager.Publish(new UseStairsEvent(playerId, StairDirection.Down));
        }

        if (keyboard.IsKeyPressed(Keys.I))
        {
            GameState newState = (currentState == GameState.Run) ? GameState.Inventory : GameState.Run;
            eventManager.Publish(new GameStateChangedEvent(newState));
        }
    }
}
