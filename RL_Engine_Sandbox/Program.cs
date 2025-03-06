using Microsoft.Extensions.DependencyInjection;
using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Manager;
using RL_Engine_Sandbox.Backend.ECS.Map;
using RL_Engine_Sandbox.Backend.ECS.Systems;
using RL_Engine_Sandbox.Frontend.Interface;
using RL_Engine_Sandbox.Frontend.UI.Screens;
using RL_Engine_Sandbox.Frontend.UI.UiManager;
using SadConsole.Configuration;
namespace RL_Engine_Sandbox;

internal class Program
{
    private static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices();

        // Setup basic game settings
        var width = 150;
        var height = 45;
        Settings.WindowTitle = "LikeRogue";
        Settings.ResizeMode = Settings.WindowResizeOptions.Fit;

        var configuration = ConfigureGame(width, height, serviceProvider);

        // Run the game
        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection()
            .AddSingleton<IEventManager, EventManager>()
            .AddSingleton<IEntityManager, EntityManager>()
            .AddSingleton<IComponentManager, ComponentManager>()
            .AddSingleton<IMovementSystem, Movement_System>()
            .AddSingleton<ICollisionSystem, Collision_System>()
            .AddSingleton<IEntityFactory, EntityFactory>()
            .AddSingleton<IUiManager, UiManager>()
            .AddSingleton<IMapManagerFactory, MapManagerFactory>()
            .AddTransient<IMapFactory, MapFactory>()
            .AddSingleton<IMapManager, MapManager>()
            .AddSingleton<IFovSystem, FOV_System>()
            .AddSingleton<IMessageLogSystem, MessageLog_System>()
            .AddSingleton<IInventoryManager, InventoryManager>()
            .AddSingleton<IInputHandler, Input_System>()
            .AddSingleton<IGameStateManager, GameStateManager>()
            .AddSingleton<IRenderSystem, Render_System>()
            .AddTransient<IMap, Map>()
            .AddSingleton<GameLoop>()
            .AddSingleton<RootScreen>();

        services.AddTransient<IMapFactory>(provider => new MapFactory(120, 30));

        return services.BuildServiceProvider();
    }
    

    private static Builder ConfigureGame(int width, int height, ServiceProvider serviceProvider)
    {
        return new Builder()
            .SetScreenSize(width, height)
            .SetStartingScreen(_ => serviceProvider.GetRequiredService<RootScreen>())
            .OnStart(StartUp);
    }

    private static void StartUp(object? sender, GameHost e)
    {
        // Additional startup logic can go here if needed.
    }
}