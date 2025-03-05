using RL_Engine_Sandbox.Backend.ECS.Map;

using RL_Engine_Sandbox.Backend.ECS.Map;

namespace RL_Engine_Sandbox.Backend.ECS.Interface
{
    public interface IMapManager
    {
        // IMap Map { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        
        (int X, int Y) FindValidSpawnPoint();
        Tile GetTileAt(int x, int y);
        bool IsTileWalkable(int x, int y);

        // New members for multi-level support:
        int CurrentLevel { get; }
        public IMap CurrentMap => Maps[CurrentLevel];
        void SetCurrentLevel(int level);
        Dictionary<int, IMap> Maps { get; set; }

        // Optional additional helpers:
        bool IsTileExplored(int x, int y);
        bool IsTileInFov(int x, int y);
    }
}
