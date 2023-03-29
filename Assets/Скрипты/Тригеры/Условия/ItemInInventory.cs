using System.Linq;

public class ItemInInventory : ConditionBase
{
    public string id;

    public override bool Condition() => 
        InventorySystem.Active.items.Any(i => i != null && i.id == id);
}