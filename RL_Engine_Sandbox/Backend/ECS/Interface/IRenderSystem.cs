using RL_Engine_Sandbox.Backend.ECS.Entities;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IRenderSystem
{
    public Console Console { get; set; }
    public void RenderAll();
    Color Darken(Color color);
    public void Initialize();

}