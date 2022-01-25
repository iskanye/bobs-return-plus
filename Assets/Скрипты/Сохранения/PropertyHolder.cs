using UnityEngine;

public class PropertyHolder : MonoBehaviour
{
    public int id
    {
        get => gameObject.GetHashCode();
    }
    public bool property { set; get; }
    public UnityEngine.Events.UnityEvent<bool> action;
}
