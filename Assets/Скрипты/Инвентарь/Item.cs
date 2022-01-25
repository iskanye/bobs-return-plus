using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject 
{
    public int id;
    public Sprite icon;
    public new string name;
    public bool local;
    public UnityEngine.Events.UnityEvent action;
}