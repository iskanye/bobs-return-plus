using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Active { get; private set; }

    public PlayerWarp warp;
    public UninteractiveDialogue pickUpDialogue;
    public UninteractiveDialogue fullDialogue;
    public UninteractiveDialogueActivator activator;
    public TMPro.TMP_Text label;

    public ContainerController container;
    
    public System.Action<Item> onItemUse;
    public int Index { get; set; }

    void Awake()
    {
        Active = this;
        warp = FindObjectOfType<PlayerWarp>();
                
        Index = 0;
        var inventory = SceneData.Data.inventory;
        container.items = new Item[container.maxItems];

        if (inventory != null)
            for (int i = 0; i < (inventory.Count > container.maxItems ? container.maxItems : inventory.Count); i++)
            {
                var item = Item.GetItem(inventory[i].id);
                item.mn = this;
                container.items[i] = item;
            }

        InputManager.Input.Player.UseItem.started += i => Use(Index);
        InputManager.Input.Player.ItemChoose.started += i => Index = (Index + 1) % container.maxItems;
        InputManager.Input.Player.ThrowAway.started += i => container.items[Index] = null;
    }

    void Update()
    {
        for (int i = 0; i < container.maxItems; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                Index = i;

        label.text = container.items[Index] != null ? container.items[Index].name : "";
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < container.maxItems; i++)
            if (container.items[i] == null)
            {
                var pickUp = pickUpDialogue;
                pickUp.dialogues[0].text = pickUp.dialogues[0].text.Replace("{", item.name.ToLower());
                activator.dialogues = pickUp;
                activator.Dialogue();
                pickUp.dialogues[0].text = pickUp.dialogues[0].text.Replace(item.name.ToLower(), "{");

                item.mn = this;
                container.items[i] = item;
                return true;
            }

        activator.dialogues = fullDialogue;
        activator.Dialogue();
        return false;
    }

    public void Use(int index)
    {
        if (container.items[index] == null || DialogueSystem.Active.state is not Dialogues.IdleState)
            return;

        if (container.items[index].Action())
        {
            if (onItemUse != null)
                onItemUse.Invoke(container.items[index]);
                
            container.items[index] = null;
        }
    }
}
