namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface ICollisionSystem
{
  bool CollisionCheck(long entityId1, int x, int y);


}