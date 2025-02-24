using RL_Engine_Sandbox.Backend.ECS.Entity.Actors;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Frontend.Interface;
using Color = SadRogue.Primitives.Color;
using Point = SadRogue.Primitives.Point;
using Rectangle = SadRogue.Primitives.Rectangle;

namespace RL_Engine_Sandbox.Frontend.Screens.SubScreens
{
    public class CharacterSheetPanel : IUiElement {
        private Console _console;
        private int _width;
        private int _height;
        private readonly Point _position;
        private IEventManager _eventManager;
        private PlayerInfo _playerInfo;

        // Constructor now accepts a PlayerInfo object
        public CharacterSheetPanel(int width, int height, Point position, IEventManager eventManager, PlayerInfo playerInfo) {
            _width = width;
            _height = height;
            _position = position;
            _console = new Console(width, height) {
                Position = position,
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };
            _eventManager = eventManager;
            _playerInfo = playerInfo;
            // Subscribe to updates if the player's stats change
            _eventManager.Subscribe<StatChangeEvent>(OnStatChangeEvent);
        }

        // Event handler that updates the PlayerInfo based on incoming events
        private void OnStatChangeEvent(StatChangeEvent evt) {
            _playerInfo.Attack = evt.StatsComponent.Attack;
            _playerInfo.Defense = evt.StatsComponent.Defense;
            _playerInfo.Speed = evt.StatsComponent.Speed;
            // You can update additional stats here as needed.
            UpdatePanel();
            _console.IsDirty = true;
        }

        public Console GetConsole() => _console;

        public Console GetContentConsole() {
            throw new NotImplementedException();
        }

        // Initial panel setup: draws the box and the initial text.
        public void Initialize() {
            _console.DrawBox(
                new Rectangle(0, 0, _console.Width, _console.Height), 
                ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(Color.Bisque, Color.Black))
            );
            UpdatePanel();
        }

        // Method that updates the panel's printed information using the injected PlayerInfo.
        private void UpdatePanel() {

            _console.Print(5, 1, "Character Sheet");
            _console.Print(3, 3, $"Name:  {_playerInfo.Name}");
            _console.Print(3, 4, $"Class:  {_playerInfo.Class}");
            _console.Print(3, 5, $"Level:  {_playerInfo.Level}");
            _console.Print(3, 6, $"Xp:  {_playerInfo.Experience}/{_playerInfo.Experience}");
            _console.Print(3, 7, $"Health:  {_playerInfo.Health}/{_playerInfo.MaxHealth}");
            _console.Print(3, 8, $"Attack:  {_playerInfo.Attack}");
            _console.Print(3, 9, $"Defense:  {_playerInfo.Defense}");
            _console.Print(3, 10, $"Speed:  {_playerInfo.Speed}");
            _console.Print(3, 11, $"Gold:  {_playerInfo.Gold}");

            _console.Surface.DefaultBackground = Color.Black;
            _console.IsDirty = true;
        }

        public Console ReSize(int width, int height) {
            _width = width;
            _height = height;
            _console = new Console(width, height) {
                Position = _position,
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };
            UpdatePanel();
            return _console;
        }

        // Update method refreshes the panel display.
        public void Update() {
            UpdatePanel();
        }

        public void Render() {
            _console.IsDirty = true;
        }

        public void HandleInput() { }
    }
}
