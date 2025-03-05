using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities.Actors;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Frontend.Interface;
using Color = SadRogue.Primitives.Color;
using Point = SadRogue.Primitives.Point;
using Rectangle = SadRogue.Primitives.Rectangle;

namespace RL_Engine_Sandbox.Frontend.UI.Screens.SubScreens;

public class CharacterSheetPanel : IUiElement
{
    private readonly IEventManager _eventManager;
    private readonly IComponentManager _componentManager;
    private readonly IInventoryManager _inventoryManager;
    private readonly Point _position;
    private readonly long _playerId;
    private Console _console;
    private int _height;
    private int _width;

    // Constructor now accepts a PlayerInfo object
    public CharacterSheetPanel(int width, int height, Point position, IEventManager eventManager, IComponentManager componentManager, long playerId)
    {
        _width = width;
        _height = height;
        _position = position;
        _console = new Console(width, height)
        {
            Position = position,
            IsEnabled = false,
            IsFocused = false,
            IsVisible = true,
            UseMouse = false,
            UseKeyboard = false
        };
        _eventManager = eventManager;
        _playerId = playerId;
        _componentManager = componentManager;
        // Subscribe to updates if the player's stats change
    }



    public Console GetConsole()
    {
        return _console;
    }

    public Console GetContentConsole()
    {
        throw new NotImplementedException();
    }

    // Initial panel setup: draws the box and the initial text.
    public void Initialize()
    {
        _console.DrawBox(
            new Rectangle(0, 0, _console.Width, _console.Height),
            ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick,
                new ColoredGlyph(Color.Bisque, Color.Black))
        );
        UpdatePanel();
    }

    public Console ReSize(int width, int height)
    {
        _width = width;
        _height = height;
        _console = new Console(width, height)
        {
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
    public void Update()
    {
        UpdatePanel();
    }

    public void Render()
    {
        _console.IsDirty = true;
    }
    public void HandleInput() { }
    private void UpdatePanel()
    {
        var player = _playerId;
        var playerInventory = _componentManager.GetComponent<InventoryComponent>(player);
        _console.Print(5, 1, "Character Sheet");
        // _console.Print(3, 3, $"Name: {_playerInfo.Name} ");
        // _console.Print(3, 4, $"Class: {_playerInfo.Class}");
        // _console.Print(3, 5, $"Level: {_playerInfo.Level} ");
        // _console.Print(3, 6, $"Xp: {_playerInfo.Experience}");
        // _console.Print(3, 7, $"Health: {_playerInfo.Health}/{_playerInfo.MaxHealth}");
        // _console.Print(3, 8, $"Attack:  {_playerInfo.Attack}");
        // _console.Print(3, 9, $"Defense:  {_playerInfo.Defense}");
        // _console.Print(3, 10, $"Speed:  {_playerInfo.Speed}");
        _console.Print(3, 11, $"Gold:  0");
        _console.Print(3, 12, $"-------------------");
        _console.Print(3, 13, "Inventory:");
        var invHeight = 14;
        var items = playerInventory.Items;
        foreach (var i in items)
        {
            if (i == null)
            {
                _console.Print(3, invHeight++, "(Unknown Item)"); // Handle missing items
                continue;
            }

            var nameComponent = _componentManager.GetComponent<NameComponent>(i.Id);
            if (nameComponent != null)
            {
                _console.Print(3, invHeight++, $"{nameComponent.Name}"); // Print item name
            }
            else
            {
                _console.Print(3, invHeight++, "(Unnamed Item)"); // Handle missing NameComponent
            }
        }


        _console.IsDirty = true;
        _console.Surface.DefaultBackground = Color.Black;
        _console.IsDirty = true;
    }
}