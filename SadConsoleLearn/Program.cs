#region Using Statements

using Microsoft.Extensions.DependencyInjection;
using RL_Engine_Sandbox.Backend.ECS.Entity;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Manager;
using RL_Engine_Sandbox.Backend.ECS.Systems;
using RL_Engine_Sandbox.Frontend;
using RL_Engine_Sandbox.Frontend.Interface;
using RL_Engine_Sandbox.Frontend.Manager.UiManager;
using RL_Engine_Sandbox.Frontend.Screens;
using SadConsole.Configuration;

#endregion

namespace RL_Engine_Sandbox;
class Program {
    static void Main(string[] args) {
        // Set the width and height of the game
        int width = 150;
        int height = 45;
        // Setup Dependency Injection
        var services = new ServiceCollection()
            .AddSingleton<IEventManager, EventManager>()
            .AddSingleton<IEntityManager, EntityManager>()
            .AddSingleton<IComponentManager, ComponentManager>()
            .AddSingleton<IRenderingSystem, RenderingSystem>()
            .AddSingleton<IMovementSystem, MovementSystem>()
            .AddSingleton<IRenderingSystem, RenderingSystem>()
            .AddSingleton<IUiManager, UiManager>()
            .AddSingleton<EntityBuilder>()
            .AddSingleton<EntityFactory>();
        
        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();
        
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
                serviceProvider.GetRequiredService<EntityBuilder>(),
                serviceProvider.GetRequiredService<EntityFactory>(),
                width, height))
            .OnStart(StartUp);
        
        // Run the game
        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }
    private static void StartUp(object? sender, GameHost e) {}
}
