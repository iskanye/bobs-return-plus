using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int maxItems;
    public UnityEngine.UI.Image[] icons;
    public TMPro.TMP_Text[] labels; 
    public Item[] allItems;

    public static InventorySystem Active { get; private set; }

    List<Item> items;
    SceneData data;

    void Start()
    {
        data = SceneData.Active;
        Active = this;
        var inventory = data.data.inventory;
        items = new List<Item>();

        if (inventory != null) foreach (var i in inventory)
            items.Add(allItems.First(p => p.id == i.id));
    }

    void Update()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (i < items.Count)
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
        if (items.Count >= maxItems)
            return;
        items.Add(item);
        data.data.inventory.Add(new IdItem(item.id));
    }
}
