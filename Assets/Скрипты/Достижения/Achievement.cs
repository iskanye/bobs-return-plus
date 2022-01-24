using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement", order = 0)]
public class Achievement : ScriptableObject 
{
    public Sprite icon;
    public string title;
    public string description;
    public int rarity; // 0 - обычное. 1 - редкое. 2 - супер редкое и т.д
    public int id;
}

[Serializable]
public class AchievementJson
{
    public int id;

    public AchievementJson(int id) => this.id = id;
}