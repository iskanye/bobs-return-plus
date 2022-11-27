using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
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

    public int Index { get; set; }

    void Awake()
    {
        Index = 0;
        var inventory = SceneData.Data.inventory;
        items = new Item[maxItems];

        if (inventory != null)
            for (int i = 0; i < (inventory.Count > maxItems ? maxItems : inventory.Count); i++)
                items[i] = Item.GetItem(inventory[i].id);

        InputManager.Input.Player.UseItem.started += i =>
        {
            if (items[Index] == null || !(DialogueSystem.Active.state is Dialogues.IdleState))
                return;

            if (items[Index].Action())
                items[Index] = null;
        };

        InputManager.Input.Player.ItemChoose.started += i => Index = (Index + 1) % 4;
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
        }

        label.text = items[Index] != null ? items[Index].name : "";
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < maxItems; i++)
            if (items[i] == null)
            {
                var pickUp = pickUpDialogue;
                pickUp.dialogues[0].text.Replace("{}", item.name.ToLower());
                activator.dialogues = pickUp;
                activator.Dialogue();

                items[i] = item;
                return;
            }

        activator.dialogues = fullDialogue;
        activator.Dialogue();
    }
}
