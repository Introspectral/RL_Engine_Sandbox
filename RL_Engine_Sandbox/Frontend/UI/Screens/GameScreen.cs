using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Frontend.Interface;

namespace RL_Engine_Sandbox.Frontend.UI.Screens;

internal class GameScreen : ScreenObject
{
    private IEventManager _eventManager;
    private IUiManager _uiManager;

    public GameScreen(IEventManager eventManager, int width, int height, IUiManager uiManager)
    {
        GameConsole = new Console(width, height);
        IsFocused = false;
        IsVisible = true;
        UseKeyboard = false;
        _eventManager = eventManager;
        _uiManager = uiManager;

        Children.Add(GameConsole);
        AttachUiElements();
    }

    public Console GameConsole { get; private set; }

    private void AttachUiElements()
    {
        GameConsole.Children.Clear();
        foreach (var uiElement in _uiManager.UiElements.Values) GameConsole.Children.Add(uiElement.GetConsole());
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);
        GameConsole.IsDirty = true;
    }
}