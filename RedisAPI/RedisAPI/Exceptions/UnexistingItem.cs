namespace RedisAPI.Exceptions;

public class UnexistingItem : Exception
{
    public string ItemId { get; set; }

    public UnexistingItem(string itemId) : base("unexisting_item")
    {
        ItemId = itemId;
    }
}