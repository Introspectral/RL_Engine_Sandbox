using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Manager;

public class InventoryManager : IInventoryManager
{
    private readonly IComponentManager _componentManager;

    public InventoryManager(IComponentManager componentManager)
    {
        _componentManager = componentManager;
    }

    public void AddItem(Entity item, InventoryComponent inventory)
    {
        inventory.Items.Add(item);
    }

    private void RemoveItem()
    {
        // Check if the item exists
        // Remove the item from the inventory
    }

    public void UseItem()
    {
        // Check if the item exists
        // Check if the item is usable
        // Check if the requirements are met
        // If they are, use the item
    }

    public void DropItem()
    {
        // Check if the item exists
        // Proceed to remove the item from the inventory
        // and add it to the current map at the same position as the actor
        // and remove it from the inventory
    }

    public void EquipItem()
    {
        // Check if the item exists
        // Check if the item can be equipped
        // Check if the item is already equipped
        // If it not, equip it
    }

    public void UnEquipItem()
    {
        // Check if the item exists
        // Check if the item is equipped
        // If it is, un-equip it
    }

    public List<string> GetPlayerInventoryItems(long playerId)
    {
        throw new NotImplementedException();
    }
}