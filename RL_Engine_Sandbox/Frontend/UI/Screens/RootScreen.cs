namespace RL_Engine_Sandbox.Frontend.UI.Screens;

public class RootScreen : ScreenObject
{
    public RootScreen(GameLoop gameLoop)
    {
        Children.Add(gameLoop);

        Game.Instance.Screen = gameLoop;
        Game.Instance.DestroyDefaultStartingConsole();
    }
}