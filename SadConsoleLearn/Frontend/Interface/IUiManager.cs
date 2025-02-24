using RL_Engine_Sandbox.Frontend.Manager.UiManager;

namespace RL_Engine_Sandbox.Frontend.Interface;

public interface IUiManager {
    Dictionary<string, IUiElement> UiElements { get; set; }
    Dictionary<string, UiLayoutConfig> LayoutConfigs { get; set; }
    void ApplyLayout();
    void SetScreenSize(int width, int height);
    void AddUiElement(string name, IUiElement uiElement, UiLayoutConfig layoutConfig);
    void RemoveUiElement(string name);
    IUiElement GetUiElement(string name);
    void Update();
    void Render();
}