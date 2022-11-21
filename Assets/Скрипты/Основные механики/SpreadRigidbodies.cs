using UnityEngine;

public class SpreadRigidbodies : MonoBehaviour
{    
    public Vector3 offset;
    public float force;
    public float spreadRadius;
    public Rigidbody2D[] rigidbodies;

    public void Spread() 
    {
        foreach (var i in rigidbodies)
            Instantiate(i, transform.position + offset + (Vector3)Random.insideUnitCircle * spreadRadius, Quaternion.identity)
                .AddForce(force * Random.onUnitSphere, ForceMode2D.Impulse);
    }
}
