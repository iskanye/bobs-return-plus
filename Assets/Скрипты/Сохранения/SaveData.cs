using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int level = 0;
    public int lives = 3;
    public List<Position> positions;
    public List<CustomProperty> customProperties;
    public List<CustomProperty> globalProperties;
    public List<IdItem> achievements;
    public List<IdItem> inventory;

    public SaveData() { }
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
public class IdItem
{
    public int id;

    public IdItem(int id) => this.id = id;
}