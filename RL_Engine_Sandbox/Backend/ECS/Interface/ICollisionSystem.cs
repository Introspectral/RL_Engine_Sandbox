using RL_Engine_Sandbox.Backend.ECS.Entities;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface ICollisionSystem
{
   
    public void CollisionCheck(long entityId, int x, int y);
    public bool IsTileOccupied(int x, int y, long entityId);
    public Entity? GetEntityAtPosition(int x, int y, long ignoreEntityId);
    bool IsTileWalkable(int x, int y);
}