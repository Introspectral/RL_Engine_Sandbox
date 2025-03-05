namespace RL_Engine_Sandbox.Backend.ECS.Entities.Item;

public class ItemInfo
{
    public long Id { get; }
    public string Name { get; }
    public Color Color { get; }

    public ItemInfo(long id, string name, Color color)
    {
        Id = id;
        Name = name;
        Color = color;
    }
}