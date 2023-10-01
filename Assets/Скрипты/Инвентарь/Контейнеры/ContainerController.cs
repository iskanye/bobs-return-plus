using UnityEngine;

public class ContainerController : MonoBehaviour
{
    public int maxItems;
    public Item[] items;
    public UnityEngine.UI.Image[] icons;

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
        }
    }
}
