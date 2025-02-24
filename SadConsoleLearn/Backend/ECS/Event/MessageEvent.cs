using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Event;

public class MessageEvent : IEvent
{
    public string Message { get; set; }
    public MessageEvent(string message)
    {
        Message = message;
    }
    
}