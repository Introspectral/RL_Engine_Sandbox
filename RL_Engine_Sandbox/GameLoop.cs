#region Usings

using RL_Engine_Sandbox.Backend;
using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Entities.Actors;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Manager;
using RL_Engine_Sandbox.Backend.ECS.Map;
using RL_Engine_Sandbox.Backend.ECS.Systems;
using RL_Engine_Sandbox.Frontend.Interface;
using RL_Engine_Sandbox.Frontend.UI.Screens;
using RL_Engine_Sandbox.Frontend.UI.Screens.SubScreens;
using RL_Engine_Sandbox.Frontend.UI.UiManager;
using SadConsole.Input;

#endregion

namespace RL_Engine_Sandbox;
public class GameLoop : ScreenObject
{
    #region Properties
    private readonly IEventManager _eventManager;//
    private readonly GameStateManager _gameStateManager;//
    private Render_System _renderSystem;//
    private readonly IEntityManager _entityManager;//
    private readonly IComponentManager _componentManager;//
    private IMapManager _mapManager;//
    private readonly IUiManager _uiManager;//
    private Input_System _inputSystem;//
    private IMapFactory _mapFactory;
    private IEntityFactory _entityFactory;
    private readonly IMapManagerFactory _mapManagerFactory; // Needs to be encapsulated
    private readonly IMovementSystem _movementSystem;
    private IFovSystem _fovSystem;
    private readonly ICollisionSystem _collisionSystem;
    private readonly IInventoryManager _inventoryManager;
    private readonly IMessageLogSystem _messageLogSystem;
    private readonly Interaction_System _interactionSystem;
    public EntityInfo _playerInfo;
    private readonly int _width;
    private readonly int _height;

    private List<Entity> _entitiesToRender;
    public long PlayerId;
    private GameState _currentState = GameState.Run;
    #endregion
    public GameLoop(
        #region Constructor Parameters

        IEventManager eventManager, IEntityManager entityManager, IComponentManager componentManager,
        IUiManager uiManager, IMovementSystem movementSystem, ICollisionSystem collisionSystem,
        IFovSystem fovSystem, IEntityFactory entityFactory, IMapManagerFactory mapManagerFactory,
        IMapManager mapManager, IMessageLogSystem messageLogSystem, int width = 150, int height = 45)

        #endregion
    {
        #region Constructor Assignments
        IsFocused = true;
        UseKeyboard = true;
        _eventManager = eventManager;
        _entityManager = entityManager;
        _componentManager = componentManager;
        _uiManager = uiManager;
        _movementSystem = movementSystem;
        _collisionSystem = collisionSystem;
        _fovSystem = fovSystem;
        _entityFactory = entityFactory;
        _entityManager = entityManager;
        _mapManagerFactory = mapManagerFactory;
        _mapManager = mapManager;
        _entitiesToRender = new List<Entity>();
        _renderSystem = new Render_System(_mapManager, _componentManager, _fovSystem, _eventManager, 0);
        _componentManager = componentManager;
        _messageLogSystem = messageLogSystem;
        _gameStateManager = new GameStateManager(eventManager);
        _width = width;
        _height = height;
        _mapFactory = new MapFactory(width, height);
        _playerInfo = new EntityInfo();
        _inventoryManager = new InventoryManager(_componentManager);
        _interactionSystem = new Interaction_System(_eventManager, _entityManager, _componentManager, _inventoryManager, _mapManager);

        #endregion
        InitializeEntities(entityFactory);
        InitializeUi();
        InitializeMap();
        InitializeInput();
        InitializeRendering();
        
        Game.Instance.FocusedScreenObjects.Set(this);
        _eventManager.Subscribe<GameStateChangedEvent>(OnGameStateChanged);
        _eventManager.Subscribe<RemoveEntityEvent>(OnRemoveEntity);
    }
    public override void Update(TimeSpan delta)
    {
        base.Update(delta);
        switch (_currentState)
        {
            case GameState.Run:
                _uiManager.Update();
                _renderSystem.RenderAll();
                _eventManager.ProcessEvents();
                break;
            case GameState.Inventory:
                // Handle inventory screen rendering
                //_uiManager.GetUiElement("inventoryScreen")?.Update();
                break;
        }
    }
    public override bool ProcessKeyboard(Keyboard keyboard) {
        _inputSystem.ProcessInput(keyboard, _gameStateManager.GetCurrentState());
        return true;
    }
    
    #region Initialization
    private void InitializeEntities(IEntityFactory entityFactory) {
        var (player, playerInfo) = entityFactory.CreatePlayer();
        PlayerId = player.Id;
        _playerInfo = playerInfo;
        var stairsUp = entityFactory.CreateStairUp();
        var stairDown = entityFactory.CreateStairDown();
        var sword = entityFactory.CreateSword();
        var shield = entityFactory.CreateShield();
        var potion = entityFactory.CreatePotion();
        _mapManager.CurrentMap.Entities.Add(stairsUp);
        _mapManager.CurrentMap.Entities.Add(stairDown);
        _mapManager.CurrentMap.Entities.Add(sword);
        _mapManager.CurrentMap.Entities.Add(shield);
        _mapManager.CurrentMap.Entities.Add(potion);
        _mapManager.CurrentMap.Entities.Add(player);
    }

    private void InitializeUi() {
        var gameAreaPanelConfig = GameAreaPanelConfig(out var characterSheetPanelConfig, out var messageLogPanelConfig);
        RegisterUiElements(gameAreaPanelConfig, characterSheetPanelConfig, messageLogPanelConfig);
        _uiManager.SetScreenSize(_width, _height);
    }

    private void InitializeMap() {
        var gameAreaConsole = _uiManager.GetUiElement("gameAreaPanel");

        var gameScreen = new GameScreen(_eventManager, gameAreaConsole.GetContentConsole().Width,
            gameAreaConsole.GetContentConsole().Height, _uiManager);
        Children.Add(gameScreen);

        _fovSystem = new FOV_System(_mapManager, _componentManager);
        foreach (var entity in _mapManager.CurrentMap.Entities)
        {
            var position = _componentManager.GetComponent<PositionComponent>(entity.Id);
            if (position == null) continue;
            var spawnPosition = _mapManager.FindValidSpawnPoint();
            position.X = spawnPosition.X;
            position.Y = spawnPosition.Y;
        }
    }

    private void InitializeInput() {
        _inputSystem = new Input_System(_eventManager, PlayerId);
    }

    private void InitializeRendering() {
        var gameAreaConsole = _uiManager.GetUiElement("gameAreaPanel");
        _renderSystem =
            new Render_System(_mapManager, _componentManager, _fovSystem, _eventManager, PlayerId)
            {
                Console = gameAreaConsole.GetContentConsole()
            };
        foreach (var i in _mapManager.CurrentMap.Entities) _renderSystem.EntitiesToRender.Add(i);
    }
    #endregion
    #region UI Initialization
    private void RegisterUiElements(UiLayoutConfig gameAreaPanelConfig, UiLayoutConfig characterSheetPanelConfig,
        UiLayoutConfig messageLogPanelConfig)
    {
        _uiManager.AddUiElement("gameAreaPanel", new GameAreaPanel(120, 30, new Point(0, 0)), gameAreaPanelConfig);
        _uiManager.AddUiElement("messageLogPanel", new MessageLogPanel(120, 15, new Point(0, 30), _eventManager),
            messageLogPanelConfig);
        _uiManager.AddUiElement("characterSheetPanel",
            new CharacterSheetPanel(30, 30, new Point(100, 0), _eventManager, _componentManager, PlayerId),
            characterSheetPanelConfig);
    }
    private UiLayoutConfig GameAreaPanelConfig(out UiLayoutConfig characterSheetPanelConfig,
        out UiLayoutConfig messageLogPanelConfig)
    {
        var gameAreaPanelConfig = new UiLayoutConfig(0f, 0f, 120f / _width, 35f / _height);
        characterSheetPanelConfig = new UiLayoutConfig(120f / _width, 0f, 30f / _width, 1f);
        messageLogPanelConfig = new UiLayoutConfig(0f, 35f / _height, 120f / _width, 10f / _height);
        return gameAreaPanelConfig;
    }
    #endregion
    #region Event Handlers

    private void OnRemoveEntity(RemoveEntityEvent obj)
    {
        _entitiesToRender.RemoveAll(e => e.Id == obj.EntityId);
        _renderSystem.EntitiesToRender.RemoveAll(e => e.Id == obj.EntityId);
    }

    private void OnGameStateChanged(GameStateChangedEvent gameStateChangedEvent)
    {
        _currentState = gameStateChangedEvent.NewState;
        _uiManager.HandleGameStateChange(_currentState);
    }

    #endregion
}
