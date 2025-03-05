using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public class MessageEvent(string message, Color color) : IEvent
{
    public string Message { get; set; } = message;
    public Color MessageColor { get; set; } = color;
    public bool IsImportant { get; set; } = false;
}