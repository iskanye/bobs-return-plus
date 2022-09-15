using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public const string currentVersion = "Battle Test 11";
    public string version = currentVersion;

    public int level;
    public int lives = -1;

    public List<Position> positions;
    public List<Integer> integers;

    public List<CustomProperty> customProperties;
    public List<CustomProperty> globalProperties;

    public List<IdItem> achievements;
    public List<GuidItem> inventory;

    public SaveData() 
    {
        customProperties = new List<CustomProperty>();
        globalProperties = new List<CustomProperty>();
        positions = new List<Position>();
        integers = new List<Integer>();
        achievements = new List<IdItem>();
        inventory = new List<GuidItem>();
    }
}

[Serializable]
public class Position 
{
    public string id;
    public Vector3 position;

    public Position() { }

    public Position(string id, Vector3 position)
    {
        this.id = id;
        this.position = position;
    }
}

[Serializable]
public class CustomProperty
{
    public string id;
    public bool property;

    public CustomProperty() { }

    public CustomProperty(string id, bool property)
    {
        this.id = id;
        this.property = property;
    }
}

[Serializable]
public class Integer
{
    public string id;
    public int integer;

    public Integer() { }

    public Integer(string id, int integer)
    {
        this.id = id;
        this.integer = integer;
    }
}

[Serializable]
public class IdItem : IEquatable<IdItem>
{
    public int id;

    public IdItem(int id) => this.id = id;

    public bool Equals(IdItem i)
    {
        if (id == i.id)
            return true;

        return false;
    }
}

[Serializable]
public class GuidItem : IEquatable<GuidItem>
{
    public string id;

    public GuidItem(string id) => this.id = id;

    public bool Equals(GuidItem i)
    {
        if (id == i.id)
            return true;

        return false;
    }
}
