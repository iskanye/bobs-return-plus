using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Item : GUIDScriptableObject
{
    public Sprite icon;
    public new string name;
    public string description;

    [HideInInspector] public InventorySystem mn;
    
    public static List<Item> AllItems = new List<Item>();

    public static Item GetItem(string id) =>
        AllItems.First(i => i.id == id);

    public abstract bool Action();

    void OnEnable() =>
        AllItems.Add(this);
}