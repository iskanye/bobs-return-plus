using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement", order = 0)]
public class Achievement : ScriptableObject 
{
    public Sprite icon;
    public string title;
    public string description;
    public int rarity; // 0 - обычное. 1 - редкое. 2 - супер редкое и т.д
    public string id;

    void Reset() =>
        id = System.Guid.NewGuid().ToString();
}