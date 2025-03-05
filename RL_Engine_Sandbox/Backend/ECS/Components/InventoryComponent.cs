using RL_Engine_Sandbox.Backend.ECS.Entities;
using RL_Engine_Sandbox.Backend.ECS.Interface;
using RL_Engine_Sandbox.Backend.ECS.Manager;


namespace RL_Engine_Sandbox.Backend.ECS.Components;

public class InventoryComponent : IComponents
{
    public List<Entity> Items { get; } = new();

    public InventoryComponent()
    {
        Items = [];

    }
}
