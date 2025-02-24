using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

public class InventorySystem : ISystem
{
    public void AddItem() {
        // Check if the item exists
        // Check if the item can be added to the inventory
        // If it can, add the item to the inventory
    }

    private void RemoveItem() {
        // Check if the item exists
        // Remove the item from the inventory
    }

    public void UseItem() {
        // Check if the item exists
        // Check if the item is usable
        // Check if the requirements are met
        // If they are, use the item
    }
    public void DropItem() {
        // Check if the item exists
        // Proceed to remove the item from the inventory
        // and add it to the current map at the same position as the actor
        // and remove it from the inventory
        
    }
    public void EquipItem() {
        // Check if the item exists
        // Check if the item can be equipped
        // Check if the item is already equipped
        // If it not, equip it
        
    }
    public void UnEquipItem() {
        // Check if the item exists
        // Check if the item is equipped
        // If it is, un-equip it
    }
}