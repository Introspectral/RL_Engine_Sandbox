namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IFovSystem
{
    public HashSet<Point> ComputeFov(long entityid);
    bool HasLineOfSight(Point start, Point end);
}