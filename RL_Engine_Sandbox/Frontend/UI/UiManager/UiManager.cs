using RL_Engine_Sandbox.Backend;
using RL_Engine_Sandbox.Frontend.Interface;

namespace RL_Engine_Sandbox.Frontend.UI.UiManager;

public class UiManager : IUiManager
{
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
    public Dictionary<string, IUiElement> UiElements { get; set; } = new();
    public Dictionary<string, UiLayoutConfig> LayoutConfigs { get; set; } = new();

    public void AddUiElement(string name, IUiElement uiElement, UiLayoutConfig layoutConfig)
    {
        UiElements[name] = uiElement;
        LayoutConfigs[name] = layoutConfig;
    }

    public void RemoveUiElement(string name)
    {
        UiElements.Remove(name);
        LayoutConfigs.Remove(name);
    }

    public IUiElement GetUiElement(string name)
    {
        return UiElements[name];
    }
    public void HandleGameStateChange(GameState newState)
    {
        // if (newState == GameState.Inventory)
        //     GetUiElement("inventoryScreen")?.Show();
        // else
        //     GetUiElement("inventoryScreen")?.Hide();
    }

    public void SetScreenSize(int width, int height)
    {
        ScreenWidth = width;
        ScreenHeight = height;
        ApplyLayout();
    }

    public void ApplyLayout()
    {
        foreach (var pair in UiElements)
        {
            var name = pair.Key;
            var uiElement = pair.Value;

            if (!LayoutConfigs.TryGetValue(name, out var config))
                continue;

            var absoluteX = (int)(ScreenWidth * config.RelativeX);
            var absoluteY = (int)(ScreenHeight * config.RelativeY);
            var absoluteWidth = (int)(ScreenWidth * config.RelativeWidth);
            var absoluteHeight = (int)(ScreenHeight * config.RelativeHeight);

            uiElement.ReSize(absoluteWidth, absoluteHeight);
            uiElement.Initialize();

            // Set the underlying console's position.
            uiElement.GetConsole().Position = new Point(absoluteX, absoluteY);
        }
    }

    public void Update()
    {
        foreach (var uiElement in UiElements.Values) uiElement.Update();
    }

    public void Render()
    {
        foreach (var uiElement in UiElements.Values) uiElement.Render();
    }
}