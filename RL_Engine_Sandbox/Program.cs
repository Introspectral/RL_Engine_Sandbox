#region Using Statements

using Microsoft.Extensions.DependencyInjection;
using RL_Engine_Sandbox.Backend;
using RL_Engine_Sandbox.Backend.ECS.Entity;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Manager;
using RL_Engine_Sandbox.Backend.ECS.Map;
using RL_Engine_Sandbox.Backend.ECS.Systems;
using RL_Engine_Sandbox.Backend.ECS.Systems.Core;
using RL_Engine_Sandbox.Frontend;
using RL_Engine_Sandbox.Frontend.Interface;
using RL_Engine_Sandbox.Frontend.Manager.UiManager;
using RL_Engine_Sandbox.Frontend.Screens;
using SadConsole.Configuration;

#endregion

namespace RL_Engine_Sandbox;
class Program {
    static void Main(string[] args) {

        int width = 150;
        int height = 45;

        var services = new ServiceCollection()
            .AddSingleton<IEventManager, EventManager>()
            .AddSingleton<IEntityManager, EntityManager>()
            .AddSingleton<IComponentManager, ComponentManager>()
            .AddSingleton<IMovementSystem, MovementSystem>()
            .AddSingleton<IRenderingSystem, RenderingSystem>()
            .AddSingleton<ICollisionSystem, CollisionSystem>()
            .AddSingleton<IEntityFactory, EntityFactory>()
            .AddSingleton<IUiManager, UiManager>();
            

        
        var serviceProvider = services.BuildServiceProvider();
        var componentManager = serviceProvider.GetRequiredService<IComponentManager>();

        
        ServiceLocator.Register<IEntityFactory>(serviceProvider.GetRequiredService<IEntityFactory>());
        ServiceLocator.Register<IComponentManager>(componentManager);
        
        
        // Setup SadConsole Settings
        Settings.WindowTitle = "LikeRogue";
        Settings.ResizeMode = Settings.WindowResizeOptions.Fit;
        var configuration = new Builder()
            .SetScreenSize(width, height)
            .SetStartingScreen(_ => new RootScreen(
                serviceProvider.GetRequiredService<IEventManager>(),
                serviceProvider.GetRequiredService<IEntityManager>(),
                serviceProvider.GetRequiredService<IComponentManager>(),
                serviceProvider.GetRequiredService<IUiManager>(),
                width, height))
            .OnStart(StartUp);
        
        // Run the game
        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }
    private static void StartUp(object? sender, GameHost e) {}
}
