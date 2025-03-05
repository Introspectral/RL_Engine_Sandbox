namespace RL_Engine_Sandbox.Frontend.Interface;

public interface IUiElement
{
    public Console GetConsole();
    public Console GetContentConsole();
    public void Initialize();
    public Console ReSize(int width, int height);
    public void Update();
    public void Render();
    public void HandleInput();
}