using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Entities;

public class Entity
{
    public long Id { get; set; } = NextId();
    public List<IComponents> Components { get; set; } = new();

    private static long NextId()
    {
        return DateTime.Now.Ticks;
    }
}