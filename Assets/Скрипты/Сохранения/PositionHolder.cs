public class PositionHolder : UnityEngine.MonoBehaviour
{
    public int id
    {
        get => gameObject.GetHashCode();
    }
}
