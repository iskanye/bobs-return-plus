using UnityEngine;

public class LivesBase : MonoBehaviour
{
    public virtual int Lives { get; set; }
    public System.Func<int, int> livesCalculation;
    [HideInInspector] public Vector2 hitDirection;

    public ObjectType Durability { protected set; get; }
}
