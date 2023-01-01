using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string id;
    public Sprite icon;
    public new string name;

    [HideInInspector] public InventorySystem mn;
    
    public static List<Item> AllItems = new List<Item>();

    public static Item GetItem(string id) =>
        AllItems.First(i => i.id == id);

    public abstract bool Action();

    void Reset() =>
        id = System.Guid.NewGuid().ToString();

    void OnEnable() =>
        AllItems.Add(this);
}