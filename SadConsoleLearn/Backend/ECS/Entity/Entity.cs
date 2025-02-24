using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Entity;

public class Entity
{
    public long Id { get; set; }
    public List<IComponents> Components { get; set; }
    
    public Entity()
    {
        Id = NextId();
        Components = new List<IComponents>();
    }
    
    private static long NextId()
    {
        return DateTime.Now.Ticks;
    }
}