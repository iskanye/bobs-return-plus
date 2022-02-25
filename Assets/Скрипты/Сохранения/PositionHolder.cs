public class PositionHolder : UnityEngine.MonoBehaviour
{
    public string id;
    
    void Reset() => 
        id = System.Guid.NewGuid().ToString();
}
