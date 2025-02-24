using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class RenderingComponent : IComponents
{
    public ColoredGlyph Glyph { get; set; }

    public RenderingComponent(ColoredGlyph glyph)
    {
        Glyph = glyph;
    }
}