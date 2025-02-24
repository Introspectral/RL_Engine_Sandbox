using RL_Engine_Sandbox.Backend.ECS.Interface;
using Color = SadRogue.Primitives.Color;

namespace RL_Engine_Sandbox.Backend.ECS.Map
{
    public class CaveMap : IMap 
    {
        public int Width { get; }
        public int Height { get; }
        public Tile[,] Tiles { get; }
        public bool IsFloorTile(int x, int y, Map.TileType tileType)
        {
            if (Tiles[x, y].TileType == tileType)
            {
                return true;
            }
            return false;
        }

        // Configuration constants
        private const int InitialWallProbability = 45; // Percentage
        private const int BirthLimit = 4;
        private const int DeathLimit = 5;
        private const int NumberOfIterations = 11;
        private const int MinRegionSize = 50;

        private readonly Random _random;
        private bool[,] _cellMap;

        public CaveMap(int width, int height, int? seed = null) {
            Width = width;
            Height = height;
            Tiles = new Tile[width, height];
            _cellMap = new bool[width, height];
            _random = seed.HasValue ? new Random(seed.Value) : new Random();

            GenerateCave();
        }

        private void GenerateCave() {
            InitializeRandomCells();
            RunCellularAutomata();
            RemoveSmallRegions();
            ConnectRegions();
            TransferToTileMap();
        }

        private void InitializeRandomCells() {
            for (int x = 0; x < Width; x++){
                for (int y = 0; y < Height; y++) {
                    // True represents a wall
                    _cellMap[x, y] = _random.Next(100) < InitialWallProbability;
                    // Always make the borders walls
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1) {
                        _cellMap[x, y] = true;
                    }
                }
            }
        }

        private void RunCellularAutomata() {
            for (int i = 0; i < NumberOfIterations; i++) {
                bool[,] newMap = new bool[Width, Height];
                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        int neighbors = CountWallNeighbors(x, y);
                        // Apply cellular automata rules
                        if (_cellMap[x, y]) {
                            // Cell is currently a wall
                            newMap[x, y] = neighbors >= DeathLimit;
                        }
                        else {
                            // Cell is currently a floor
                            newMap[x, y] = neighbors >= BirthLimit;
                        }

                        // Keep the borders as walls
                        if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1) {
                            newMap[x, y] = true;
                        }
                    }
                }
                _cellMap = newMap;
            }
        }

        private int CountWallNeighbors(int x, int y) {
            int count = 0;
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if (i == 0 && j == 0) continue;
                    int nx = x + i;
                    int ny = y + j;
                    // Count out-of-bounds cells as walls
                    if (!IsInBounds(nx, ny) || _cellMap[nx, ny]) {
                        count++;
                    }
                }
            }
            return count;
        }

        private void RemoveSmallRegions() {
            List<HashSet<Point>> regions = GetRegions(!true); // Get floor regions
            foreach (var region in regions) {
                if (region.Count < MinRegionSize) {
                    foreach (var point in region) {
                        _cellMap[point.X, point.Y] = true; // Convert to wall
                    }
                }
            }
        }

        private List<HashSet<Point>> GetRegions(bool wallType) {
            List<HashSet<Point>> regions = new List<HashSet<Point>>();
            bool[,] visited = new bool[Width, Height];
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    if (!visited[x, y] && _cellMap[x, y] == wallType) {
                        HashSet<Point> newRegion = GetRegionTiles(x, y, wallType);
                        regions.Add(newRegion);
                        // Mark all tiles in this region as visited
                        foreach (var point in newRegion) {
                            visited[point.X, point.Y] = true;
                        }
                    }
                }
            }

            return regions;
        }

        private HashSet<Point> GetRegionTiles(int startX, int startY, bool wallType) {
            HashSet<Point> tiles = new HashSet<Point>();
            Queue<Point> queue = new Queue<Point>();
            Point start = new Point(startX, startY);
            queue.Enqueue(start);
            tiles.Add(start);
            while (queue.Count > 0) {
                Point current = queue.Dequeue();
                foreach (var neighbor in GetAdjacentPoints(current.X, current.Y)) {
                    if (IsInBounds(neighbor.X, neighbor.Y) &&
                        _cellMap[neighbor.X, neighbor.Y] == wallType &&
                        !tiles.Contains(neighbor)) {
                        tiles.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return tiles;
        }

        private void ConnectRegions() {
            List<HashSet<Point>> regions = GetRegions(!true); // Get floor regions
            if (regions.Count <= 1) return;
            List<Point> regionConnectors = new List<Point>();
            // Find all possible connections between regions
            for (int i = 0; i < regions.Count; i++) {
                for (int j = i + 1; j < regions.Count; j++) {
                    Point? connector = FindClosestTiles(regions[i], regions[j]);
                    if (connector.HasValue) {
                        regionConnectors.Add(connector.Value);
                    }
                }
            }
            // Create passages between regions
            foreach (var connector in regionConnectors) {
                CreatePassage(connector);
            }
        }

        private Point? FindClosestTiles(HashSet<Point> regionA, HashSet<Point> regionB) {
            Point? bestTile = null;
            double bestDistance = double.MaxValue;
            foreach (var tileA in regionA) {
                foreach (var tileB in regionB) {
                    double distance = Distance(tileA, tileB);
                    if (distance < bestDistance) {
                        bestDistance = distance;
                        bestTile = new Point(
                            (tileA.X + tileB.X) / 2,
                            (tileA.Y + tileB.Y) / 2
                        );
                    }
                }
            }
            return bestTile;
        }

        private void CreatePassage(Point connector) {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    int nx = connector.X + x;
                    int ny = connector.Y + y;
                    if (IsInBounds(nx, ny)) {
                        _cellMap[nx, ny] = false; // Create floor
                    }
                }
            }
        }

        private void TransferToTileMap() {
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    Tiles[x, y] = CreateTile(new Point(x, y), _cellMap[x, y] ? TileType.Wall : TileType.Floor);
                }
            }
        }
        

        private Tile CreateTile(Point position, TileType type) => type switch {
            TileType.Floor => new Tile(position, true, new ColoredGlyph(Color.Bisque, Color.Black, '.'),Map.TileType.Floor),
            TileType.Wall => new Tile(position, false, '#', Color.Gray, Color.Black, Map.TileType.Wall),
            _ => throw new ArgumentException($"Unknown tile type: {type}")
        };

        private bool IsInBounds(int x, int y) =>
            x >= 0 && x < Width && y >= 0 && y < Height;

        private IEnumerable<Point> GetAdjacentPoints(int x, int y){
            for (int dx = -1; dx <= 1; dx++) {
                for (int dy = -1; dy <= 1; dy++) {
                    if (dx == 0 && dy == 0) continue;
                    yield return new Point(x + dx, y + dy);
                }
            }
        }

        private double Distance(Point a, Point b) {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}