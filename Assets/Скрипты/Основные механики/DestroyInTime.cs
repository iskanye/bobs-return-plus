public class DestroyInTime : UnityEngine.MonoBehaviour
{
    public float time;
    void Start() => Invoke("InTime", time);
    void InTime() => Destroy(gameObject);
}
