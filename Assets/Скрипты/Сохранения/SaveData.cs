using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public const string currentVersion = "ratking test";
    public string version = currentVersion;

    public int level;
    public int lives = -1;

    public List<Property<Vector3>> positions;
    public List<Property<int>> integers;

    public List<Property<bool>> localProperties;
    public List<Property<bool>> globalProperties;

    public List<GuidItem> achievements;
    public List<GuidItem> inventory;

    public SaveData() 
    {
        localProperties = new List<Property<bool>>();
        globalProperties = new List<Property<bool>>();
        positions = new List<Property<Vector3>>();
        integers = new List<Property<int>>();
        achievements = new List<GuidItem>();
        inventory = new List<GuidItem>();
    }
}

[Serializable]
public class GuidItem : IEquatable<GuidItem>
{
    public string id;

    public GuidItem(string id) => 
        this.id = id;

    public bool Equals(GuidItem i)
    {
        if (id == i.id)
            return true;

        return false;
    }
}

[Serializable]
public class Property<T> : GuidItem
{
    public T property;

    public Property(string id, T property) : base(id) =>
        this.property = property;
}