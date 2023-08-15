using UnityEngine;

public class ExpeditionController : MonoBehaviour
{
    public Item[] items;
    public ExpeditionItem itemButtonPrefab;
    public RectTransform itemsLayout;

    void Start()
    {
        items = Item.AllItems.ToArray();

        foreach (var i in items)
        {
            var item = Instantiate(itemButtonPrefab, itemsLayout);
            item.item = i;
            item.image.sprite = i.icon;
        }
    }    
}
