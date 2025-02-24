using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using SadConsole.Input;
using Direction = RL_Engine_Sandbox.Backend.ECS.Event.Direction;

namespace RL_Engine_Sandbox.Backend.ECS.Systems.Core
{
    internal class InputHandler {
        private readonly long _playerId;
        private readonly IEventManager _eventManager;
        public InputHandler(IEventManager eventManager, long playerId) {
            _eventManager = eventManager;
            _playerId = playerId;
        }

        public void ProcessInput(Keyboard keyboard) {
            if (keyboard.IsKeyPressed(Keys.Up)) {
                _eventManager.Publish(new MessageEvent("Up"));
                _eventManager.Publish(new MoveActionEvent(_playerId,Direction.Up));
            }
            if (keyboard.IsKeyPressed(Keys.Down)) {
                _eventManager.Publish(new MessageEvent("Down"));
                _eventManager.Publish(new MoveActionEvent(_playerId, Direction.Down));
            }
            if (keyboard.IsKeyPressed(Keys.Left)) {
                _eventManager.Publish(new MessageEvent("Left"));
                _eventManager.Publish(new MoveActionEvent(_playerId, Direction.Left));
            }
            if (keyboard.IsKeyPressed(Keys.Right)) {
                _eventManager.Publish(new MessageEvent("Right"));
                _eventManager.Publish(new MoveActionEvent(_playerId, Direction.Right));
            }
        }
    }
}
