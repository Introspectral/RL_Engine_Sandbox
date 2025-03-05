namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface ISpawnManager
{
    (int X, int Y) FindValidSpawnPoint();
}