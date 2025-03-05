using RL_Engine_Sandbox.Backend.ECS.Components;

namespace RL_Engine_Sandbox.Backend.ECS.Entities.Actors;

public class EntityInfo
{
    public long Id { get; set; }
    public string Name { get; set; }
    public ClassComponent.ClassType Class { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Gold { get; set; }
    public InventoryComponent Inventory { get; set; }
}

// This class is used to fetch player information to easily display it in the UI