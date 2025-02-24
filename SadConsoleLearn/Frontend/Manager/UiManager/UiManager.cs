using RL_Engine_Sandbox.Frontend.Interface;

namespace RL_Engine_Sandbox.Frontend.Manager.UiManager
{
    public class UiManager : IUiManager
    {
        public Dictionary<string, IUiElement> UiElements { get; set; }
        public Dictionary<string, UiLayoutConfig> LayoutConfigs { get; set; }
        
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        
        public UiManager()
        {
            UiElements = new Dictionary<string, IUiElement>();
            LayoutConfigs = new Dictionary<string, UiLayoutConfig>();
        }
        
        public void AddUiElement(string name, IUiElement uiElement, UiLayoutConfig layoutConfig)
        {
            UiElements[name] = uiElement;
            LayoutConfigs[name] = layoutConfig;
        }
        
        public void RemoveUiElement(string name)
        {
            UiElements.Remove(name);
            LayoutConfigs.Remove(name);
        }
        
        public IUiElement GetUiElement(string name)
        {
            return UiElements[name];
        }
        
        public void SetScreenSize(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            ApplyLayout();
        }
        
        public void ApplyLayout()
        {
            foreach (var pair in UiElements)
            {
                var name = pair.Key;
                var uiElement = pair.Value;
                
                if (!LayoutConfigs.TryGetValue(name, out var config))
                    continue;
                
                int absoluteX = (int)(ScreenWidth * config.RelativeX);
                int absoluteY = (int)(ScreenHeight * config.RelativeY);
                int absoluteWidth = (int)(ScreenWidth * config.RelativeWidth);
                int absoluteHeight = (int)(ScreenHeight * config.RelativeHeight);
                
                uiElement.ReSize(absoluteWidth, absoluteHeight);
                uiElement.Initialize();
                
                // Set the underlying console's position.
                uiElement.GetConsole().Position = new Point(absoluteX, absoluteY);
            }
        }
        
        public void Update()
        {
            foreach (var uiElement in UiElements.Values)
            {
                uiElement.Update();
            }
        }
        
        public void Render()
        {
            foreach (var uiElement in UiElements.Values)
            {
                uiElement.Render();
            }
        }
    }
}
