using UnityEngine;

public class AddForce : MonoBehaviour
{
    public Vector2 force;

    public void Add(GameObject p) =>
        p.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
}
