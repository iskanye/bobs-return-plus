using UnityEngine;

public class PropertyHolder : MonoBehaviour
{
    public new string name;
    public bool property { set; get; }
    public UnityEngine.Events.UnityEvent<bool> action;
}
