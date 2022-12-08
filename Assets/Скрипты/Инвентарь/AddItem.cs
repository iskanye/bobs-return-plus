public class AddItem : UnityEngine.MonoBehaviour
{
    public Item item;

    public void Add() =>
        InventorySystem.Active.AddItem(item);
}
