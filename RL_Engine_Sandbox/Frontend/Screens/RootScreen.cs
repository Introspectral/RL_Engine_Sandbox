﻿using RL_Engine_Sandbox.Backend.ECS.Entity;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Frontend.Interface;

namespace RL_Engine_Sandbox.Frontend.Screens
{
    public class RootScreen : ScreenObject
    {
        public RootScreen(IEventManager eventManager, IEntityManager entityManager, IComponentManager componentManager, IUiManager uiManager, EntityBuilder entityBuilder, EntityFactory entityFactory, int width, int height)
        {
            var gameLoop = new GameLoop(eventManager, entityManager, componentManager, uiManager, entityBuilder, entityFactory, width, height);
            Children.Add(gameLoop);
            
            SadConsole.Game.Instance.Screen = gameLoop;
            SadConsole.Game.Instance.DestroyDefaultStartingConsole();
        }
    }
}