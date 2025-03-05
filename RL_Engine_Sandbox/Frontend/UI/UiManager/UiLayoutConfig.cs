namespace RL_Engine_Sandbox.Frontend.UI.UiManager;

public class UiLayoutConfig(float relativeX, float relativeY, float relativeWidth, float relativeHeight)
{
    public float RelativeX { get; set; } = relativeX;
    public float RelativeY { get; set; } = relativeY;
    public float RelativeWidth { get; set; } = relativeWidth;
    public float RelativeHeight { get; set; } = relativeHeight;
}