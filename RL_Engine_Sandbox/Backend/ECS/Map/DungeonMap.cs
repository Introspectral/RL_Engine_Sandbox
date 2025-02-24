using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Entity;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Manager;

namespace RL_Engine_Sandbox.Backend.ECS.Map;

    public class DungeonMap : IMap
    {
        public int Width { get; }
        public int Height { get; }
        public Tile[,] Tiles { get; }
        public List<Entity.Entity> Entities { get; }
        private List<Rectangle> _rooms = new List<Rectangle>();
        private List<bool> _roomConnected = new List<bool>();
        private Random _random = new Random();
        
        public DungeonMap(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[width, height];
            Entities = new List<Entity.Entity>();
            InitializeMap();
        }
        
        private void InitializeMap()
        {
            int targetRoomCount = _random.Next(3, 8);
            int attempts = 0;
            int maxAttempts = 50;
            int minRoomSize = 4;
            int maxRoomSize = 10;

            FillWithEmptyTiles();
            CalculateRoomCount();
            ConnectRoomsWithCorridors();
            PlaceWallsAroundFloors();
            PopulateMapWithEntities();

            void FillWithEmptyTiles()
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Tiles[x, y] = new Tile(
                            new Point(x, y),
                            isWalkable: true,
                            new ColoredGlyph(Color.Black, Color.Black, ' '),
                            TileType.Empty
                        );
                    }
                }
            }
            void CalculateRoomCount()
            {
                while (_rooms.Count < targetRoomCount && attempts < maxAttempts)
                {
                    attempts++;
                    int roomWidth = _random.Next(minRoomSize, maxRoomSize + 1);
                    int roomHeight = _random.Next(minRoomSize, maxRoomSize + 1);
                    // Ensure room is at least 3 tiles away from map edges.
                    int roomX = _random.Next(3, Width - roomWidth - 3);
                    int roomY = _random.Next(3, Height - roomHeight - 3);
                    Rectangle newRoom = new Rectangle(roomX, roomY, roomWidth, roomHeight);
                
                    // Check for overlap with a buffer of 2 tiles.
                    bool overlaps = false;
                    foreach (var other in _rooms)
                    {
                        var inflatedOther = new Rectangle(other.X - 2, other.Y - 2, other.Width + 4, other.Height + 4);
                        if (newRoom.Intersects(inflatedOther))
                        {
                            overlaps = true;
                            break;
                        }
                    }
                    if (!overlaps)
                    {
                        CarveRoom(newRoom);
                        _rooms.Add(newRoom);
                        _roomConnected.Add(false);
                    }
                }
            }
            void ConnectRoomsWithCorridors()
            {
                for (int i = 1; i < _rooms.Count; i++)
                {
                    if (CreateCorridor(_rooms[i - 1], _rooms[i]))
                    {
                        _roomConnected[i - 1] = true;
                        _roomConnected[i] = true;
                    }
                    else
                    {
                        // Fallback: force a corridor using the horizontal-then-vertical candidate.
                        List<Point> fallbackPath = GetHorizontalThenVerticalPath(_rooms[i - 1].Center, _rooms[i].Center);
                        CarvePath(fallbackPath);
                        _roomConnected[i - 1] = true;
                        _roomConnected[i] = true;
                    }
                }
                // Second pass: Ensure that every room is connected.

                for (int i = 0; i < _rooms.Count; i++)
                {
                    if (_roomConnected[i])
                        continue;
                
                    // Find the nearest room (by center-to-center distance).
                    Rectangle current = _rooms[i];
                    double bestDist = double.MaxValue;
                    int bestIndex = -1;
                    for (int j = 0; j < _rooms.Count; j++)
                    {
                        if (i == j)
                            continue;
                        double dist = Distance(current.Center, _rooms[j].Center);
                        if (dist < bestDist)
                        {
                            bestDist = dist;
                            bestIndex = j;
                        }
                    }
                    if (bestIndex >= 0)
                    {
                        if (CreateCorridor(current, _rooms[bestIndex]))
                        {
                            _roomConnected[i] = true;
                            _roomConnected[bestIndex] = true;
                        }
                        else
                        {
                            // Fallback corridor if the candidate paths intersect another room.
                            List<Point> fallbackPath = GetHorizontalThenVerticalPath(current.Center, _rooms[bestIndex].Center);
                            CarvePath(fallbackPath);
                            _roomConnected[i] = true;
                            _roomConnected[bestIndex] = true;
                        }
                    }
                }
            }
            void PlaceWallsAroundFloors()
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (Tiles[x, y].Glyph.Glyph == ' ' && HasAdjacentFloor(x, y))
                        {
                            Tiles[x, y] = new Tile(
                                new Point(x, y),
                                isWalkable: false,
                                new ColoredGlyph(Color.Gray, Color.Black, '#'),
                                TileType.Wall
                            );
                        }
                    }
                }
            }
            void PopulateMapWithEntities()
            {
                foreach (var entity in Entities)
                {
                    var position = new ComponentManager().GetComponent<PositionComponent>(entity.Id);
                    if (position == null)
                        continue;
                    int x = position.X;
                    int y = position.Y;
                    if (x < 0 || x >= Width || y < 0 || y >= Height) 
                        continue;
                }
            }
        }
        
        // Carve out a room: set its tiles to floor.
        private void CarveRoom(Rectangle room)
        {
            for (int x = room.X; x < room.X + room.Width; x++)
            {
                for (int y = room.Y; y < room.Y + room.Height; y++)
                {
                    Tiles[x, y] = new Tile(
                        new Point(x, y),
                        isWalkable: true,
                        new ColoredGlyph(Color.Gray, Color.Black, '.'),
                        TileType.Floor
                    );
                }
            }
        }
        
        // Create a corridor connecting two rooms without intersecting a third room.
        // Returns true if a corridor was successfully created.
        private bool CreateCorridor(Rectangle roomA, Rectangle roomB)
        {
            Point start = roomA.Center;
            Point end = roomB.Center;
            
            // Generate two candidate L-shaped paths.
            List<Point> path1 = GetHorizontalThenVerticalPath(start, end);
            List<Point> path2 = GetVerticalThenHorizontalPath(start, end);
            
            if (!CorridorPathIntersectsOtherRooms(path1, roomA, roomB))
            {
                CarvePath(path1);
                return true;
            }
            else if (!CorridorPathIntersectsOtherRooms(path2, roomA, roomB))
            {
                CarvePath(path2);
                return true;
            }
            return false;
        }
        
        // Generate L-shaped path: horizontal then vertical.
        private List<Point> GetHorizontalThenVerticalPath(Point start, Point end)
        {
            List<Point> path = new List<Point>();
            int x = start.X;
            int y = start.Y;
            while (x != end.X)
            {
                path.Add(new Point(x, y));
                x += (end.X > x) ? 1 : -1;
            }
            while (y != end.Y)
            {
                path.Add(new Point(x, y));
                y += (end.Y > y) ? 1 : -1;
            }
            return path;
        }
        
        // Generate L-shaped path: vertical then horizontal.
        private List<Point> GetVerticalThenHorizontalPath(Point start, Point end)
        {
            List<Point> path = new List<Point>();
            int x = start.X;
            int y = start.Y;
            while (y != end.Y)
            {
                path.Add(new Point(x, y));
                y += (end.Y > y) ? 1 : -1;
            }
            while (x != end.X)
            {
                path.Add(new Point(x, y));
                x += (end.X > x) ? 1 : -1;
            }
            return path;
        }
        
        // Check if any point in the path (excluding endpoints) lies within any room other than roomA or roomB.
        private bool CorridorPathIntersectsOtherRooms(List<Point> path, Rectangle roomA, Rectangle roomB)
        {
            // Skip the first and last points (they are inside the connecting rooms).
            for (int i = 1; i < path.Count - 1; i++)
            {
                Point p = path[i];
                foreach (var room in _rooms)
                {
                    if (room.Equals(roomA) || room.Equals(roomB))
                        continue;
                    if (room.Contains(p))
                        return true;
                }
            }
            return false;
        }
        
        // Carve the corridor: set each tile along the path to floor.
        private void CarvePath(List<Point> path)
        {
            foreach (Point p in path)
            {
                if (p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height)
                {
                    Tiles[p.X, p.Y] = new Tile(
                        new Point(p.X, p.Y),
                        isWalkable: true,
                        new ColoredGlyph(Color.Gray, Color.Black, '.'),
                        TileType.Floor
                    );
                }
            }
        }
        
        // Check 8 neighboring cells to see if any is a floor tile.
        private bool HasAdjacentFloor(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx < 0 || nx >= Width || ny < 0 || ny >= Height)
                        continue;
                    if (Tiles[nx, ny].Glyph.Glyph == '.')
                        return true;
                }
            }
            return false;
        }
        
        // Compute Euclidean distance between two points.
        private double Distance(Point a, Point b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
