using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Frontend.Interface;
using RL_Engine_Sandbox.Frontend.UI.Screens.SubScreens;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

public class MessageLog_System : IMessageLogSystem
{
    private readonly int _maxMessages;
    private readonly Queue<Color> _messageColors;
    private readonly Queue<string> _messages;
    private readonly List<string> _savedMessages;
    private readonly IUiManager _uiManager;
    private MessageLogPanel? _messageLogPanel;

    public MessageLog_System(IEventManager eventManager, IUiManager uiManager, int maxMessages = 10)
    {
        _messages = new Queue<string>();
        _savedMessages = new List<string>();
        _maxMessages = maxMessages;
        _uiManager = uiManager;
        _messageColors = new Queue<Color>();

        eventManager.Subscribe<MessageEvent>(OnMessageReceived);
    }

    private void OnMessageReceived(MessageEvent messageEvent)
    {
        if (_messageLogPanel == null)
        {
            _messageLogPanel = _uiManager.GetUiElement("messageLogPanel") as MessageLogPanel;
            if (_messageLogPanel == null) return;
        }

        if (_messages.Count >= _maxMessages)
        {
            _messages.Dequeue(); // ✅ Remove oldest message
            _messageColors.Dequeue(); // ✅ Remove corresponding color
        }

        _messages.Enqueue(messageEvent.Message);
        _messageColors.Enqueue(messageEvent.MessageColor); // ✅ Store color correctly

        UpdateMessageLog();
    }


    private void UpdateMessageLog()
    {
        if (_messageLogPanel == null) return;

        _messageLogPanel.ClearMessages();

        var y = _messageLogPanel.GetContentConsole().Height - _messages.Count; // Start from bottom
        var messagesArray = _messages.ToArray();
        var colorsArray = _messageColors.ToArray();

        for (var i = 0; i < messagesArray.Length; i++)
            _messageLogPanel.PrintMessage(messagesArray[i], y++, colorsArray[i]);
    }
}