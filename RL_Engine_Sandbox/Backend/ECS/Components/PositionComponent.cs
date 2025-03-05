using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class PositionComponent(int x, int y) : IComponents
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public Point Position { get; set; }
}