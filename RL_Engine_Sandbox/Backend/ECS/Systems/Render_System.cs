using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

public class Render_System(
    IMapManager mapManager,
    IComponentManager componentManager,
    IFovSystem fovSystem,
    IEventManager eventManager,
    long playerId)
    : IRenderSystem
{
    public Console Console { get; set; }
    public List<Entity> EntitiesToRender { get; set; } = [];

    public void RenderAll()
    {
        var visibleTiles = fovSystem.ComputeFov(playerId);
        RenderMap(visibleTiles);
        RenderEntities(visibleTiles);
        Console.IsDirty = true;

    }

    public Color Darken(Color color)
    {
        return new Color(Math.Max(color.R - 120, 0), Math.Max(color.G - 120, 0), Math.Max(color.B - 120, 0));
    }


    private void RenderEntities(HashSet<Point> visibleTiles)
    {
        foreach (var entity in EntitiesToRender)
        {
            var position = componentManager.GetComponent<PositionComponent>(entity.Id);
            var render = componentManager.GetComponent<RenderingComponent>(entity.Id);

            if (position == null || render == null) continue;
            if (!visibleTiles.Contains(new Point(position.X, position.Y))) continue; // Hide entities not in FOV

            Console.SetGlyph(
                position.X,
                position.Y,
                render.Glyph.Glyph,
                render.Glyph.Foreground,
                render.Glyph.Background);
        }
    }

    private void RenderMap(HashSet<Point> visibleTiles)
    {
        for (var x = 0; x < mapManager.CurrentMap.Width; x++)
        for (var y = 0; y < mapManager.CurrentMap.Height; y++)
        {
            var tile = mapManager.GetTileAt(x, y);

            if (visibleTiles.Contains(new Point(x, y)))
            {
                tile.IsExplored = true;
                Console.SetGlyph(x, y, tile.Glyph.GlyphCharacter, tile.Glyph.Foreground, tile.Glyph.Background);
            }
            else if (tile.IsExplored)
            {
                Console.SetGlyph(x, y, tile.Glyph.GlyphCharacter, Darken(tile.Glyph.Foreground),
                    Darken(tile.Glyph.Background));
            }
            else
            {
                Console.SetGlyph(x, y, ' ', Color.Black, Color.Black);
            }
        }
    }
    public void Initialize()
    {
    
    }
}