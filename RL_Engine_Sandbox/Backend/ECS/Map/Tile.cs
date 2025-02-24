using Color = SadRogue.Primitives.Color;

namespace RL_Engine_Sandbox.Backend.ECS.Map
{
    public enum TileType {
        // Basic types
        Empty,
        Wall,
        Floor,

    }

    public class Tile {
        public Point Position { get; }
        public bool IsWalkable { get; }
        public ColoredGlyph Glyph { get; }
        public TileType TileType { get; }

        // Original constructor that accepts a ColoredGlyph.
        public Tile(Point position, bool isWalkable, ColoredGlyph glyph, TileType tileType) {
            Position = position;
            IsWalkable = isWalkable;
            Glyph = glyph;
            // Assign the colors from the glyph so they’re available separately.
            TileType = tileType;
        }
        // Overloaded constructor that accepts a char and colors.
        public Tile(Point position, bool isWalkable, char glyph, Color foreground, Color background, TileType tileType)
            : this(position, isWalkable, new ColoredGlyph(foreground, background, glyph), tileType) {}
    }

    public static class TileFactory {
        public static Tile Create(Point position, TileType type, char? customGlyph = null) {
            return type switch {
                // Basic types
                TileType.Empty => new Tile(position, false, ' ',Color.Black, Color.Black, type),
                TileType.Wall => new Tile(position, false, '#', Color.Gray, Color.Black, type),
                TileType.Floor => new Tile(position, true, '.', Color.LightGray, Color.Black, type),
                _ => new Tile(position, false, '?', Color.Magenta, Color.Black, type)
            };
        }
    }
}
