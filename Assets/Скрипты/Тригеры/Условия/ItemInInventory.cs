using System.Linq;

public class ItemInInventory : ConditionBase
{
    public string id;

    public override bool Condition(UnityEngine.GameObject obj) => 
        InventorySystem.Active.container.itemButtons.Any(i => i != null && i.item.id == id);
}