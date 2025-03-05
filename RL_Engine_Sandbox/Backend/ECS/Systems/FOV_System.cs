using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

public class FOV_System(IMapManager mapManager, IComponentManager componentManager) : IFovSystem
{
    public HashSet<Point> ComputeFov(long entityId)
    {
        var fovComponent = componentManager.GetComponent<FovComponent>(entityId);
        var posComponent = componentManager.GetComponent<PositionComponent>(entityId);
        if (fovComponent == null || posComponent == null)
            return [];

        var visibleTiles = new HashSet<Point>();

        var radius = fovComponent.Radius;
        for (var y = posComponent.Y - radius; y <= posComponent.Y + radius; y++)
        for (var x = posComponent.X - radius; x <= posComponent.X + radius; x++)
        {
            if (x < 0 || x >= mapManager.Width || y < 0 || y >= mapManager.Height) continue;
            double dx = x - posComponent.X;
            double dy = y - posComponent.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            if (!(distance <= radius) ||
                !HasLineOfSight(new Point(posComponent.X, posComponent.Y), new Point(x, y))) continue;
            var tile = mapManager.GetTileAt(x, y);
            tile.IsInFov = true;
            tile.IsExplored = true;
            visibleTiles.Add(new Point(x, y));
        }

        return visibleTiles;
    }

    public bool HasLineOfSight(Point start, Point end)
    {
        int x0 = start.X, y0 = start.Y, x1 = end.X, y1 = end.Y;
        int dx = Math.Abs(x1 - x0), dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1, sy = y0 < y1 ? 1 : -1;
        var err = dx - dy;

        while (true)
        {
            var tile = mapManager.GetTileAt(x0, y0);
            tile.IsExplored = true;
            if (x0 == x1 && y0 == y1) break;
            if (!tile.IsWalkable) return false;
            var e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        return true;
    }
}