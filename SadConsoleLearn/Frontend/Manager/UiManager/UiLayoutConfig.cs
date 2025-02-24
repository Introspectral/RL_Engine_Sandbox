namespace RL_Engine_Sandbox.Frontend.Manager.UiManager;

public class UiLayoutConfig
{
    public float RelativeX { get; set; }
    public float RelativeY { get; set; }
    public float RelativeWidth { get; set; }
    public float RelativeHeight { get; set; }
    
    public UiLayoutConfig(float relativeX, float relativeY, float relativeWidth, float relativeHeight)
    {
        RelativeX = relativeX;
        RelativeY = relativeY;
        RelativeWidth = relativeWidth;
        RelativeHeight = relativeHeight;
    }
}