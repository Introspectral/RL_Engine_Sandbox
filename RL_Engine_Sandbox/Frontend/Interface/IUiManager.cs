using RL_Engine_Sandbox.Backend;
using RL_Engine_Sandbox.Frontend.UI.UiManager;

namespace RL_Engine_Sandbox.Frontend.Interface;

public interface IUiManager
{
    Dictionary<string, IUiElement> UiElements { get; set; }
    Dictionary<string, UiLayoutConfig> LayoutConfigs { get; set; }
    void ApplyLayout();
    void HandleGameStateChange(GameState newState);
    void SetScreenSize(int width, int height);
    void AddUiElement(string name, IUiElement uiElement, UiLayoutConfig layoutConfig);
    void RemoveUiElement(string name);
    IUiElement GetUiElement(string name);
    void Update();
    void Render();
}