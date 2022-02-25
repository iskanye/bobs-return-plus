using UnityEngine;
using System.Linq;

public class InventorySystem : MonoBehaviour
{
    public int maxItems;
    public UnityEngine.UI.Image[] icons;
    public TMPro.TMP_Text[] labels;

    public static InventorySystem Active { get; private set; }

    Item[] items;

    void Awake()
    {
        Active = this;

        var inventory = SceneData.Data.inventory;
        items = new Item[maxItems];

        var allItems = Item.AllItems.OrderBy(j => j.id).ToArray();

        if (inventory != null)
            for (int i = 0; i < (inventory.Count > maxItems ? maxItems : inventory.Count); i++)
                items[i] = allItems[inventory[i].id];
    }

    void Update()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (items[i] != null)
            {
                icons[i].sprite = items[i].icon;
                icons[i].color = Color.white;
                labels[i].text = items[i].name;
            }

            else
            {
                icons[i].color = new Color(0, 0, 0, 0);
                labels[i].text = "";
            }
        }
    }

    public static void AddItem(Item item)
    {
        var active = Active;

        for (int i = 0; i < active.maxItems; i++)
            if (active.items[i] == null)
            {
                active.items[i] = item;
                SceneData.Data.inventory.Add(new IdItem(item.id));
                break;
            }
    }

    public void Use(int item)
    {
        if (items[item] == null)
            return;

        if (items[item].Action())
            items[item] = null;
    }
}
