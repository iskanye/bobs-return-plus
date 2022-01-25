public class GetItem : UnityEngine.MonoBehaviour
{
    public Item item;

    public void AddItem() => InventorySystem.Active.AddItem(item);
}
