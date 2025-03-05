using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Map;
using System;
using System.Collections.Generic;
using RL_Engine_Sandbox.Backend.ECS.Entities;

namespace RL_Engine_Sandbox.Backend.ECS.Manager
{
    public class MapManager : IMapManager
    {
        // Store maps by level (1-indexed)
        public Dictionary<int, IMap> Maps { get; set; }
        
        // Current active level
        public int CurrentLevel { get; private set; }
        public IMap CurrentMap => Maps[CurrentLevel];

        // public IMap Map { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        
        // Constructor now takes a MapFactory to generate maps.
        // It generates a fixed number of levels (default 15).
        public MapManager(IMapFactory mapFactory, int numberOfLevels = 15)
        {
            
            Maps = new Dictionary<int, IMap>();
            for (int i = 1; i <= numberOfLevels; i++)
            {
                var map = mapFactory.CreateMap(120,30);
                Maps.Add(i, map);
            }
            CurrentLevel = 1; // Ensure we start at level 1, not 0!
            Width = CurrentMap.Width;
            Height = CurrentMap.Height;
        }

        // Switch the active map (level) during gameplay.
        public void SetCurrentLevel(int level)
        {
            if (!Maps.ContainsKey(level))
                throw new Exception($"Level {level} does not exist.");
            CurrentLevel = level;
            Width = CurrentMap.Width;
            Height = CurrentMap.Height;
        }

        public Tile GetTileAt(int x, int y)
        {
            if (!IsWithinBounds(x, y))
                throw new ArgumentOutOfRangeException($"Coordinates ({x},{y}) are out of bounds.");
            return CurrentMap.Tiles[x, y];
        }

        public bool IsTileWalkable(int x, int y)
        {
            if (!IsWithinBounds(x, y))
                return false;
            return GetTileAt(x, y).IsWalkable;
        }

        public (int X, int Y) FindValidSpawnPoint()
        {
            var rooms = CurrentMap.Rooms;
            var random = new Random();
            if (rooms == null || rooms.Count == 0)
                throw new Exception("No rooms available!");

            var randomRoom = rooms[random.Next(rooms.Count)];
            var spawnX = randomRoom.X + random.Next(randomRoom.Width);
            var spawnY = randomRoom.Y + random.Next(randomRoom.Height);

            if (IsTileWalkable(spawnX, spawnY))
                return (spawnX, spawnY);

            return (randomRoom.X + randomRoom.Width / 2, randomRoom.Y + randomRoom.Height / 2);
        }

        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < CurrentMap.Width && y >= 0 && y < CurrentMap.Height;
        }

        public bool IsTileExplored(int x, int y)
        {
            if (!IsWithinBounds(x, y))
                return false;
            return GetTileAt(x, y).IsExplored;
        }

        public bool IsTileInFov(int x, int y)
        {
            if (!IsWithinBounds(x, y))
                return false;
            return GetTileAt(x, y).IsInFov;
        }
    }
}
