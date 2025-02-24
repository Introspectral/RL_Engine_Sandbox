using RL_Engine_Sandbox.Frontend.Interface;
using Color = SadRogue.Primitives.Color;
using Point = SadRogue.Primitives.Point;
using Rectangle = SadRogue.Primitives.Rectangle;

namespace RL_Engine_Sandbox.Frontend.Screens.SubScreens
{
    public class GameAreaPanel : IUiElement 
    {
        // Container console holds both the frame and content consoles.
        private Console _containerConsole;
        
        // The frame console holds the static border.
        private Console _frameConsole;
        
        // The content console is where dynamic gameplay rendering happens.
        private Console _contentConsole;
        
        private int _width;
        private int _height;
        private readonly Point _position;

        public GameAreaPanel(int width, int height, Point position) 
        {
            _width = width;
            _height = height;
            _position = position;
            
            // Create the container console.
            _containerConsole = new Console(width, height)
            {
                Position = position,
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };

            // Create the frame console that will display the border.
            _frameConsole = new Console(width, height)
            {
                Position = new Point(0, 0),
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };

            // Create the content console sized to fit inside the border.
            _contentConsole = new Console(width - 2, height - 2)
            {
                Position = new Point(1, 1),
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };

            // Add both consoles as children of the container.
            _containerConsole.Children.Add(_frameConsole);
            _containerConsole.Children.Add(_contentConsole);
        }
        // Returns the container console (which includes both frame and content).
        public Console GetConsole() => _containerConsole;
        // Returns only the content console for dynamic rendering.
        public Console GetContentConsole() => _contentConsole;
        public void Initialize() 
        {
            // Draw the static border on the frame console.
            _frameConsole.DrawBox(
                new Rectangle(0, 0, _frameConsole.Width, _frameConsole.Height),
                ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, 
                    new ColoredGlyph(Color.Bisque, Color.Transparent))
            );
            _frameConsole.Surface.DefaultBackground = Color.Transparent;
            _frameConsole.IsDirty = true;

            // Initialize the content console with its default background.
            _contentConsole.Surface.DefaultBackground = Color.Transparent;
            _contentConsole.IsDirty = true;
        }
        
        public Console ReSize(int width, int height)
        {
            _width = width;
            _height = height;
            // Recreate the container console.
            _containerConsole = new Console(width, height)
            {
                Position = _position,
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };

            // Recreate the frame console.
            _frameConsole = new Console(width, height)
            {
                Position = new Point(0, 0),
                IsEnabled = false,
                IsFocused = false,
                IsVisible = true,
                UseMouse = false,
                UseKeyboard = false
            };

            // Recreate the content console.
            _contentConsole = new Console(width - 2, height - 2)
            {
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

        public void Update() 
        {
            _containerConsole.IsDirty = true;
        }

        public void Render() 
        {
            // Do not clear the frame console here.
            // The RenderingSystem should only clear and update the content console.
            _containerConsole.IsDirty = true;
        }

        public void HandleInput() { }
    }
}
