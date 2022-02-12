using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject 
{
    public int id;
    public Sprite icon;
    public new string name;
    public bool local;

    public static List<Item> AllItems = new List<Item>();

    public abstract bool Action();

    void OnEnable() =>
        AllItems.Add(this);
}