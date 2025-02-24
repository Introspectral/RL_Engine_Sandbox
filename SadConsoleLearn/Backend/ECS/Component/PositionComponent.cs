using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class PositionComponent : IComponents
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point Position { get; set; }
    public PositionComponent(int x, int y){
        X = x;
        Y = y;
        
    }
}