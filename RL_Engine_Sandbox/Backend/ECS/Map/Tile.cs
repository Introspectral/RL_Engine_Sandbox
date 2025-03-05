using Color = SadRogue.Primitives.Color;

namespace RL_Engine_Sandbox.Backend.ECS.Map;

public enum TileType
{
    Empty,
    Wall,
    Floor
}

public class Tile(
    Point position,
    bool isWalkable,
    bool isExplored,
    bool isInFov,
    ColoredGlyph glyph,
    TileType tileType)
{
    public Tile(Point position, bool isWalkable, bool isExplored, bool isInFov, char glyph, Color foreground,
        Color background, TileType tileType)
        : this(position, isWalkable, isExplored, isInFov, new ColoredGlyph(foreground, background, glyph), tileType)
    {
    }

    public Point Position { get; } = position;
    public bool IsWalkable { get; } = isWalkable;
    public bool IsExplored { get; set; } = isExplored;
    public bool IsInFov { get; set; } = isInFov;

    public ColoredGlyph Glyph { get; } = glyph;
    public TileType TileType { get; } = tileType;
}