namespace RL_Engine_Sandbox.Backend.ECS.Map;

public static class TileFactory
{
    public static Tile Create(Point position, TileType type, char? customGlyph = null)
    {
        return type switch
        {
            TileType.Empty => new Tile(position, false, false, false, ' ', Color.Black, Color.Black, type),
            TileType.Wall => new Tile(position, false, false, false, '#', Color.Gray, Color.Black, type),
            TileType.Floor => new Tile(position, true, false, false, '.', Color.LightGray, Color.Black, type),
            _ => new Tile(position, false, false, false, '?', Color.Magenta, Color.Black, type)
        };
    }
}