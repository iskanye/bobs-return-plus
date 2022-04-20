using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    public float speed;
    public virtual bool IsWalking { get; set; }
    public virtual Vector2 Direction { get; set; }
}
