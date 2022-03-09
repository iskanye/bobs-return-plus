using UnityEngine;
using System.Linq;

public class InventorySystem : MonoBehaviour
{
    public int maxItems;
    public UnityEngine.UI.Image[] icons;
    public TMPro.TMP_Text[] labels;

    [HideInInspector] public Item[] items;

    void Awake()
    {
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
        if (items[item] == null)
            return;

        if (items[item].Action())
            items[item] = null;
    }
}
