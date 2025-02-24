using RL_Engine_Sandbox.Backend.ECS.Entity.Item;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Component;

public class InventoryComponent : IComponents
{
    public Dictionary<string , Item>  Items { get; set; }
    // Needs a list of items and a inventory size
    public InventoryComponent(Dictionary<string , Item> items) {
        Items = items;
    }
    
}