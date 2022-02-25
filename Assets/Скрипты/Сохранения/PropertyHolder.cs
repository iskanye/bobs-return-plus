public class PropertyHolder : UnityEngine.MonoBehaviour
{
    public string id;
    public bool property { set; get; }
    public UnityEngine.Events.UnityEvent<bool> action;

    void Reset() => 
        id = System.Guid.NewGuid().ToString();
}
