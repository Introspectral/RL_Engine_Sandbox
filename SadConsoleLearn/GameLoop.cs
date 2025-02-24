using RL_Engine_Sandbox.Backend.ECS.Entity;
using RL_Engine_Sandbox.Backend.ECS.Entity.Actors;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Map;
using RL_Engine_Sandbox.Backend.ECS.Systems;
using RL_Engine_Sandbox.Frontend;
using RL_Engine_Sandbox.Frontend.Interface;
using RL_Engine_Sandbox.Frontend.Manager.UiManager;
using RL_Engine_Sandbox.Frontend.Screens;
using RL_Engine_Sandbox.Frontend.Screens.SubScreens;
using SadConsole.Input;

namespace RL_Engine_Sandbox
{
    internal class GameLoop : ScreenObject
    {
        private GameScreen _gameScreen;
        private InputHandler _inputHandler;
        private RenderingSystem _renderingSystem;
        private MovementSystem _movementSystem;
        private IEventManager _eventManager;
        private IUiManager _uiManager;
        private PlayerInfo _playerInfo;
        private MapRenderer _mapRenderer;
        private int _width;
        private int _height;
        
        public GameLoop(
            IEventManager eventManager, 
            IEntityManager entityManager, 
            IComponentManager componentManager, 
            IUiManager uiManager,
            EntityBuilder entityBuilder,
            EntityFactory entityFactory,
            int width,
            int height
            ){
            IsFocused = true;
            UseKeyboard = true;
            _eventManager = eventManager;
            _uiManager = uiManager;
            _width = width;
            _height = height;
            
            // Define layout configurations (relative to the screen size).
            var gameAreaPanelConfig = new UiLayoutConfig(0f, 0f, 120f / 150f, 35f / 45f);
            var characterSheetPanelConfig = new UiLayoutConfig(120f / 150f, 0f, 30f / 150f, 45f / 45f);
            var messageLogPanelConfig = new UiLayoutConfig(0f, 35f / 45f, 120f / 150f, 10f / 45f);
            // Create a player entity and add it to the EntityManager.
            var stairsUp = entityFactory.CreateStairUp();
            var stairsDown = entityFactory.CreateStairDown();
            var (player, playerEntityInfo) = entityFactory.CreatePlayer();
            
            // Register UI elements with the UIManager.
            _uiManager.AddUiElement("gameAreaPanel",
                new GameAreaPanel(120, 30, new Point(0, 0)),
                gameAreaPanelConfig);
            _uiManager.AddUiElement("characterSheetPanel",
                new CharacterSheetPanel(30, 30, new Point(100, 0), eventManager, playerEntityInfo),
                characterSheetPanelConfig);
            _uiManager.AddUiElement("messageLogPanel", 
                new MessageLogPanel(120, 15, new Point(0, 30), eventManager),
                messageLogPanelConfig);
            
            // Apply layout based on the screen dimensions.
            _uiManager.SetScreenSize(width, height);
            
            // Create GameScreen (which attaches UI elements once) and add it as a child.
            _gameScreen = new GameScreen(_eventManager, 
                _uiManager.GetUiElement("gameAreaPanel").GetContentConsole().Width, 
                _uiManager.GetUiElement("gameAreaPanel").GetContentConsole().Height, 
                _uiManager);
            Children.Add(_gameScreen);
            Game.Instance.FocusedScreenObjects.Set(this);
            //var caveMap = new CaveMap(_gameScreen.GameConsole.Width, _gameScreen.GameConsole.Height);
            var dungeonMap = new DungeonMap(_gameScreen.GameConsole.Width, _gameScreen.GameConsole.Height);
            _mapRenderer =new MapRenderer(dungeonMap, _gameScreen.GameConsole);
            
            // Retrieve the GameAreaPanel's console for the RenderingSystem.
            var gameAreaConsole = _uiManager.GetUiElement("gameAreaPanel");
            _renderingSystem = new RenderingSystem(entityManager, componentManager, gameAreaConsole.GetContentConsole());

            _inputHandler = new InputHandler(eventManager, player.Id);
            _movementSystem = new MovementSystem(entityManager, componentManager, eventManager);
        }

        public override void Update(TimeSpan delta) {
            base.Update(delta);
            _gameScreen.Update(delta);
            _renderingSystem.Update();
            _mapRenderer.Render();
            _eventManager.ProcessEvents();
        }
        
        public override bool ProcessKeyboard(Keyboard keyboard) {
            _inputHandler.ProcessInput(keyboard);
            return true;
        }
    }
}

