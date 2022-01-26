using UnityEngine;

public abstract class Item : ScriptableObject 
{
    public int id;
    public Sprite icon;
    public new string name;
    public bool local;

    public abstract bool Action();
}