using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Active { get; private set; }

    public UninteractiveDialogue fullDialogue;
    public UninteractiveDialogueActivator activator;
    public TMPro.TMP_Text label;

    public ContainerController container;
    
    public System.Action<Item> onItemUse;
    public int Index { get; set; }

    void Awake()
    {
        Active = this;
                
        Index = 0;
        var inventory = SceneData.Data.inventory;

        if (inventory != null)
            for (int i = 0; i < (inventory.Count > container.maxItems ? container.maxItems : inventory.Count); i++)
            {
                var item = Item.GetItem(inventory[i].id);
                item.mn = this;
                container[Index] = item;
            }
    }

    void Update()
    {
        label.text = container[Index] != null ? container[Index].name : "";
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < container.maxItems; i++)
            if (container[i]== null)
            {
                item.mn = this;
                container[i] = item;
                return true;
            }

        activator.dialogues = fullDialogue;
        activator.Dialogue();
        return false;
    }

    public void Use(int index)
    {
        if (container[index] == null || DialogueSystem.Active.state is not Dialogues.IdleState)
            return;

        if (container[index].Action())
        {
            onItemUse?.Invoke(container[index]);                
            container[index] = null;
        }
    }
}
