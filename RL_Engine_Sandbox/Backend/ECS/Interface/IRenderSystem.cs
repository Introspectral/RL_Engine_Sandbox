using RL_Engine_Sandbox.Backend.ECS.Entities;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IRenderSystem
{
    public long PlayerId { get; set; }
    public Console Console { get; set; }
    public void RenderAll();
    Color Darken(Color color);
    public void Initialize();
    public List<Entity> EntitiesToRender { get; set; }
}