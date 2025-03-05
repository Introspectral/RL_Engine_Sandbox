using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Manager;
using Point = SadRogue.Primitives.Point;

namespace RL_Engine_Sandbox.Backend.ECS.Map;

public class Map : IMap
{
    private readonly Random _random = new();

    private readonly List<bool> _roomConnected = new();

    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new Tile[width, height];
        Entities = new List<Entity>();
        Rooms = new List<Rectangle>();
        InitializeMap();
    }

    public int Width { get; }
    public int Height { get; }
    public Tile[,] Tiles { get; }
    public List<Entity> Entities { get; }
    public List<Rectangle> Rooms { get; }

    private void InitializeMap()
    {
        var targetRoomCount = _random.Next(6, 10);
        var attempts = 0;
        var maxAttempts = 50;
        var minRoomSize = 4;
        var maxRoomSize = 10;

        FillWithEmptyTiles();
        CalculateRoomCount();
        ConnectRoomsWithCorridors();
        PlaceWallsAroundFloors();
        PopulateMapWithEntities();

        void FillWithEmptyTiles()
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                Tiles[x, y] = TileFactory.Create
                (
                    new Point(x, y),
                    TileType.Empty
                );
        }

        void CalculateRoomCount()
        {
            while (Rooms.Count < targetRoomCount && attempts < maxAttempts)
            {
                attempts++;
                var roomWidth = _random.Next(minRoomSize, maxRoomSize + 1);
                var roomHeight = _random.Next(minRoomSize, maxRoomSize + 1);
                // Ensure room is at least 3 tiles away from map edges.
                var roomX = _random.Next(3, Width - roomWidth - 3);
                var roomY = _random.Next(3, Height - roomHeight - 3);
                var newRoom = new Rectangle(roomX, roomY, roomWidth, roomHeight);

                // Check for overlap with a buffer of 2 tiles.
                var overlaps = false;
                foreach (var other in Rooms)
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
                    Rooms.Add(newRoom);
                    _roomConnected.Add(false);
                }
            }
        }

        void ConnectRoomsWithCorridors()
        {
            for (var i = 1; i < Rooms.Count; i++)
                if (CreateCorridor(Rooms[i - 1], Rooms[i]))
                {
                    _roomConnected[i - 1] = true;
                    _roomConnected[i] = true;
                }
                else
                {
                    // Fallback: force a corridor using the horizontal-then-vertical candidate.
                    var fallbackPath = GetHorizontalThenVerticalPath(Rooms[i - 1].Center, Rooms[i].Center);
                    CarvePath(fallbackPath);
                    _roomConnected[i - 1] = true;
                    _roomConnected[i] = true;
                }
            // Second pass: Ensure that every room is connected.

            for (var i = 0; i < Rooms.Count; i++)
            {
                if (_roomConnected[i])
                    continue;

                // Find the nearest room (by center-to-center distance).
                var current = Rooms[i];
                var bestDist = double.MaxValue;
                var bestIndex = -1;
                for (var j = 0; j < Rooms.Count; j++)
                {
                    if (i == j)
                        continue;
                    var dist = Distance(current.Center, Rooms[j].Center);
                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        bestIndex = j;
                    }
                }

                if (bestIndex >= 0)
                {
                    if (CreateCorridor(current, Rooms[bestIndex]))
                    {
                        _roomConnected[i] = true;
                        _roomConnected[bestIndex] = true;
                    }
                    else
                    {
                        // Fallback corridor if the candidate paths intersect another room.
                        var fallbackPath = GetHorizontalThenVerticalPath(current.Center, Rooms[bestIndex].Center);
                        CarvePath(fallbackPath);
                        _roomConnected[i] = true;
                        _roomConnected[bestIndex] = true;
                    }
                }
            }
        }

        void PlaceWallsAroundFloors()
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (Tiles[x, y].Glyph.Glyph == ' ' && HasAdjacentFloor(x, y))
                    Tiles[x, y] = TileFactory.Create
                    (
                        new Point(x, y),
                        TileType.Wall
                    );
        }

        void PopulateMapWithEntities()
        {
            foreach (var entity in Entities)
            {
                var position = new ComponentManager().GetComponent<PositionComponent>(entity.Id);
                if (position == null)
                    continue;
                var x = position.X;
                var y = position.Y;
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    continue;
            }
        }
    }

    // Carve out a room: set its tiles to floor.
    private void CarveRoom(Rectangle room)
    {
        for (var x = room.X; x < room.X + room.Width; x++)
        for (var y = room.Y; y < room.Y + room.Height; y++)
            Tiles[x, y] = TileFactory.Create
            (
                new Point(x, y),
                TileType.Floor
            );
    }

    // Create a corridor connecting two rooms without intersecting a third room.
    // Returns true if a corridor was successfully created.
    private bool CreateCorridor(Rectangle roomA, Rectangle roomB)
    {
        var start = roomA.Center;
        var end = roomB.Center;

        // Generate two candidate L-shaped paths.
        var path1 = GetHorizontalThenVerticalPath(start, end);
        var path2 = GetVerticalThenHorizontalPath(start, end);

        if (!CorridorPathIntersectsOtherRooms(path1, roomA, roomB))
        {
            CarvePath(path1);
            return true;
        }

        if (!CorridorPathIntersectsOtherRooms(path2, roomA, roomB))
        {
            CarvePath(path2);
            return true;
        }

        return false;
    }

    // Generate L-shaped path: horizontal then vertical.
    private List<Point> GetHorizontalThenVerticalPath(Point start, Point end)
    {
        var path = new List<Point>();
        var x = start.X;
        var y = start.Y;
        while (x != end.X)
        {
            path.Add(new Point(x, y));
            x += end.X > x ? 1 : -1;
        }

        while (y != end.Y)
        {
            path.Add(new Point(x, y));
            y += end.Y > y ? 1 : -1;
        }

        return path;
    }

    // Generate L-shaped path: vertical then horizontal.
    private List<Point> GetVerticalThenHorizontalPath(Point start, Point end)
    {
        var path = new List<Point>();
        var x = start.X;
        var y = start.Y;
        while (y != end.Y)
        {
            path.Add(new Point(x, y));
            y += end.Y > y ? 1 : -1;
        }

        while (x != end.X)
        {
            path.Add(new Point(x, y));
            x += end.X > x ? 1 : -1;
        }

        return path;
    }

    // Check if any point in the path (excluding endpoints) lies within any room other than roomA or roomB.
    private bool CorridorPathIntersectsOtherRooms(List<Point> path, Rectangle roomA, Rectangle roomB)
    {
        // Skip the first and last points (they are inside the connecting rooms).
        for (var i = 1; i < path.Count - 1; i++)
        {
            var p = path[i];
            foreach (var room in Rooms)
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
        foreach (var p in path)
            if (p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height)
                Tiles[p.X, p.Y] = TileFactory.Create
                (
                    new Point(p.X, p.Y),
                    TileType.Floor
                );
    }

    // Check 8 neighboring cells to see if any is a floor tile.
    private bool HasAdjacentFloor(int x, int y)
    {
        for (var dx = -1; dx <= 1; dx++)
        for (var dy = -1; dy <= 1; dy++)
        {
            var nx = x + dx;
            var ny = y + dy;
            if (nx < 0 || nx >= Width || ny < 0 || ny >= Height)
                continue;
            if (Tiles[nx, ny].Glyph.Glyph == '.')
                return true;
        }

        return false;
    }

    // Compute Euclidean distance between two points.
    private double Distance(Point a, Point b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}