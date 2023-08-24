public abstract class GUIDHolder : UnityEngine.MonoBehaviour
{
    public string id;
    
    public void Reset() => 
        id = System.Guid.NewGuid().ToString();
}
