using RL_Engine_Sandbox.Backend;
using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Manager;

namespace RL_Engine_Sandbox.Frontend;

public class MapManager : IMapManager
{
    private readonly IMap _map;
    private readonly Console _console;

    public MapManager(IMap map, Console console)
    {
        _map = map;
        _console = console;
    }
    public bool IsTileWalkable(int x, int y) {
        if (x < 1 || x >= _map.Width-1 || y < 1 || y >= _map.Height-1) return false;
        
        return _map.Tiles[x, y].IsWalkable;
    }
    public void Render()
    {
        for (int x = 0; x < _map.Width; x++)
        {
            for (int y = 0; y < _map.Height; y++)
            {
                var tile = _map.Tiles[x, y];
                _console.SetGlyph(x, y, tile.Glyph.GlyphCharacter, tile.Glyph.Foreground, tile.Glyph.Background);
            }
        }

        _console.IsDirty = true; 
    }
}