using RL_Engine_Sandbox.Backend.ECS.Component;

namespace RL_Engine_Sandbox.Backend.ECS.Entity.Actors;

public class PlayerInfo {
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
}

// This class is used to fetch player information to easily display it in the UI