using RL_Engine_Sandbox.Backend.ECS.Components;
using RL_Engine_Sandbox.Backend.ECS.Event;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Backend.ECS.Systems;

public class Interaction_System
{
    private readonly IComponentManager _componentManager;
    private readonly IEntityManager _entityManager;
    private readonly IEventManager _eventManager;
    private readonly IInventoryManager _inventoryManager;
    private readonly IMapManager _mapManager;
    public Interaction_System(IEventManager eventManager, IEntityManager entityManager,
        IComponentManager componentManager, IInventoryManager inventoryManager, IMapManager mapManager)
    {
        _eventManager = eventManager;
        _entityManager = entityManager;
        _componentManager = componentManager;
        _inventoryManager = inventoryManager;
        _mapManager = mapManager;
        
        _eventManager.Subscribe<PickUpEvent>(OnPickUp);
        _eventManager.Subscribe<UseStairsEvent>(OnUseStairs);
    }

 private void OnUseStairs(UseStairsEvent useStairsEvent)
        {
            var player = _entityManager.GetById(useStairsEvent.EntityId);
            if (player == null)
                return;

            var playerPos = _componentManager.GetComponent<PositionComponent>(player.Id);
            if (playerPos == null)
                return;

            // Look for a stairs entity at the player's current position.
            var stairsEntity = _entityManager.GetAllEntities().FirstOrDefault(e =>
            {
                var pos = _componentManager.GetComponent<PositionComponent>(e.Id);
                var stairsComp = _componentManager.GetComponent<StairsComponent>(e.Id);
                return pos != null && stairsComp != null &&
                       pos.X == playerPos.X && pos.Y == playerPos.Y;
            });

            if (stairsEntity == null)
            {
                _eventManager.Publish(new MessageEvent("There are no stairs here to use!", Color.Red));
                return;
            }

            var stairsComponent = _componentManager.GetComponent<StairsComponent>(stairsEntity.Id);
            if (stairsComponent == null)
                return;

            // Ensure the stairs type matches the player's intended direction.
            // (For example, if the player pressed the key to go down,
            // they should be on a stairs-down tile.)
            if (stairsComponent.Direction != useStairsEvent.Direction)
            {
                _eventManager.Publish(new MessageEvent("You are not on the correct stairs to go that way!", Color.Red));
                return;
            }

            // Determine new level based on current level and stairs direction.
            int currentLevel = _mapManager.CurrentLevel;
            int maxLevel = _mapManager.Maps.Count; // Assuming Maps is accessible.
            int newLevel = currentLevel;

            if (useStairsEvent.Direction == StairDirection.Down)
            {
                if (currentLevel >= maxLevel)
                {
                    _eventManager.Publish(new MessageEvent("You cannot go any further down!", Color.Red));
                    return;
                }
                newLevel = currentLevel + 1;
            }
            else if (useStairsEvent.Direction == StairDirection.Up)
            {
                if (currentLevel <= 1)
                {
                    _eventManager.Publish(new MessageEvent("You cannot go any further up!", Color.Red));
                    return;
                }
                newLevel = currentLevel - 1;
            }

            // Change the active map.
            _mapManager.SetCurrentLevel(newLevel);

            // On the new map, look for the corresponding stairs.
            // For example, if the player used stairs down (leaving a stairs-down behind),
            // we expect the new map to have stairs up at the same coordinates.
            var correspondingStairs = _entityManager.GetAllEntities().FirstOrDefault(e =>
            {
                var pos = _componentManager.GetComponent<PositionComponent>(e.Id);
                var comp = _componentManager.GetComponent<StairsComponent>(e.Id);
                if (pos == null || comp == null)
                    return false;
                // Look for the opposite type.
                if (useStairsEvent.Direction == StairDirection.Down)
                    return comp.Direction == StairDirection.Up && pos.X == playerPos.X && pos.Y == playerPos.Y;
                else
                    return comp.Direction == StairDirection.Down && pos.X == playerPos.X && pos.Y == playerPos.Y;
            });

            if (correspondingStairs != null)
            {
                var newPos = _componentManager.GetComponent<PositionComponent>(correspondingStairs.Id);
                if (newPos != null)
                {
                    playerPos.X = newPos.X;
                    playerPos.Y = newPos.Y;
                }
                else
                {
                    // Fallback if no valid position is found.
                    var spawn = _mapManager.FindValidSpawnPoint();
                    playerPos.X = spawn.X;
                    playerPos.Y = spawn.Y;
                }
            }
            else
            {
                // Fallback: use a valid spawn point from the new map.
                var spawn = _mapManager.FindValidSpawnPoint();
                playerPos.X = spawn.X;
                playerPos.Y = spawn.Y;
            }

            _eventManager.Publish(new MessageEvent($"You have moved to level {newLevel}.", Color.Green));
        }
    

    private void OnPickUp(PickUpEvent pickUpEvent)
    {
        var player = _entityManager.GetById(pickUpEvent.EntityId);
        if (player == null) return;

        var position = _componentManager.GetComponent<PositionComponent>(player.Id);
        if (position == null) return;

        // Find an item at the player's position
        var item = _entityManager.GetAllEntities()
            .FirstOrDefault(e =>
                _componentManager.GetComponent<ItemComponent>(e.Id) != null &&
                _componentManager.GetComponent<PositionComponent>(e.Id)?.X == position.X &&
                _componentManager.GetComponent<PositionComponent>(e.Id)?.Y == position.Y);

        if (item == null)
        {
            _eventManager.Publish(new MessageEvent("There is nothing to pick up here.", Color.Red));
            return;
        }

        var itemName = _componentManager.GetComponent<NameComponent>(item.Id)?.Name;
        
        // Add to player's inventory (if inventory exists)
        var inventory = _componentManager.GetComponent<InventoryComponent>(player.Id);
        if (inventory == null) return;
        _inventoryManager.AddItem(item, inventory);
        _eventManager.Publish(new MessageEvent($"Your inventory now holds {inventory.Items.Count} items", Color.Green));
        // Remove the item from the map
        _eventManager.Publish(new MessageEvent($"You picked up {itemName}!", Color.Green));
        _eventManager.Publish<RemoveEntityEvent>(new RemoveEntityEvent(item.Id));
    }
}