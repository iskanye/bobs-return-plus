using UnityEngine;

public class PenetratingObject : MonoBehaviour
{
    public ObjectType type;
    public UnityEngine.Events.UnityEvent<GameObject> onPenetrate;

    public ObjectType Durability { set => type = value; }

    public void Penetrate() =>
        onPenetrate.Invoke(gameObject);
}
