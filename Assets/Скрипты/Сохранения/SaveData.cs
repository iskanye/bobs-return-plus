using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int level;
    public int lives = 6;

    public List<Position> positions;

    public List<CustomProperty> customProperties;
    public List<CustomProperty> globalProperties;

    public List<IdItem> achievements;
    public List<GuidItem> inventory;

    public SaveData() 
    {
        customProperties = new List<CustomProperty>();
        globalProperties = new List<CustomProperty>();
        positions = new List<Position>();
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
public class IdItem : IEquatable<IdItem>
{
    public int id;

    public IdItem(int id) => this.id = id;

    public bool Equals(IdItem i)
    {
        if (i == null)
            return false;

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
        if (i == null)
            return false;

        if (id == i.id)
            return true;

        return false;
    }
}