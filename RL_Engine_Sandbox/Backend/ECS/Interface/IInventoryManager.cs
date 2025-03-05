using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Entities;

namespace RL_Engine_Sandbox.Backend.ECS.Interface;

public interface IInventoryManager
{
    void AddItem(Entity item, InventoryComponent inventory);
    void DropItem();
    void EquipItem();
    void UnEquipItem();
    List<string> GetPlayerInventoryItems(long playerId);

}