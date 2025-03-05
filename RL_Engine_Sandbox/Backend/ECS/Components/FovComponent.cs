using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class FovComponent(int radius) : IComponents
{
    public int Radius { get; set; } = radius;
    public List<Point> VisibleTiles { get; set; } = new();
}