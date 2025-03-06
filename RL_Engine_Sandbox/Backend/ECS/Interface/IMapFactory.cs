namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IMapFactory
{
     int Width { get; set; }
     int Height { get; set; }

     IMap CreateMap()
     {
          return new Map.Map(Width, Height);
     }
}