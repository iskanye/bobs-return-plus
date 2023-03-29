using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Active { get; private set; }

    public PlayerWarp warp;
    public int maxItems;
    public Image[] icons;
    public Image[] panels;
    public Sprite regular;
    public Sprite selected;
    public UninteractiveDialogue pickUpDialogue;
    public UninteractiveDialogue fullDialogue;
    public UninteractiveDialogueActivator activator;
    public TMPro.TMP_Text label;

    [HideInInspector] public Item[] items;
    
    public System.Action<Item> onItemUse;
    public int Index { get; set; }

    void Awake()
    {
        Active = this;
        warp = FindObjectOfType<PlayerWarp>();
                
        Index = 0;
        var inventory = SceneData.Data.inventory;
        items = new Item[maxItems];

        if (inventory != null)
            for (int i = 0; i < (inventory.Count > maxItems ? maxItems : inventory.Count); i++)
            {
                var item = Item.GetItem(inventory[i].id);
                item.mn = this;
                items[i] = item;
            }

        InputManager.Input.Player.UseItem.started += i => Use(Index);
        InputManager.Input.Player.ItemChoose.started += i => Index = (Index + 1) % maxItems;
        InputManager.Input.Player.ThrowAway.started += i => items[Index] = null;
    }

    void Update()
    {
        for (int i = 0; i < maxItems; i++)
        {
            if (items[i] != null)
            {
                icons[i].sprite = items[i].icon;
                icons[i].color = Color.white;
            }

            else
                icons[i].color = new Color(0, 0, 0, 0);

            panels[i].sprite = i == Index ? selected : regular;

            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                Index = i;
        }

        label.text = items[Index] != null ? items[Index].name : "";
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < maxItems; i++)
            if (items[i] == null)
            {
                var pickUp = pickUpDialogue;
                pickUp.dialogues[0].text = pickUp.dialogues[0].text.Replace("{", item.name.ToLower());
                activator.dialogues = pickUp;
                activator.Dialogue();
                pickUp.dialogues[0].text = pickUp.dialogues[0].text.Replace(item.name.ToLower(), "{");

                item.mn = this;
                items[i] = item;
                return true;
            }

        activator.dialogues = fullDialogue;
        activator.Dialogue();
        return false;
    }

    public void Use(int index)
    {
        if (items[index] == null || !(DialogueSystem.Active.state is Dialogues.IdleState))
            return;

        if (items[index].Action())
        {
            if (onItemUse != null)
                onItemUse.Invoke(items[index]);
                
            items[index] = null;
        }
    }
}
