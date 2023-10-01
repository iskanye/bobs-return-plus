using UnityEngine;

public class ContainerController : MonoBehaviour
{
    public int maxItems;
    public ItemButton[] itemButtons;
    public System.Action<Item, ItemButton> onItemChange;

    public Item this[int index] 
    {
        set =>
            itemButtons[index].item = value;
        get =>
            itemButtons[index].item;
    }
}
