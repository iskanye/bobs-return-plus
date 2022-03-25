using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public int maxItems;
    public Image[] icons;
    public Image[] panels;
    public Sprite regular;
    public Sprite selected;
    public TMPro.TMP_Text label;

    [HideInInspector] public Item[] items;

    public int Index { get; set; }

    void Awake()
    {
        var inventory = SceneData.Data.inventory;
        items = new Item[maxItems];

        if (inventory != null)
            for (int i = 0; i < (inventory.Count > maxItems ? maxItems : inventory.Count); i++)
                items[i] = Item.GetItem(inventory[i].id);
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

            if (Input.GetKeyDown((KeyCode)(49 + i)))
                if (i == Index)
                    Use(i);

                else
                    Index = i;
        }

        label.text = items[Index] != null ? items[Index].name : "";
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < maxItems; i++)
            if (items[i] == null)
            {
                items[i] = item;
                break;
            }
    }

    public void Use(int item)
    {
        if (items[item] == null || !(DialogueSystem.Active.state is Dialogues.IdleState))
            return;

        if (items[item].Action())
            items[item] = null;
    }
}
