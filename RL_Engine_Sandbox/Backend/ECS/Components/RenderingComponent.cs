using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class RenderingComponent(ColoredGlyph glyph) : IComponents
{
    public ColoredGlyph Glyph { get; set; } = glyph;
    public Color Color { get; set; } = glyph.Foreground;
}