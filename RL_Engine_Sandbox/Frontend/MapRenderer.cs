using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Frontend;

public class MapRenderer
{
    private readonly IMap _dungeonMap;
    private readonly Console _console;

    public MapRenderer(IMap dungeonMap, Console console)
    {
        _dungeonMap = dungeonMap;
        _console = console;
    }

    public void Render()
    {
        for (int x = 0; x < _dungeonMap.Width; x++)
        {
            for (int y = 0; y < _dungeonMap.Height; y++)
            {
                var tile = _dungeonMap.Tiles[x, y];
                _console.SetGlyph(x, y, tile.Glyph.GlyphCharacter, tile.Glyph.Foreground, tile.Glyph.Background);
            }
        }
        _console.IsDirty = true; 
    }
}