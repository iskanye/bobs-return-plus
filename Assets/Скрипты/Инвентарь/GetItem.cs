public class GetItem : UnityEngine.MonoBehaviour
{
    public Item item;

    public void AddItem() => InventorySystem.AddItem(item);
}
