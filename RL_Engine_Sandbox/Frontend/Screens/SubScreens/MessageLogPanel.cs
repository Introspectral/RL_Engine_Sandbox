using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Frontend.Interface;
using Color = SadRogue.Primitives.Color;
using Point = SadRogue.Primitives.Point;
using Rectangle = SadRogue.Primitives.Rectangle;

namespace RL_Engine_Sandbox.Frontend.Screens.SubScreens
{
    public class MessageLogPanel : IUiElement
    {
        private Console _containerConsole;
        private Console _frameConsole;
        private Console _contentConsole;
        private int _width;
        private int _height;
        private readonly Point _position;
        public IEventManager EventManager { get; set; }
        public MessageLogPanel(int width, int height, Point position, IEventManager eventManager){
            _width = width;
            _height = height;
            _position = position;
            _containerConsole = new Console(width, height) {
                Position = position,
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };
            _frameConsole = new Console(width, height) {
                Position = position,
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };

            _contentConsole = new Console(width-2, height-2) {
                Position = position,
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };
            EventManager = eventManager;
            
            _containerConsole.Children.Add(_frameConsole);
            _containerConsole.Children.Add(_contentConsole);
            
            eventManager.Subscribe<MessageEvent>(OnMessageEvent);
        }

        private void OnMessageEvent(MessageEvent obj) {
            _contentConsole.Clear();
            _contentConsole.Print(1, 1, obj.Message, Color.White);
        }
        
        public Console GetConsole() => _containerConsole;
        public Console GetContentConsole() {
            return _contentConsole;
        }

        public void Initialize() {
            _frameConsole.DrawBox(
                new Rectangle(0, 0, _frameConsole.Width, _frameConsole.Height), 
                ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, 
                    new ColoredGlyph(Color.Moccasin, Color.Transparent))
            );
           _frameConsole.Surface.DefaultBackground = Color.Black;
           _frameConsole.Surface.Print(5, 0, "Message Log", Color.White);
           _frameConsole.IsDirty = true;
           _contentConsole.Surface.DefaultBackground = Color.Black;
           _contentConsole.IsDirty = true;
        }

        public Console ReSize(int width, int height) {
            _width = width;
            _height = height;

            // Recreate the container console.
            _containerConsole = new Console(width, height) {
                Position = _position,
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };

            // Recreate the frame console.
            _frameConsole = new Console(width, height) {
                Position = new Point(0, 0),
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };
            

            // Recreate the content console.
            _contentConsole = new Console(width - 2, height - 2) {
                Position = new Point(1, 1),
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };

            // Re-add the consoles to the container.
            _containerConsole.Children.Clear();
            _containerConsole.Children.Add(_frameConsole);
            _containerConsole.Children.Add(_contentConsole);

            Initialize();

            return _containerConsole;
        }

        public void Update() {
            _containerConsole.IsDirty = true;
        }

        public void Render() {
            _containerConsole.IsDirty = true;
        }

        public void HandleInput() { }
    }
}
