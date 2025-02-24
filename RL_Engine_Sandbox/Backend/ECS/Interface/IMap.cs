using RL_Engine_Sandbox.Backend.ECS.Map;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IMap
{
     public int Width { get; }
     public int Height { get; }
     public Tile[,] Tiles { get; }
     public List<Entity.Entity> Entities { get; }
 

     
}